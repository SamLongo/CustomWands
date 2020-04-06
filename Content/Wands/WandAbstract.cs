using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using CustomWands.Content.SpellComponents;
using static Terraria.ModLoader.ModContent;
using System;

namespace CustomWands.Content.Wands
{
    public class WandAbstract : ModItem
    {
        public override bool CloneNewInstances => true;
        
        protected List<SpellComponent> ComponentList;
        public int CurrSlot = 0;

        //special wand variables
        public int wandsize = 1;
        public int CastsPerUse = 1;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Generic wand");
            Tooltip.SetDefault("An error has occured and default wand was created");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "wandsize", "size: " + wandsize);
            tooltips.Add(line);
            for (int i = 0; i < wandsize; i++)
            {
                line = new TooltipLine(mod, "Slot " + i, "Slot " + i + ": " + ComponentList[i]);
                tooltips.Add(line);
            }
        }

        public override void SetDefaults()
        {
            ComponentList = new List<SpellComponent>(wandsize);
            for (int i = 0; i < wandsize; i++)
            {
                ComponentList.Add(null);
            }
        }

        public override void UpdateInventory(Player player)
        {
            // This method handles preparing the next cast in sequence
            if (player.whoAmI == Main.myPlayer)
            {

                SpellComponent CurrItem = ComponentList[CurrSlot];
                if (CurrItem is ProjectileComponent)
                {
                    item.damage = CurrItem.item.damage;
                    item.mana = CurrItem.item.mana;
                    item.knockBack = CurrItem.item.knockBack;
                    item.UseSound = CurrItem.item.UseSound;
                    item.shoot = CurrItem.item.shoot;
                    item.shootSpeed = CurrItem.item.shootSpeed;
                    return; //once the projectile has been determined its fine to just return and not apply any more modifiers
                }
            }
        }

        private bool IncrementSlot()
        {

            CurrSlot = (CurrSlot + 1) % wandsize;
            return (CurrSlot == 0 && wandsize > 1);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            IncrementSlot();
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }


        public override TagCompound Save()
        {
            return new TagCompound
            {
                [nameof(ComponentList)] = ConvertToIDs(),
            };
        }

        public override void Load(TagCompound tag)
        {
            List<int> intlist = tag.Get<List<int>>(nameof(ComponentList));
            ConvertToComponents(intlist);

        }

        public void ReplaceComponentAt(int index, SpellComponent newComponent)
        {
            if (index >= 0 && index < wandsize)
            {
                ComponentList[index] = newComponent;
            }
        }

        public SpellComponent GetComponentAt(int index)
        {
            if (index >= 0 && index < wandsize)
            {
                return ComponentList[index];
            }
            else
            {
                return null;
            }
        }

        private List<int> ConvertToIDs()
        {
            List<int> intlist = new List<int>(20);
            for (int i = 0; i < wandsize; i++)
            {
                intlist.Add(SpellComponent.GetComponentID(ComponentList[i]));
            }

            return intlist;
        }

        private void ConvertToComponents(List<int> intlist)
        {
            for(int i = 0; i < wandsize; i++)
            {
                ComponentList[i] = SpellComponent.CreateComponentByID(intlist[i]); 
            }
        }

    }

}

