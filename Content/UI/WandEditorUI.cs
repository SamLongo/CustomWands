using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using CustomWands.Content.Wands;
using CustomWands.Content.SpellComponents;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CustomWands.Content.UI
{
    class WandEditorUI : UIState
    {
        const int UIWIDTH = 570;
        const int UIHEIGHT = 220;
        public bool visible = false;
        public bool WaitingForInsert = true;

        WandInvSlotUI WandSlot;

        //for iterating through the respective slots of the components
        List<SpellComponentInvSlotUI> SpellElementSlots = new List<SpellComponentInvSlotUI>();

        public override void OnInitialize()
        {
            DragableUIPanel panel = new DragableUIPanel();
            panel.Width.Set(UIWIDTH, 0);
            panel.Height.Set(UIHEIGHT, 0);
            panel.HAlign = 0.5f;


            WandSlot = new WandInvSlotUI();
            WandSlot.HAlign = 0.5f;
            WandSlot.VAlign = 0f;
            panel.Append(WandSlot);

            int y = 1;
            int x = 0;
            int xcountmax = (int)(UIWIDTH / InvSlotUI.panelwidth);
            for (int i = 0; i < 20; i++)
            {
                SpellComponentInvSlotUI slot = new SpellComponentInvSlotUI();
                SpellElementSlots.Add(slot);
                if (x >= xcountmax)
                {
                    x = 0;
                    y++;
                }

                slot.MarginLeft = x * InvSlotUI.panelwidth;
                slot.MarginTop = y * InvSlotUI.panelheight + 5f;
                panel.Append(slot);
                x++;
            }


            Append(panel);
            WaitingForInsert = true;
        }

        public void UnloadUI()
        {
            WandSlot = null;
            foreach (SpellComponentInvSlotUI slot in SpellElementSlots)
            {
                slot.UnloadUI();
            }
            SpellElementSlots = null;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.

            int CurrWandSize = WandSlot.GetWandSize();


            for (int i = 0; i < CurrWandSize && i < SpellElementSlots.Count; i++)
            {
                SpellElementSlots[i].setUsuable();
            }

            for (int i = CurrWandSize; i < SpellElementSlots.Count; i++)
            {
                SpellElementSlots[i].SetUnusuable();
            }



            if (CurrWandSize > 0 && WaitingForInsert)
            {
                //wand just placed into slot and slots need to be loaded with elements
                WaitingForInsert = false;
                WandAbstract CurrWand = ((WandAbstract)WandSlot.GetItem()?.modItem);
                for (int i = 0; i < CurrWandSize; i++)
                {

                    if (CurrWand.GetComponentAt(i) != null)
                    {
                    SpellElementSlots[i].SetItem(CurrWand.GetComponentAt(i).item);
                    }
                    else
                    {
                        SpellElementSlots[i].item.TurnToAir();
                    }

                } 
            }
            else if (CurrWandSize > 0)
            {

                //wand in slot and the components are being editted
                WandAbstract CurrWand = ((WandAbstract)WandSlot.GetItem().modItem);

                for (int i = 0; i < CurrWandSize; i++)
                {
                    if (SpellElementSlots[i].GetItem() != null)
                    {
                        CurrWand.ReplaceComponentAt(i, ((SpellComponent)SpellElementSlots[i].GetItem().modItem));
                    }
                    else
                    {
                        CurrWand.ReplaceComponentAt(i, null);
                    }

                }
            } 
            else
            {
                //TODO: An issue with directly replacing the wand with another wand causes the components to be deleted
                //figure out some way to modify this logic to prevent that or duplication
                //no wand in slot so it waits for the next insert
                WaitingForInsert = true;
            }
        }

    }
}
