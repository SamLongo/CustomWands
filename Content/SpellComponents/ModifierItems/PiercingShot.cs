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
    class PiercingShot : ModifierComponent
    {
        public new const int ComponentID = 103; //component ID is used for saving/loading and every type of component requires a unique ID

        public override void SetDefaults()
        {
            item.mana = 4;
            penetrate = 2;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Projectile can pierce enemies 2 additional times");
        }

        public override void DoOnHitNPC(CustomProjectile CurrentProjectile, NPC target, int damage, float knockback, bool crit)
        {
            CurrentProjectile.CantHitNPCS.Add(target);
        }

    }
}

