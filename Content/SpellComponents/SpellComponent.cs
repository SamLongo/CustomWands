using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using CustomWands.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;

namespace CustomWands.Content.SpellComponents
{
    public abstract class SpellComponent : ModItem
    {
        public const int ComponentID = 0; //component ID is used for saving/loading and every type of component requires a unique ID

        public override bool CloneNewInstances => true;

        public override void SetDefaults()
        {
            
            base.SetDefaults();
            
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
                case (HeavyShot.ComponentID):
                    newitem.SetDefaults(ItemType<HeavyShot>());
                    return (SpellComponent)newitem.modItem;
                case (DoubleCast.ComponentID):
                    newitem.SetDefaults(ItemType<DoubleCast>());
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

        public virtual void ApplyMetaValues(CustomShot currentShot) { }

        public abstract void DoPreinitValues(CustomProjectile CurrentProjectile);

        public abstract void DoApplyValues(CustomProjectile CurrentProjectile);

        public virtual void DoAI(CustomProjectile CurrentProjectile)
        {
            //by default components have no AI to add
        }

        public virtual bool DoOnTileCollide(CustomProjectile CurrentProjectile, Vector2 oldVelocity)
        {
            //by default components have no effect on colliding with a block (just get deleted)
            return true;
        }

        public virtual void DoOnHitNPC(CustomProjectile CurrentProjectile, NPC target, int damage, float knockback, bool crit)
        {
            //by default components have no special effect on colliding with an NPC (deal damage and get deleted)
        }

        public virtual void DoKill(CustomProjectile CurrentProjectile, int timeLeft)
        {
            //by default components have no special effect when they are deleted
        }

        public abstract void DoDraw(CustomProjectile CurrentProjectile, SpriteBatch spriteBatch);
        //used for stuff like adding the actual projectile or dusts
        //every projectilecomponent REQUIRED TO override this
        //modifiercomponents optional

        public virtual void DoPostInitValues(CustomProjectile customProjectile)
        {
            //do nothing
        }


        //set of helper functions to apply standard AI pieces to the components
        protected void ApplyGravity(CustomProjectile CurrentProjectile)
        {
            CurrentProjectile.projectile.velocity.Y += 0.2f;
        }

    }
}
