using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWands.Content.SpellComponents
{
    class DoubleCast : ModifierComponent
    {
        public new const int ComponentID = 102; //component ID is used for saving/loading and every type of component requires a unique ID

        public override void SetDefaults()
        {
            ExtraCasts = 1;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts an extra spell");
        }
    }
}
