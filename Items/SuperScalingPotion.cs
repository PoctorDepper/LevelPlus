using Terraria;
using Terraria.ID;

namespace LevelPlus.Items;

public class SuperScalingPotion : ScalingPotion
{
    protected override float HealLifePercent => 0.4f;
    protected override float HealManaPercent => 0.75f;
    
    public override string Texture => $"{Mod.Name}/Assets/Textures/Items/SuperScalingPotion";

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.height = 30;
        Item.value = Item.buyPrice(gold: 20);
        Item.rare = new Item(ItemID.SuperHealingPotion).rare + 1;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<Essence>(2)
            .AddIngredient(ItemID.SuperHealingPotion)
            .AddIngredient(ItemID.SuperManaPotion)
            .AddTile(TileID.Bottles)
            .DisableDecraft()
            .Register();
        
        CreateRecipe()
            .AddIngredient<Essence>(100)
            .AddIngredient<GreaterScalingPotion>()
            .AddTile(TileID.Bottles)
            .DisableDecraft()
            .Register();
    }
}