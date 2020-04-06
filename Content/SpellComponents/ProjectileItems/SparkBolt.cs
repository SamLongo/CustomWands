using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CustomWands.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CustomWands.Content.SpellComponents
{

    class SparkBolt : ProjectileComponent
	{
		public new const int ComponentID = 1; //component ID is used for saving/loading and every type of component requires a unique ID
		public override void SetDefaults()
		{
			item.damage = 40;
			item.magic = true;
			item.mana = 12;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ProjectileType<SparkBoltProjectile>();
			item.shootSpeed = 16f;

			
		}

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The simplest magic bolt");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

	}
}
