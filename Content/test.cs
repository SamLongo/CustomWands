using Terraria.ID;
using Terraria.ModLoader;

namespace CustomWands.Content
{
    public class Test : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Awesome sword");
            Tooltip.SetDefault("This is my super duper awesome sword.");

        }

        public override void SetDefaults()
        {
            
            item.damage = 5000;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = 10; // Value is in copper
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;

            
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}