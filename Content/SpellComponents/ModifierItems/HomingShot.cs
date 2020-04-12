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
    class HomingShot : ModifierComponent
    {
        public new const int ComponentID = 104; //component ID is used for saving/loading and every type of component requires a unique ID

        public override void SetDefaults()
        {
            item.mana = 8;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Seeks out the nearest enemy");
        }

        public override void DoAI(CustomProjectile CurrentProjectile)
        {
            bool target = false;
            Vector2 move = Vector2.Zero;
            float distance = 400f; //this defines how far the projectile will track the closest enemy
            double direction = 0;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - CurrentProjectile.projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        direction = Math.Atan2((double)(Main.npc[k].Center.Y - CurrentProjectile.projectile.Center.Y), (double)(Main.npc[k].Center.X - CurrentProjectile.projectile.Center.X));
                        target = true;
                    }
                }
            }

            if (target)
            {
                move.Normalize();
                CurrentProjectile.projectile.velocity = CurrentProjectile.projectile.velocity + move *0.5f;
            }

            
        }
    }
}
