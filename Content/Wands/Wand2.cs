using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using CustomWands.Content.SpellComponents;
using Microsoft.Xna.Framework;

namespace CustomWands.Content.Wands
{

    public class Wand2 : WandAbstract
    {
        public override void SetDefaults()
        {

			item.width = 40;
			item.height = 20;
			item.useTime = 15;
            item.useAnimation = item.useTime;
            item.useStyle = 5;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item11;
			item.shoot = 10;
            item.shootSpeed = 16f;
            Spread = MathHelper.ToRadians(25);
			
			wandsize = 8;
            base.SetDefaults();

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weak wand");
            Tooltip.SetDefault("Use the Wand Editor UI to put components in (Default Hotkey: Y)");


        }

    }
}