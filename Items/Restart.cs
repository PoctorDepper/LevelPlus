using LevelPlus.Players;
using LevelPlus.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PlayConfiguration = LevelPlus.Configs.PlayConfiguration;

namespace LevelPlus.Items;

public class Restart : ModItem
{
    public override string Texture => $"{Mod.Name}/Assets/Textures/Items/Restart";

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 15;
        Item.useTime = 20;
        Item.UseSound = SoundID.Item4;
        Item.maxStack = 1;
        Item.consumable = true;
        Item.value = Item.sellPrice(silver: 20);
        Item.rare = ItemRarityID.Green;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<Essence>()
            .AddTile(TileID.WorkBenches)
            .DisableDecraft()
            .Register();
    }

    public override bool? UseItem(Player player)
    {
        ModContent.GetInstance<StatSystem>().GetStats(player.whoAmI).ForEach(s => s.Value = 0);
        
        var levelPlayer = player.GetModPlayer<LevelPlayer>();

        levelPlayer.Experience = 0;
        levelPlayer.Points = PlayConfiguration.Instance.StartingPoints;
        
        return true;
    }
}