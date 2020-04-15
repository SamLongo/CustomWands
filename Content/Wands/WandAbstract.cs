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
using CustomWands.Content.Projectiles;

namespace CustomWands.Content.Wands
{
    public class WandAbstract : ModItem
    {
        public override bool CloneNewInstances => true;

        protected List<SpellComponent> ComponentList;
        public CustomShot shotobject;
        public int CurrSlot = 0;

        public double Spread = 0f;

        //special wand variables
        public int wandsize = 1;
        public int CastsPerUse = 1;

        private bool WandCanApplyNextComponent = true;

        public const int BASEDAMAGEFORMODIFIER = 100; //this exists so that the % bonuses from accessories and whatnot are applied flat bonuses screw up
        public const float BASEKNOCKBACKFORMODIFIER = 10;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Generic wand");
            Tooltip.SetDefault("An error has occured and default wand was created");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.RemoveAll(x => x.Name == "Knockback");
            tooltips.RemoveAll(x => x.Name == "Damage");
            
            TooltipLine line = new TooltipLine(mod, "wandsize", "size: " + wandsize);
            tooltips.Add(line);
            line = new TooltipLine(mod, "Spread", "Wand Spread: " + MathHelper.ToDegrees((float)Spread) + "Degrees");
            tooltips.Add(line);
            for (int i = 0; i < wandsize; i++)
            {
                line = new TooltipLine(mod, "Slot " + i, "Slot " + i + ": " + ComponentList[i]);
                tooltips.Add(line);
            }
        }

        public override void SetDefaults()
        {
            //holds the universal defaults for all wands
            //make sure that in the SetDefaults() for any wand includes base.setdefaults()
            item.damage = BASEDAMAGEFORMODIFIER;
            item.knockBack = BASEKNOCKBACKFORMODIFIER;
            item.autoReuse = true;
            item.magic = true;
            item.noMelee = true;

            shotobject = new CustomShot(CastsPerUse, Spread);
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
                ShotUpdate();
                if(!shotobject.ReadyToUse)
                {
                    //wasn't successful in making a projectile in the first pass since the wand wrapped around, try again
                    WandCanApplyNextComponent = true;
                    shotobject.Reset(CastsPerUse, Spread);
                    ShotUpdate();
                }
            }
        }


        private void ShotUpdate()
        {
            while (shotobject.ExpectingComponent && WandCanApplyNextComponent)
            {
                if (ComponentList[CurrSlot] != null)
                {
                    if (shotobject.ExpectingComponent && WandCanApplyNextComponent)
                    {
                        shotobject.ApplyNextComponent(ComponentList[CurrSlot]);
                        WandCanApplyNextComponent = IncrementSlot();
                    }
                }
                else
                {
                    WandCanApplyNextComponent = IncrementSlot(); // so that it ignores empty spaces on the wand
                }
            }

            item.mana = shotobject.GetManaCost();
        }



        private bool IncrementSlot()
        {
            CurrSlot = CurrSlot + 1;
            bool wrapped = (CurrSlot != wandsize);
            CurrSlot = CurrSlot % wandsize;
            return wrapped;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            shotobject.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
            WandCanApplyNextComponent = true; //this should never be neccessary but it should help prevent deadlocks
            shotobject.Reset(CastsPerUse, Spread);

            return false;
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

        // NetSend and NetRecieve allow the components to be saved when the item is taken out of the inventory in multiplayer
        public override void NetSend(BinaryWriter writer)
        {
            List<int> intlist = ConvertToIDs();
            byte[] bytearray = new byte[wandsize];
            for (int i = 0; i < ComponentList.Count; i++)
            {
                bytearray[i] = (byte)intlist[i];
            }
            writer.Write(bytearray);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            List<int> intlist = new List<int>(wandsize);
            byte[] bytearray = new byte[wandsize];
            bytearray = reader.ReadBytes(wandsize);
            for (int i = 0; i < wandsize; i++)
            {
                intlist.Add((int)bytearray[i]);
            }

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
            for (int i = 0; i < wandsize; i++)
            {
                ComponentList[i] = SpellComponent.CreateComponentByID(intlist[i]);
            }
        }

    }

}

