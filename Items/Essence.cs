using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LevelPlus.Items;

public class Essence : ModItem
{
    public override string Texture => $"{Mod.Name}/Assets/Textures/Items/Essence";

    public override void SetStaticDefaults()
    {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
        
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        ItemID.Sets.ItemIconPulse[Item.type] = true;
        ItemID.Sets.ItemNoGravity[Item.type] = true;

        Item.ResearchUnlockCount = 100;
    }

    public override void SetDefaults()
    {
        // Item.width = 10;
        // Item.height = 10;
        Item.maxStack = Terraria.Item.CommonMaxStack;
        Item.value = Terraria.Item.sellPrice(silver: 35);
        Item.rare = ItemRarityID.Blue;
    }

    public override Color? GetAlpha(Color lightColor) => new Color(1f, 1f, 1f, .75f);

    public override void PostUpdate()
    {
        
        Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
    }
}