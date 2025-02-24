using System.Reflection;
using LevelPlus.Configs;
using LevelPlus.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace LevelPlus.UI;

public class ExperienceBar : UIState
{
    // 100% - left square % - right endcap %
    private const float quotientScalar = 1f - 13f / 60f - 3f / 60f;
    
    private BarBackground background;
    private UIImage bar;
    private UIText level;

    private LevelPlayer LevelPlayer => Main.LocalPlayer.GetModPlayer<LevelPlayer>();

    public override void OnInitialize()
    {
        base.OnInitialize();

        background = new BarBackground
        {
            Width = StyleDimension.FromPixels(120f),
            Height = StyleDimension.FromPixels(26f)
        };

        background.OnLeftDoubleClick += delegate { ModContent.GetInstance<StatUISystem>().Toggle(); };

        level = new UIText("0")
        {
            Width = StyleDimension.FromPercent(13f / 60f),
            Height = StyleDimension.FromPercent(1f),
            TextOriginX = 0.5f,
            TextOriginY = 0.5f
        };
        background.Append(level);

        bar = new UIImage(ModContent.GetInstance<LevelPlus>().Assets.Request<Texture2D>("Assets/Textures/UI/Bar"))
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f),
            Left = StyleDimension.FromPixels(background.Height.Pixels),
            ScaleToFit = true,
            Color = new Color(50, 205, 30)
        };
        background.Append(bar);

        Append(background);

        
    }

    public override void OnActivate()
    {
        base.OnActivate();

        var placement = UIConfig.Instance.ExperienceBar;

        background.Left.Set(placement.X, 0);
        background.Top.Set(placement.Y, 0);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        level.SetText(LevelPlayer.Level.ToString());

        if (background.IsMouseHovering) Main.instance.MouseText(LevelPlayer.Description.Value);
        if (bar.IsMouseHovering) Main.instance.MouseText(LevelPlayer.ExperienceTooltip.Value);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        // Current level progress experience / Experience needed to get to from current level to next level
        var quotient = (float) (LevelPlayer.Experience - LevelPlayer.LevelToExperience(LevelPlayer.Level)) /
                       (LevelPlayer.LevelToExperience(LevelPlayer.Level + 1) -
                        LevelPlayer.LevelToExperience(LevelPlayer.Level));
        
        
        
        bar.Width.Percent = quotient * quotientScalar;
        
        Recalculate();
    }
}

internal class BarBackground : DraggableUIElement
{
    private Asset<Texture2D> outline;
    private Asset<Texture2D> texture;

    public BarBackground()
    {
        var mod = ModContent.GetInstance<LevelPlus>();
        texture = mod.Assets.Request<Texture2D>("Assets/Textures/UI/Bar_Background");
        outline = mod.Assets.Request<Texture2D>("Assets/Textures/UI/Bar_Outline");

        OnLeftMouseUp += UpdateConfig;
    }

    private void UpdateConfig(UIMouseEvent evt, UIElement element)
    {
        UIConfig.Instance.ExperienceBar = new Vector2(Left.Pixels, Top.Pixels);

        // Thank you SheepishShepherd, I did not know the configs didn't save in-code modification
        var saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
        saveMethodInfo?.Invoke(null, [UIConfig.Instance]);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var inner = GetInnerDimensions().ToRectangle();

        spriteBatch.Draw(texture.Value, inner, Color.White);

        if (!UIConfig.Instance.PointNotifier ||
            Main.LocalPlayer.GetModPlayer<LevelPlayer>().Points <= 0 ||
            IsMouseHovering ||
            Dragging) return;

        spriteBatch.Draw(outline.Value, inner, Main.DiscoColor);
    }
}