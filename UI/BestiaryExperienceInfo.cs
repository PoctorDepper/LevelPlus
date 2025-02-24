using LevelPlus.GlobalNPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace LevelPlus.UI;

public class BestiaryExperienceElement(NPC instance)
    : IBestiaryInfoElement, ICategorizedBestiaryInfoElement, IBestiaryPrioritizedElement
{
    public float OrderPriority => 1f;

    public UIBestiaryEntryInfoPage.BestiaryInfoCategory ElementCategory =>
        UIBestiaryEntryInfoPage.BestiaryInfoCategory.Stats;

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
    {
        if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
        {
            return null;
        }

        UIPanel panel = new(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel"),
            null, customBarSize: 7)
        {
            Width = StyleDimension.FromPixelsAndPercent(-11f, 1f),
            Height = StyleDimension.FromPixels(30f),
            BackgroundColor = new Color(43, 56, 101),
            BorderColor = Color.Transparent,
            Left = StyleDimension.FromPixels(-8f),
            HAlign = 1f,
        };
        panel.SetPadding(0f);
        panel.PaddingRight = 5f;
        panel.OnUpdate += ShowExperienceTooltip;

        UIImage icon = new(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Knockback"))
        {
            IgnoresMouseInteraction = true,
            ScaleToFit = true,
            HAlign = 0,
            VAlign = 0.5f,
        };
        panel.Append(icon);

        UIText experienceText = new(
            info.UnlockState >= BestiaryEntryUnlockState.CanShowStats_2
                ? ScalingNPC.CalculateExperience(instance).ToString()
                : "???",
            0.85f)
        {
            IgnoresMouseInteraction = true,
            HAlign = 1f,
            VAlign = 0.5f,
            Left = StyleDimension.FromPixels(-3),
            TextColor = Color.White,
        };
        panel.Append(experienceText);

        return panel;
    }

    private void ShowExperienceTooltip(UIElement element)
    {
        if (!element.IsMouseHovering) return;
        Main.instance.MouseText(ModContent.GetInstance<LevelPlus>()
            .GetLocalization("Bestiary.Tooltip", () => "Experience").Value);
    }
}