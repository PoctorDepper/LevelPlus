using System;
using System.Collections.Generic;
using System.Linq;
using LevelPlus.Configs;
using LevelPlus.Items;
using LevelPlus.Network;
using LevelPlus.Systems;
using LevelPlus.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using tModPorter;

namespace LevelPlus.Players;

public class LevelPlayer : ModPlayer
{
    public int Level
    {
        get => ExperienceToLevel(Experience);

        set => Experience = LevelToExperience(value);
    }

    public int Points { get; set; }

    public long Experience { get; set; }

    public LocalizedText Description => Mod.GetLocalization("Stats.Level.Tooltip" +
                                                            (Main.netMode == NetmodeID.MultiplayerClient
                                                                ? ".Multiplayer"
                                                                : ""))
        .WithFormatArgs(Level, Life, Mana, Points, GetAverageLevel());

    public LocalizedText ExperienceTooltip => Mod.GetLocalization("Stats.Level.Experience")
        .WithFormatArgs(Experience, LevelToExperience(Level + 1), LevelToExperience(Level + 1) - Experience);

    private int Life => Level * PlayConfiguration.Instance.Level.Life;
    private int Mana => Level * PlayConfiguration.Instance.Level.Mana;

    public static int GetAverageLevel()
    {
        int level = 0;
        int players = 0;

        foreach (var player in Main.ActivePlayers)
        {
            players++;
            level += player.GetModPlayer<LevelPlayer>().Level;
        }

        return players != 0 ? level / players : 0;
    }

    public static int ExperienceToLevel(long experience)
    {
        return Math.Min((int)Math.Floor(Math.Pow(experience / 100f, 5 / 11f)), PlayConfiguration.Instance.Level.Max);
    }

    public static long LevelToExperience(int level)
    {
        return (long)Math.Ceiling(100f * Math.Pow(level, 11 / 5f));
    }

    /// Used explicitly for gaining experience legitimately
    /// <param name="experience">The amount of experience to cause </param>
    /// <param name="teamShare">If the source of the experience gained comes from team share</param>
    public void GainExperience(long experience, bool teamShare = false)
    {
        var priorLevel = Level;

        bool shared = false;

        // Ensure that the packet received was not from a share and that TeamShare is actually enabled
        if (!teamShare && Player.team != 0 && PlayConfiguration.Instance.TeamSharePercentage != 0)
            // Check if there are other players in the team
            shared = Main.player.Any(player => player.whoAmI != Player.whoAmI && player.team ==  Player.team);
        
        // If there are other players in the team, send the team share packet to the server
        if (shared) {
            var packet = new TeamSharePacket
            {
                Amount = (long)Math.Max(1, experience * PlayConfiguration.Instance.TeamSharePercentage),
                Team = Player.team
            };

            packet.Send();
        }

        // If the experience was shared to teammates, subtract the amount
        Experience += (long)(experience * (shared ? 1.0 - PlayConfiguration.Instance.TeamSharePercentage : 1.0));

        // As for why these are CombatText instead of PopupText, I just like the look more
        // Show experience gain popup
        if (Main.dedServ) return;
        var xpText = Main.combatText[CombatText.NewText(Player.getRect(), Color.Lime,
            Mod.GetLocalization("Stats.Level.Popup.Experience").Format(experience))];

        xpText.lifeTime = 100;
        xpText.velocity.Y = -4;

        // Give runtime points, show level up popup, and play level up sound
        if (priorLevel == Level) return;
        Points += PlayConfiguration.Instance.Level.Points * (Level - priorLevel);

        SoundEngine.PlaySound(new SoundStyle($"{Mod.Name}/Assets/Sounds/LevelUp"));
        var levelText = Main.combatText[CombatText.NewText(Player.getRect(), Color.Red,
            Mod.GetLocalization("Stats.Level.Popup.LevelUp").Value, true)];

        levelText.lifeTime = 180;
        levelText.velocity.Y = -6;
    }

    public override void OnEnterWorld()
    {
        if (PlayConfiguration.Instance.CommandsEnabled) return;
        ModContent.GetInstance<StatSystem>().ValidateStats(Player);
    }

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        if (mediumCoreDeath) return [];
        return
        [
            new Item(ModContent.ItemType<Respec>()),
            new Item(ModContent.ItemType<LesserScalingPotion>(), 5)
        ];
    }

    public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod,
        bool mediumCoreDeath)
    {
        if (mediumCoreDeath || !PlayConfiguration.Instance.RandomStartingWeapon) return;

        var rand = new Random(DateTime.Now.Millisecond);

        var startingWeapon = rand.Next(9) switch
        {
            0 => new Item(ItemID.WoodenBoomerang),
            1 => new Item(ItemID.CopperBow),
            2 => new Item(ItemID.WandofSparking),
            3 => new Item(ItemID.BabyBirdStaff),
            4 => new Item(ItemID.Spear),
            5 => new Item(ItemID.WoodYoyo),
            6 => new Item(ItemID.Blowpipe),
            7 => new Item(ItemID.Shuriken, 200 + rand.Next(101)),
            _ => new Item(ItemID.CopperBroadsword)
        };

        itemsByMod["Terraria"][0] = startingWeapon;

        itemsByMod[Mod.Name].Add(startingWeapon.type switch
        {
            ItemID.CopperBow => new Item(rand.Next(3) switch
            {
                0 => ItemID.BoneArrow,
                1 => ItemID.FlamingArrow,
                _ => ItemID.WoodenArrow
            }, 100 + rand.Next(101)),
            ItemID.WandofSparking => new Item(ItemID.ManaCrystal),
            ItemID.Blowpipe => new Item(ItemID.Seed, 100 + rand.Next(101)),
            _ => new Item(ItemID.SilverCoin, 50)
        });
    }

    public override void OnRespawn()
    {
        var lossPercentage = PlayConfiguration.Instance.LossPercentage;

        if (lossPercentage == 0f) return;

        Experience = Math.Max(LevelToExperience(Level),
            (long)(Experience - (Experience - LevelToExperience(Level)) * lossPercentage));
    }

    public override void PostUpdateMiscEffects()
    {
        Player.statLifeMax2 += Life;
        Player.statManaMax2 += Mana;
    }

    // Just to get XP for fishing
    public override void ModifyCaughtFish(Item fish)
    {
        var experience = (int)(PlayConfiguration.Instance.ExperienceScale.Fishing * (fish.value / 1000));

        if (experience == 0) return;

        GainExperience(experience);
    }

    public override void Initialize() => Experience = 0;

    public override void LoadData(TagCompound tag) => Experience = tag.GetLong("Experience");

    public override void SaveData(TagCompound tag) => tag["Experience"] = Experience;

    public override void CopyClientState(ModPlayer targetCopy) => ((LevelPlayer)targetCopy).Level = Level;

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        if (!newPlayer) return;

        var packet = new StatPacket
        {
            Id = "level",
            Value = Level
        };

        packet.Send();
    }

    public override void SendClientChanges(ModPlayer clientCopy)
    {
        if (((LevelPlayer)clientCopy).Level == Level) return;

        SyncPlayer(0, 0, true);
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (Main.netMode == NetmodeID.Server) return;
        if (!ModContent.GetInstance<KeybindSystem>().ToggleStatUI.JustPressed) return;
        ModContent.GetInstance<StatUISystem>().Toggle();
    }
}