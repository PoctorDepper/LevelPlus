using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LevelPlus.Systems;

[Autoload(Side = ModSide.Client)]
public class PopupSystem : ModSystem
{
    private static string LevelUp;
    
    public override void Load()
    {
        LevelUp = Mod.GetLocalization("Stats.Level.Popup.LevelUp").Value;
    }

    public override void Unload()
    {
        LevelUp = null;
    }

    // Not sure I should use this method, but it will only run on client so ¯\_(ツ)_/¯
    public override void UpdateUI(GameTime gameTime)
    {
        Main.combatText
            .Where(t => t.active &&
                        t.text == LevelUp)
            .ToList()
            .ForEach(t => t.color = Main.DiscoColor);
    }
}