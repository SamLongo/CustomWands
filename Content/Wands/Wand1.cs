using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using CustomWands.Content.SpellComponents;

namespace CustomWands.Content.Wands
{
    public class Wand1 : WandAbstract
    {

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 20;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = 10; 
			item.shootSpeed = 16f;
			item.magic = true;
			wandsize = 3;
			ComponentList = new List<SpellComponent>(wandsize);
			for (int i = 0; i < wandsize; i++)
			{
				ComponentList.Add(null);
			}
		}

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weak wand");
            Tooltip.SetDefault("three slot wand");
		}

	}
}
