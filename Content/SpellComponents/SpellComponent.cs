using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace CustomWands.Content.SpellComponents
{
    public class SpellComponent : ModItem
    {
        public const int ComponentID = 0; //component ID is used for saving/loading and every type of component requires a unique ID
        public SpellComponent()
        {
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("generic component");
            Tooltip.SetDefault("An error has occured and default component was created");
        }

        public static SpellComponent CreateComponentByID(int ID)
        {
            //big case switch for every component that is added. neccesary for save/load functionality
            Item newitem = new Item();

            switch (ID)
            {
                default:
                    return null;//either there was no component in that slot and this is correct or something went wrong and it was deleted (most likely missing componentID)

                //the following 2 lines and its repetition is literally the reason for all the excess code in CreateComponentByID and GetComponentID
                //to do a case switch you need a constant reference to a value
                //to create an actual item instance you need a constant reference to a type
                //together it means for saving and loading all of this is required
                //potential TODO: rework the save/load system to not require any of this
                case (SparkBolt.ComponentID):
                    newitem.SetDefaults(ItemType<SparkBolt>());
                    return (SpellComponent)newitem.modItem;
                case (BouncingBolt.ComponentID):
                    newitem.SetDefaults(ItemType<BouncingBolt>());
                    return (SpellComponent)newitem.modItem;
            }
        }

        
        public static int GetComponentID(SpellComponent component)
        {
            //use this to get the ID since they are consts and aren't accessed normally as class members
            if(component != null)
            {
                //How I think this line works
                //some kind of inspection on the type of the component being passed in to find the constant field "componentID"
                //it then gets the value as an object and converts it to a string before finally parsing it into an int
                //basically magic to prevent the need for another big case switch statement
                return int.Parse(component.GetType().GetField("ComponentID").GetValue(null).ToString());
            } else
            {
                return 0;
            }
        }
        
    }
}
