using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace LevelPlus.Configs;

[BackgroundColor(55, 15, 85, 190)]
public partial class PlayConfiguration : ModConfig
{
    public static PlayConfiguration Instance { get; private set; }

    public override ConfigScope Mode => ConfigScope.ServerSide;
    public override void OnLoaded() => Instance = this;

    [Header("Play")]
    [BackgroundColor(0, 0, 0)]
    [Range(0f, 0.10f)]
    [DefaultValue(0.02f)]
    public float ScalingPercentage { get; set; }

    [BackgroundColor(0, 0, 0)]
    [Increment(0.1f)]
    [Range(0f, 1f)]
    [DefaultValue(0.1f)]
    public float LossPercentage { get; set; }
    
    [BackgroundColor(0, 0, 0)]
    [Increment(0.1f)]
    [Range(0f, 1f)]
    [DefaultValue(0.2f)]
    public float TeamSharePercentage { get; set; }

    [BackgroundColor(0, 0, 0)]
    [DefaultValue(true)]
    public bool RandomStartingWeapon { get; set; }

    [BackgroundColor(0, 0, 0)]
    [Range(0, 10)]
    [DefaultValue(3)]
    public int StartingPoints { get; set; }
    
    [Expand(false)]
    [BackgroundColor(0, 0, 0)]
    [LabelKey("$Mods.LevelPlus.Configs.ExperienceScaleConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.ExperienceScaleConfig.Tooltip")]
    public ExperienceScaleConfig ExperienceScale { get; set; } = new();

    public class ExperienceScaleConfig
    {
        [BackgroundColor(0, 0, 0)]
        [Range(0f, 3f)]
        [Increment(0.25f)]
        [Slider]
        [DrawTicks]
        public float Combat { get; set; } = 1.0f;

        [BackgroundColor(0, 0, 0)]
        [Range(0f, 3f)]
        [Increment(0.25f)]
        [Slider]
        [DrawTicks]
        public float Mining { get; set; } = 1.0f;

        [BackgroundColor(0, 0, 0)]
        [Range(0f, 3f)]
        [Increment(0.25f)]
        [Slider]
        [DrawTicks]
        public float Fishing { get; set; } = 1.0f;

        public override int GetHashCode() => HashCode.Combine(Combat, Mining, Fishing);
    }
}