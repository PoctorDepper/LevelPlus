using System;
using LevelPlus.Network;
using LevelPlus.Systems;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LevelPlus.Players;

// A class to make developing stats faster
public abstract class Stat : ModPlayer
{
    // The value of, or amount of points invested in, the stat.
    public int Value { get; set; }

    // The value that would be while spending
    public int ProjectedValue
    {
        get
        {
            var keybind = ModContent.GetInstance<KeybindSystem>();
            return Value + Math.Min(Player.GetModPlayer<LevelPlayer>().Points,
                (keybind.SpendMultFive.Current ? 5 : 1) *
                (keybind.SpendMultTen.Current ? 10 : 1) *
                (keybind.SpendMultTwenty.Current ? 20 : 1)
            );
        }
    }

    // The LocalizedText for the name of the Stat
    public virtual LocalizedText Name => Mod.GetLocalization("Stats." + Id + ".DisplayName", () => Id);

    // The LocalizedText for the description. Should be pre-formatted with args.
    public virtual LocalizedText Description => Mod.GetLocalization("Stats." + Id + ".Tooltip", () => Id + " Tooltip");

    // The LocalizedText for the description for next point(s) spent. Should be pre-formatted with args.
    public virtual LocalizedText SpendTooltip => Description;

    // The path of the icon to be used in the UI.
    public virtual string IconPath => "Assets/Textures/UI/Icons/" + Id;

    // The color to modify the UI element by.
    public virtual Color Color => Color.White;

    // The access key for stat, usually the name.
    public abstract string Id { get; }

    public override void Initialize() => Value = 0;

    public override void LoadData(TagCompound tag) => Value = tag.GetInt(Id);

    public override void SaveData(TagCompound tag) => tag[Id] = Value;

    public override void CopyClientState(ModPlayer targetCopy) => ((Stat)targetCopy).Value = Value;

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        if (!newPlayer) return;

        var packet = new StatPacket
        {
            Id = Id,
            Value = Value,
        };

        packet.Send(toWho, fromWho);
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        if (((Stat)clientPlayer).Value == Value) return;
        SyncPlayer(0, 0, true);
    }
}