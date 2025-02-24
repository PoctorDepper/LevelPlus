using LevelPlus.Configs;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LevelPlus.Players;

public class DeftStat : Stat
{
    public override LocalizedText Description => base.Description.WithFormatArgs(Damage(), MoveSpeed(), PlacementSpeed());

    public override LocalizedText SpendTooltip =>
        base.SpendTooltip.WithFormatArgs(Damage(true), MoveSpeed(true), PlacementSpeed(true));

    public override string Id => "Deft";

    private float Damage(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Deft.Damage;
    }

    private float MoveSpeed(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Deft.MoveSpeed;
    }

    private float PlacementSpeed(bool projected = false)
    {
        return (projected ? ProjectedValue : Value) * PlayConfiguration.Instance.Deft.PlacementSpeed;
    }


    public override void PostUpdateMiscEffects()
    {
        Player.GetDamage(DamageClass.Ranged) += Damage();
        Player.tileSpeed *= 1 + PlacementSpeed();
        Player.wallSpeed *= 1 + PlacementSpeed();
    }

    public override void PostUpdateRunSpeeds()
    {
        Player.moveSpeed *= 1 + MoveSpeed();
    }
}