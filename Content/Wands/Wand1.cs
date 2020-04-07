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
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item11;
			item.shoot = 10;
			item.shootSpeed = 16f;

			wandsize = 3;
			base.SetDefaults();
		}

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weak wand");
            Tooltip.SetDefault("three slot wand");
		}

	}
}
