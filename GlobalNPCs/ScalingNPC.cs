using LevelPlus.Configs;
using LevelPlus.Items;
using LevelPlus.Network;
using LevelPlus.Players;
using LevelPlus.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace LevelPlus.GlobalNPCs;

public class ScalingNPC : GlobalNPC
{
    private static float Scalar = LevelPlayer.GetAverageLevel() * PlayConfiguration.Instance.ScalingPercentage;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        // Invalid NPC checks
        return !(
            // I don't think you can kill a target dummy, but better to be safe
            entity.type == NPCID.TargetDummy ||
            // No farming
            entity.SpawnedFromStatue ||
            // No NPC murder either
            entity.friendly ||
            // No critters shall be harmed in the making of this mod
            entity.CountsAsACritter ||
            // Should take care of most projectiles giving xp
            entity.lifeMax < 5 ||
            // Killing segments shouldn't get you more xp
            entity.realLife > -1
        );
    }

    public static int CalculateExperience(NPC npc)
    {
        npc.CloneDefaults(npc.netID);
        return (int) (PlayConfiguration.Instance.ExperienceScale.Combat * (npc.lifeMax / 10 + npc.defense + npc.defDamage / 3));
    }

    public override void ModifyHitPlayer(NPC npc, Terraria.Player target, ref Terraria.Player.HurtModifiers modifiers)
    {
        modifiers.SourceDamage += Scalar;
    }

    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        // Doesn't work on variants for some reason (e.g. Green Slime)
        // May end up moving this to ApplyDifficultyAndPlayerScaling and make scaling an Expert+ feature
        npc.life = npc.lifeMax = (int)(npc.lifeMax * (1 + Scalar));
    }

    public override void OnKill(NPC npc)
    {
        if (!npc.AnyInteractions() || Main.netMode == NetmodeID.MultiplayerClient) return;

        var experience = CalculateExperience(npc);

        if (experience == 0) return;
        
        switch (Main.netMode)
        {
            case NetmodeID.SinglePlayer:
                Main.LocalPlayer.GetModPlayer<LevelPlayer>().GainExperience(experience);
                break;

            case NetmodeID.Server:
                for (var i = 0; i < npc.playerInteraction.Length; i++)
                {
                    if (!npc.playerInteraction[i]) continue;

                    var packet = new GainExperiencePacket
                    {
                        Amount = experience
                    };

                    packet.Send(i);
                }

                break;
        }
    }

    public override void ModifyGlobalLoot(GlobalLoot globalLoot)
    {
        globalLoot.Add(ItemDropRule.Common(ModContent.ItemType<Essence>(), 100, 1, 5));
    }

    public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.Add(new BestiaryExperienceElement(npc));
    }
}