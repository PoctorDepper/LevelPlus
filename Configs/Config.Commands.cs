using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace LevelPlus.Configs;

public partial class PlayConfiguration
{
    [Header("Commands")]
    [DefaultValue(false)]
    [BackgroundColor(0, 0, 0)]
    public bool CommandsEnabled { get; set; }

    [Expand(false)]
    [BackgroundColor(0, 0, 0)]
    [LabelKey("$Mods.LevelPlus.Configs.CommandConfig.Label")]
    [TooltipKey("$Mods.LevelPlus.Configs.CommandConfig.Tooltip")]
    public CommandConfig Command { get; set; } = new();

    public class CommandConfig
    {
        [BackgroundColor(0, 0, 0)] public bool Level { get; set; } = true;

        [BackgroundColor(0, 0, 0)] public bool Experience { get; set; } = true;

        [BackgroundColor(0, 0, 0)] public bool Point { get; set; } = true;

        public override int GetHashCode() => HashCode.Combine(Level, Experience, Point);
    }
}