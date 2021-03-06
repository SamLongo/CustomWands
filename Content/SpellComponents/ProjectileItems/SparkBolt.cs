﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CustomWands.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CustomWands.Content.SpellComponents
{

    class SparkBolt : ProjectileComponent
    {
        public new const int ComponentID = 1; //component ID is used for saving/loading and every type of component requires a unique ID


        public override void SetDefaults()
        {
            item.damage = 40;
            item.magic = true;
            item.mana = 6;
            item.knockBack = 5;
            item.value = 10000;
            item.rare = 2;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            projspeed = 16f;
            timeLeft = 300;
            width = 8;
            height = 8;


        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The simplest magic bolt");
        }


        public override void DoDraw(CustomProjectile CurrentProjectile, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GFX.GFX.SPARKBOLT, CurrentProjectile.projectile.position - Main.screenPosition - new Vector2((float)(12*Math.Cos(CurrentProjectile.angle)), (float)(12 * Math.Sin(CurrentProjectile.angle))) , null, Color.White, CurrentProjectile.angle, Vector2.Zero, CurrentProjectile.projectile.scale, SpriteEffects.None, 0f);

        }
    }
}
