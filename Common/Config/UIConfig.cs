using System.ComponentModel;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Config;

namespace LevelPlus.Common.Config;

public class UIConfig : ModConfig
{
    public static UIConfig Instance { get; private set; }
    
    public override ConfigScope Mode => ConfigScope.ClientSide;
    
    public override void OnLoaded() => Instance = this;

    [BackgroundColor(0, 0, 0)]
    [SliderColor(0, 0, 0)]
    [Range(0, 1920f)]
    [DefaultValue(typeof(Vector2), "480, 35")]
    public Vector2 ExperienceBar { get; set; }
    
    [BackgroundColor(0, 0, 0)]
    [SliderColor(0, 0, 0)]
    [Range(0, 1920f)]
    [DefaultValue(typeof(Vector2), "35, 200")]
    public Vector2 SpendPanel { get; set; }

    [DefaultValue(true)]
    public bool PointNotifier { get; set; }
}