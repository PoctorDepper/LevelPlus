using System;
using Terraria.ModLoader.Config;

namespace LevelPlus.Configs;

public partial class PlayConfiguration
{
    [Header("Stats")]
    [Expand(false)]
    [BackgroundColor(255, 255, 255)]
    [LabelKey("$Mods.LevelPlus.Configs.LevelConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.LevelConfig.Tooltip")]
    public LevelConfig Level { get; set; } = new();

    public class LevelConfig
    {
        [Slider]
        [Increment(10)]
        [Range(10, 500)]
        [BackgroundColor(255, 255, 255)]
        public int Max { get; set; } = 100;

        [Slider]
        [Range(0, 10)]
        [BackgroundColor(255, 255, 255)]
        public int Points { get; set; } = 2;

        [Slider]
        [Range(0, 10)]
        [BackgroundColor(255, 255, 255)]
        public int Life { get; set; } = 1;

        [Slider]
        [Range(0, 10)]
        [BackgroundColor(255, 255, 255)]
        public int Mana { get; set; } = 0;

        public override int GetHashCode() => HashCode.Combine(Max, Points, Life, Mana);
    }


    [Expand(false)]
    [BackgroundColor(255, 255, 0)]
    [LabelKey("$Mods.LevelPlus.Configs.EnduranceConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.EnduranceConfig.Tooltip")]
    public EnduranceConfig Endurance { get; set; } = new();

    public class EnduranceConfig
    {
        [Slider]
        [Range(0, 10)]
        [BackgroundColor(255, 255, 0)]
        public int Life { get; set; } = 5;

        [Slider]
        [Range(0, 10)]
        [BackgroundColor(255, 255, 0)]
        public int Defense { get; set; } = 2;

        [Slider]
        [Range(10, 60)]
        [BackgroundColor(255, 255, 0)]
        public int LifeRegenCost { get; set; } = 20;

        public override int GetHashCode() => HashCode.Combine(Life, Defense, LifeRegenCost);
    }


    [Expand(false)]
    [BackgroundColor(255, 0, 0)]
    [LabelKey("$Mods.LevelPlus.Configs.BrawnConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.BrawnConfig.Tooltip")]
    public BrawnConfig Brawn { get; set; } = new();

    public class BrawnConfig
    {
        [Range(0, 0.10f)]
        [BackgroundColor(255, 0, 0)]
        public float Damage { get; set; } = 0.01f;

        [Range(0, 0.10f)]
        [BackgroundColor(255, 0, 0)]
        public float MaxWingTime { get; set; } = 0.02f;

        [Range(0, 0.10f)]
        [BackgroundColor(255, 0, 0)]
        public float PickSpeed { get; set; } = 0.01f;

        public override int GetHashCode() => HashCode.Combine(Damage, MaxWingTime, PickSpeed);
    }


    [Expand(false)]
    [BackgroundColor(0, 255, 255)]
    [LabelKey("$Mods.LevelPlus.Configs.DeftConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.DeftConfig.Tooltip")]
    public DeftConfig Deft { get; set; } = new();

    public class DeftConfig
    {
        [Range(0, 0.10f)]
        [BackgroundColor(0, 255, 255)]
        public float Damage { get; set; } = 0.01f;

        [Range(0, 0.10f)]
        [BackgroundColor(0, 255, 255)]
        public float MoveSpeed { get; set; } = 0.01f;

        [Range(0, 0.10f)]
        [BackgroundColor(0, 255, 255)]
        public float PlacementSpeed { get; set; } = 0.02f;
        
        public override int GetHashCode() => HashCode.Combine(Damage, MoveSpeed, PlacementSpeed);
    }


    [Expand(false)]
    [BackgroundColor(0, 0, 255)]
    [LabelKey("$Mods.LevelPlus.Configs.IntellectConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.IntellectConfig.Tooltip")]
    public IntellectConfig Intellect { get; set; } = new();

    public class IntellectConfig
    {
        [Range(0, 0.10f)]
        [BackgroundColor(0, 0, 255)]
        public float Damage { get; set; } = 0.01f;

        [Slider]
        [Range(0, 10)]
        [BackgroundColor(0, 0, 255)]
        public int Mana { get; set; } = 2;

        [Slider]
        [Range(10, 60)]
        [BackgroundColor(0, 0, 255)]
        public int ManaRegenCost { get; set; } = 30;

        [Slider]
        [Range(10, 60)]
        [BackgroundColor(0, 0, 255)]
        public int BlockRangeCost { get; set; } = 25;

        public override int GetHashCode() => HashCode.Combine(Damage, Mana, ManaRegenCost, BlockRangeCost);
    }


    [Expand(false)]
    [BackgroundColor(255, 0, 255)]
    [LabelKey("$Mods.LevelPlus.Configs.CharmConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.CharmConfig.Tooltip")]
    public CharmConfig Charm { get; set; } = new();

    public class CharmConfig
    {
        [Range(0, 0.10f)]
        [BackgroundColor(255, 0, 255)]
        public float Damage { get; set; } = 0.01f;

        [Slider]
        [Range(10, 60)]
        [BackgroundColor(255, 0, 255)]
        public int MinionCost { get; set; } = 20;

        [Slider]
        [Range(10, 60)]
        [BackgroundColor(255, 0, 255)]
        public int SentryCost { get; set; } = 30;

        [Range(0, 0.10f)]
        [BackgroundColor(255, 0, 255)]
        public float Fishing { get; set; } = 0.01f;

        public override int GetHashCode() => HashCode.Combine(Damage, MinionCost, SentryCost, Fishing);
    }


    [Expand(false)]
    [BackgroundColor(0, 255, 0)]
    [LabelKey("$Mods.LevelPlus.Configs.LuckConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.LuckConfig.Tooltip")]
    public LuckConfig Luck { get; set; } = new();

    public class LuckConfig
    {
        [Range(0, 0.10f)]
        [BackgroundColor(0, 255, 0)]
        public float Crit { get; set; } = 0.05f;

        [Range(0, 0.15f)]
        [BackgroundColor(0, 255, 0)]
        public float TerrariaLuck { get; set; } = 0.05f;

        [Range(0, 0.10f)]
        [BackgroundColor(0, 255, 0)]
        public float Ammo { get; set; } = 0.03f;

        public override int GetHashCode() => HashCode.Combine(Crit, TerrariaLuck, Ammo);
    }
}