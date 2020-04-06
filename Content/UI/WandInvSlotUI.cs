using Terraria;
using CustomWands.Content.Wands;

namespace CustomWands.Content.UI
{
    class WandInvSlotUI : InvSlotUI
    {

        public WandInvSlotUI() : base()
        {

        }

        public override bool CanInsertItem(Item item)
        {
            //acts as a normal inventory slot unless this method is overridden
            return item.modItem is WandAbstract;
        }

        //gets the size of the wand in the slot or returns 0 if there is no wand
        public int GetWandSize()
        {
            if (!item.IsAir)
            {
                //todo: currently will crash if a non wand gets into the slot somehow
                return ((WandAbstract)item.modItem).wandsize;

            } else
            {
                return 0;
            }

        }
    }
}
