using CustomWands.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CustomWands.Content.SpellComponents
{
    class HeavyShot : ModifierComponent
    {
        public new const int ComponentID = 101; //component ID is used for saving/loading and every type of component requires a unique ID

        public override void SetDefaults()
        {
            item.mana = 8;
            ExtraPercentageDamage = 0.5f;
            ExtraSpeedPercentage = -2f;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases damage but greatly reduces projectile speed");
        }

        public override void DoAI(CustomProjectile CurrentProjectile)
        {
            CurrentProjectile.projectile.velocity = CurrentProjectile.projectile.velocity * 0.99f;
            Color color = Color.Red;
            Vector2 direction = new Vector2((float)Math.Cos(CurrentProjectile.projectile.rotation), (float)Math.Sin(CurrentProjectile.projectile.rotation));
            Vector2 center = CurrentProjectile.projectile.Center;
            float angle = CurrentProjectile.projectile.rotation;
            float speed = (float)Main.rand.NextDouble() * 2.6f + 1f;
            Vector2 velocity = speed * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            int dust = Dust.NewDust(center, 0, 0, 267, velocity.X, velocity.Y, 0, color, 1.2f);
            Main.dust[dust].noGravity = true;

        }
    }
}
