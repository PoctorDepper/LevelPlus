using LevelPlus.Configs;
using Terraria.Localization;

namespace LevelPlus.Players;

public class EnduranceStat : Stat
{
    public override LocalizedText Description => base.Description.WithFormatArgs(Life(), Defense(), LifeRegen());

    public override LocalizedText SpendTooltip =>
        base.SpendTooltip.WithFormatArgs(Life(true), Defense(true), LifeRegen(true));

    public override string Id => "Endurance";

    private int Life(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Endurance.Life;
    }

    private int Defense(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Endurance.Defense;
    }
    
    private int LifeRegen(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) / PlayConfiguration.Instance.Endurance.LifeRegenCost * 2;
    }

    public override void PostUpdateMiscEffects()
    {
        Player.statLifeMax2 += LifeRegen();
        Player.statDefense += Defense();
    }

    public override void UpdateLifeRegen()
    {
        Player.lifeRegen += LifeRegen();
    }
}