using CustomWands.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CustomWands.Content.SpellComponents
{
    public abstract class ProjectileComponent : SpellComponent
    {

        public int width = 16;
        public int height = 16;
        public bool friendly = true;
        public int penetrate = 0;
        public int timeLeft = 600;
        public float projspeed = 0;
        public int bounces = 0;


        public override void DoInitValues(CustomProjectile CurrentProjectile)
        {
            CurrentProjectile.projectile.width = width;
            CurrentProjectile.projectile.height = height;
            CurrentProjectile.projectile.friendly = friendly;
            CurrentProjectile.projectile.penetrate += penetrate;
            CurrentProjectile.projectile.timeLeft += timeLeft;
            CurrentProjectile.projectile.damage += item.damage;
            CurrentProjectile.bounces += bounces;
            CurrentProjectile.projectile.magic = item.magic;

        }

        public override void DoPreinitValues(CustomProjectile CurrentProjectile)
        {
            //stuff that needs to be added to the projectile prior to it existing in space
            //only parameters of CustomProjectile no projectile stuff
            CurrentProjectile.speed = projspeed;
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            TooltipLine line = new TooltipLine(mod, "IsProjectile", "Projectile");
            line.overrideColor = Color.Red;
            tooltips.Add(line);
        }


    }
}
