using Terraria;
using Terraria.ID;

namespace LevelPlus.Items;

public class LesserScalingPotion : ScalingPotion
{
    protected override float HealLifePercent => 0.1f;
    protected override float HealManaPercent => 0.125f;
    
    public override string Texture => $"{Mod.Name}/Assets/Textures/Items/LesserScalingPotion";

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.height = 26;
        Item.value = Item.buyPrice(silver: 50);
        Item.rare = new Item(ItemID.LesserHealingPotion).rare + 1;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<Essence>(2)
            .AddIngredient(ItemID.LesserHealingPotion)
            .AddIngredient(ItemID.LesserManaPotion)
            .AddTile(TileID.Bottles)
            .DisableDecraft()
            .Register();
    }
}