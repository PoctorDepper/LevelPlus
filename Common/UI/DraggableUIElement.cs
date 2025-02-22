using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace LevelPlus.Common.UI;

public class DraggableUIElement : UIElement
{
    private Vector2 offset;
    protected bool Dragging { get; private set; }

    private void Move(Vector2 position)
    {
        // Update the location mouse relative
        Left.Set(position.X - offset.X, 0f);
        Top.Set(position.Y - offset.Y, 0f);

        // Grab the parent bounds and ensure that we are inside them
        var parentSpace = Parent.GetDimensions().ToRectangle();
        if (GetDimensions().ToRectangle().Intersects(parentSpace)) return;

        Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
        Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
    }

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);

        // if (evt.Target != this) return;
        if (!IsMouseHovering) return;
        
        offset = evt.MousePosition - new Vector2(Left.Pixels, Top.Pixels);
        Dragging = true;
    }

    public override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);

        // if (evt.Target != this) return;
        if (!IsMouseHovering) return;
        
        Dragging = false;

        Move(evt.MousePosition);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsMouseHovering) Main.LocalPlayer.mouseInterface = true;

        if (!Dragging) return;

        Move(Main.MouseScreen);
    }
}