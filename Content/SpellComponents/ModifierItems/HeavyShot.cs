using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWands.Content.SpellComponents
{
    class HeavyShot : ModifierComponent
    {
        public new const int ComponentID = 101; //component ID is used for saving/loading and every type of component requires a unique ID

        public override void SetDefaults()
        {
            ExtraPercentageDamage = 0.5f;
            ExtraSpeedPercentage = -0.5f;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases damage but greatly reduces projectile speed");
        }
    }
}
