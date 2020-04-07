using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CustomWands.Content.Projectiles;
using CustomWands.Content.SpellComponents;
using Microsoft.Xna.Framework.Graphics;
using CustomWands.Content.GFX;

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
			width = 16;
			height = 16;
			penetrate = 0;
			projspeed = 28f;
			bounces = 3;
		}
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A weak bouncing blast with high speed");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		
		public override void DoAI(CustomProjectile CurrentProjectile)
		{
			if (CurrentProjectile.hasgravity)
			{
				ApplyGravity(CurrentProjectile);
			}
			
		}

		public override bool DoOnTileCollide(CustomProjectile CurrentProjectile, Vector2 oldVelocity)
		{
			CurrentProjectile.bounces--;
			if (CurrentProjectile.bounces <= 0)
			{
				return true;
			}
			else
			{
				if (CurrentProjectile.projectile.velocity.X != oldVelocity.X)
				{
					CurrentProjectile.projectile.velocity.X = -oldVelocity.X;
				}
				if (CurrentProjectile.projectile.velocity.Y != oldVelocity.Y)
				{
					CurrentProjectile.projectile.velocity.Y = -oldVelocity.Y;
				}
				CurrentProjectile.projectile.velocity *= 0.75f;
				Main.PlaySound(SoundID.Item10, CurrentProjectile.projectile.position);
				return false;
			}
		}

		public override void DoOnHitNPC(CustomProjectile CurrentProjectile, NPC target, int damage, float knockback, bool crit)
		{
			Main.PlaySound(SoundID.Item25, CurrentProjectile.projectile.position);
		}

		public override void DoKill(CustomProjectile CurrentProjectile, int timeLeft)
		{
		}

		public override void DoDraw(CustomProjectile CurrentProjectile, SpriteBatch spriteBatch)
		{
		//	spriteBatch.Draw(GFX.GFX.BOUNCINGBOLT, CurrentProjectile.projectile.position, CurrentProjectile.projectile.getRect(), Color.White, CurrentProjectile.angle, Vector2.Zero, CurrentProjectile.projectile.scale, SpriteEffects.None, 0f);

			spriteBatch.Draw(GFX.GFX.BOUNCINGBOLT, CurrentProjectile.projectile.position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, CurrentProjectile.projectile.scale, SpriteEffects.None, 0f);
		}
	}
}
