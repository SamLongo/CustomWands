using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CustomWands.Content.Projectiles;
using CustomWands.Content.SpellComponents;

namespace CustomWands.Content.SpellComponents
{
	class BouncingBolt : ProjectileComponent
	{

		public new const int ComponentID = 2; //component ID is used for saving/loading and every type of component requires a unique ID
		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
			item.mana = 12;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ProjectileType<BouncingBoltProjectile>();
			item.shootSpeed = 60f;
		}

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A weak bouncing blast with high speed");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

	}
}
