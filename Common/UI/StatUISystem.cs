using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace LevelPlus.Common.UI;

[Autoload(Side = ModSide.Client)]
public class StatUISystem : ModSystem
{
    // TODO Add UI States when they're made
    private ExperienceBar experienceBar;

    private UserInterface currentInterface;

    public void Toggle()
    {
        // TODO Swap the UI states
    }

    public override void Load()
    {
        experienceBar = new ExperienceBar();
        experienceBar.Activate();
        
        currentInterface = new UserInterface();
        currentInterface.SetState(experienceBar);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        currentInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        var resourceBarsIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (resourceBarsIndex == -1) return;
        layers.Insert(resourceBarsIndex, new LegacyGameInterfaceLayer(
            "Level+: Spend Interface",
            delegate
            {
                if (currentInterface?.CurrentState is null) return false;
                
                currentInterface.Draw(Main.spriteBatch, new GameTime());
                
                return true; 
            },
            InterfaceScaleType.UI)
        );
    }
}