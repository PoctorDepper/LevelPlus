using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LevelPlus.Systems;

[Autoload(Side = ModSide.Client)]
public class KeybindSystem : ModSystem
{
    public ModKeybind ToggleStatUI { get; private set; }
    public ModKeybind SpendMultFive { get; private set; }
    public ModKeybind SpendMultTen { get; private set; }
    public ModKeybind SpendMultTwenty { get; private set; }

    public override void Load()
    {
        ToggleStatUI = KeybindLoader.RegisterKeybind(Mod, Language.GetTextValue("ToggleStatUI"), Keys.P);
        SpendMultFive = KeybindLoader.RegisterKeybind(Mod, Language.GetTextValue("SpendMultFive"), Keys.LeftShift);
        SpendMultTen = KeybindLoader.RegisterKeybind(Mod, Language.GetTextValue("SpendMultTen"), Keys.LeftControl);
        SpendMultTwenty = KeybindLoader.RegisterKeybind(Mod, Language.GetTextValue("SpendMultTwenty"), Keys.LeftAlt);
    }

    public override void Unload()
    {
        ToggleStatUI = null;
        SpendMultFive = null;
        SpendMultTen = null;
        SpendMultTwenty = null;
    }
}