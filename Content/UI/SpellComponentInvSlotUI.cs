
using Terraria;
using CustomWands.Content.SpellComponents;
using Microsoft.Xna.Framework.Graphics;

namespace CustomWands.Content.UI
{
    class SpellComponentInvSlotUI : InvSlotUI
    {

        //isusable is set so that this slot knows whether it can be interacted with or not
        //deletes items stored in it when it becomes unusable
        //hopefully that item should be stored in the wand's info to be retrieved or used later
        public bool isusable = false;

        public SpellComponentInvSlotUI() : base()
        {

        }

        public override bool CanInsertItem(Item item)
        {
            //acts as a normal inventory slot unless this method is overridden
            if (isusable)
            {
                return item.modItem is SpellComponent;
            }
            else
            {
                return false; //can't use this slot while it's disabled
            }

        }

        public void SetUnusuable()
        {
            isusable = false;
            item.TurnToAir();
        }

        public void setUsuable()
        {
            isusable = true;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (isusable)
            {
                base.DrawSelf(spriteBatch);
            }
            else
            {
                //don't draw if its not usable
            }
        }
    }
}
