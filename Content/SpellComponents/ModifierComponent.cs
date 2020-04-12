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
    public abstract class ModifierComponent : SpellComponent
    {

        // stores variables that every modifier will have
        //no modifier should have a variable that cant be extended through these
        //if something special needs to be added it will need to also 
        public int ExtraCasts = 0;
        public int ExtraFlatDamage = 0;
        public float ExtraPercentageDamage = 0f;
        public float ExtraSpeedPercentage = 0f;
        public int penetrate = 0;

        public override void DoDraw(CustomProjectile CurrentProjectile, SpriteBatch spriteBatch)
        {
            //default nothing for modifiers since they don't all need to have art connected with them (like the multicasts etc)
            //but they can if they add dusts or light or something
        }

        public override void DoInitValues(CustomProjectile CurrentProjectile)
        {
            CurrentProjectile.projectile.penetrate += penetrate;
            CurrentProjectile.projectile.damage += ExtraFlatDamage;
        }

        public override void DoPreinitValues(CustomProjectile CurrentProjectile)
        {
            //no defaults like projectiles have
        }

        public override void ApplyMetaValues(CustomShot currentShot)
        {
            //for meta stuff like double cast or reducing the accuracy or anything else that effects the spellcast itself
            //only for modifiercomponents
            currentShot.RemainingCasts += ExtraCasts;
        }

        public override void DoPostInitValues(CustomProjectile CurrentProjectile)
        {
            CurrentProjectile.DamageModifierFraction += ExtraPercentageDamage;
            CurrentProjectile.SpeedModifierFraction += ExtraSpeedPercentage;
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            TooltipLine line = new TooltipLine(mod, "IsModifier", "Modifier");
            line.overrideColor = Color.CornflowerBlue;
            tooltips.Add(line);
        }

    }
}
