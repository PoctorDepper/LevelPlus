using System;
using LevelPlus.Configs;
using Terraria;
using Terraria.Localization;

namespace LevelPlus.Players;

public class LuckStat : Stat
{
    private static Random rng;
    
    public override LocalizedText Description => base.Description.WithFormatArgs(Crit(), Luck(), Ammo());

    public override LocalizedText SpendTooltip =>
        base.SpendTooltip.WithFormatArgs(Crit(true), Luck(true), Ammo(true));

    public override string Id => "Luck";

    private float Crit(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Luck.Crit;
    }

    private float Luck(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Luck.TerrariaLuck;
    }

    private float Ammo(bool projected = false)
    {
        return Math.Min(1, (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Luck.Ammo) * 100;
    }

    public override void Initialize()
    {
        base.Initialize();
        rng = new Random(DateTime.Now.Millisecond);
    }

    public override void ModifyWeaponCrit(Item item, ref float crit)
    {
        crit += Crit();
    }

    public override void ModifyLuck(ref float luck)
    {
        luck += Luck();
    }

    public override bool CanConsumeAmmo(Item weapon, Item ammo)
    {
        return Ammo() < rng.Next(100);
    }
}