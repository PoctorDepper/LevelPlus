using LevelPlus.Configs;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LevelPlus.Players;

public class IntellectStat : Stat
{
    public override LocalizedText Description => base.Description.WithFormatArgs(Damage(), Mana(), ManaRegen(), BlockRange());

    public override LocalizedText SpendTooltip =>
        base.SpendTooltip.WithFormatArgs(Damage(true), Mana(true), ManaRegen(true), BlockRange(true));

    public override string Id => "Intellect";

    private float Damage(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Intellect.Damage;
    }

    private int Mana(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Intellect.Mana;
    }

    private int ManaRegen(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) / PlayConfiguration.Instance.Intellect.ManaRegenCost;
    }
    
    private int BlockRange(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) / PlayConfiguration.Instance.Intellect.BlockRangeCost;
    }

    public override void PostUpdateMiscEffects()
    {
        Player.GetDamage(DamageClass.Magic) += Damage();
        Player.statManaMax2 += Mana();
        Player.manaRegen += ManaRegen();
        Player.blockRange += BlockRange();
    }
}