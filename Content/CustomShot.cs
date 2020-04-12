using CustomWands.Content.Projectiles;
using CustomWands.Content.SpellComponents;
using CustomWands.Content.Wands;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CustomWands.Content
{

    //this class is used to generate the entirety of a spellcast from the wand
    //feeds the components into the CustomProjectiles and spawns them when cast
    //if a component affect the casting action it needs to be handled in this class
    //if a component affects the AI it needs to be handled in CustomProjectiles
    public class CustomShot
    {
        public int RemainingCasts = 1;
        public bool ReadyToUse; //boolean so that the wand and this knows if this is a functional projectile or not, if not and its out of slots the wand needs to give up and try again next update
        public bool ExpectingComponent;

        private List<CustomProjectile> projectilelist = new List<CustomProjectile>(30);
        int currentProjectileIndex = 0;

        private int CurrentManaCost = 0; //private because I think later this may need to be altered and I'd rather keep it as a Get function

        double spreadangle = 0; //determines spread in radians (if this goes below 0 from reduced spread it should lock to 0 at the shoot phase but not earlier)


        public CustomShot(int StartingCasts, double StartingSpread)
        {
            Reset(StartingCasts, StartingSpread);
        }

        public void Reset(int StartingCasts, double StartingSpread)
        {
            ReadyToUse = false;
            ClearCurrentcast();
            spreadangle = StartingSpread;
            RemainingCasts = StartingCasts;
            CurrentManaCost = 0;
        }


        public int GetManaCost()
        {
            //gets the base mana cost of the next cast, added up component-wise for everything going into a spell, including cascading casts or any other effects

            return CurrentManaCost;
        }



        public bool ApplyNextComponent(SpellComponent newcomponent)
        {
            //will return true if this cast is expecting another component
            //whether because the cast itself has multiple projectiles or only modifiers have been loaded so far
            //need to take into consideration a cast being attempted to be built with nulls and only modifiers and must resolve that
            if (newcomponent == null)
            {
                ExpectingComponent = true;
                return ExpectingComponent;
            }
            else
            {
                if (projectilelist.Count <= currentProjectileIndex)
                {
                    projectilelist.Add(new CustomProjectile());
                    //TODO: modifier inheritance
                }

                if (newcomponent is ProjectileComponent)
                {

                    projectilelist[currentProjectileIndex].AddComponent(newcomponent);
                    ReadyToUse = true;
                    RemainingCasts--;
                    if (RemainingCasts <= 0)
                    {
                        ExpectingComponent = false;
                    }
                    else
                    {
                        ExpectingComponent = true;
                    }
                    currentProjectileIndex++;

                }
                else if (newcomponent is ModifierComponent)
                {

                    projectilelist[currentProjectileIndex].AddComponent(newcomponent);
                    newcomponent.ApplyMetaValues(this);
                    ExpectingComponent = true;
                }
                CurrentManaCost += newcomponent.item.mana;

                return ExpectingComponent;
            }
        }




        public void Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            if (ReadyToUse)
            {

                double AimAngle = Math.Atan2(speedX, speedY);

                for (int i = 0; i < projectilelist.Count; i++)
                {
                    double projectileangle = (Main.rand.NextDouble()-0.5f)*spreadangle + AimAngle;



                    //preinitializes stuff that is needed prior to the projectile being created in the world (starting speed is biggest one)
                    projectilelist[i].preinitalize();
                    //creates an instance of the projectile in the world
                    Projectile projectile =
                        Main.projectile[Projectile.NewProjectile(
                            position.X, position.Y, (float)(projectilelist[i].speed * Math.Sin(projectileangle)),
                            (float)(projectilelist[i].speed * Math.Cos(projectileangle)),
                            ProjectileType<CustomProjectile>(),
                            0, 0, player.whoAmI)];

                    //edits that projectile to match the projectile in the list
                    CustomProjectile currentproj = (CustomProjectile)projectile.modProjectile;
                    currentproj.cloneTarget(projectilelist[i]);
                    //initializes and postinitializes to build the stats for the projectile

                    currentproj.initialize();
                    currentproj.postInitialize(damage / WandAbstract.BASEDAMAGEFORMODIFIER, knockBack / WandAbstract.BASEKNOCKBACKFORMODIFIER);
                }
            }
        }


        private void ClearCurrentcast()
        {
            projectilelist.Clear();
            currentProjectileIndex = 0;
            ExpectingComponent = true;
        }



    }
}
