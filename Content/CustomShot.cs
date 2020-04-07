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
        public int RemainingCasts;
        public bool ReadyToUse; //boolean so that the wand and this knows if this is a functional projectile or not, if not and its out of slots the wand needs to give up and try again next update
        public bool ExpectingComponent;

        private List<CustomProjectile> projectilelist = new List<CustomProjectile>(30);
        int currentProjectileIndex = 0;


        public CustomShot(int StartingCasts)
        {
            Reset(StartingCasts);
        }

        public void Reset(int StartingCasts)
        {
            ClearCurrentcast();
            RemainingCasts = StartingCasts;
        }



        public bool ApplyNextComponent(SpellComponent newcomponent)
        {
            //will return true if this cast is expecting another component
            //whether because the cast itself has multiple projectiles or only modifiers have been loaded so far
            //need to take into consideration a cast being attempted to be built with nulls and only modifiers and must resolve that
            if (newcomponent is ProjectileComponent)
            {
                
                projectilelist[currentProjectileIndex].AddComponent(newcomponent);
                ReadyToUse = true;
                ExpectingComponent = false;
            }
            else if (newcomponent is ModifierComponent)
            {

            }


            return false;
        }


        public void Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for(int i=0; i < projectilelist.Count; i++) 
            {
                projectilelist[i].preinitalize();

                //creates an instance of the projectile in the world
                Projectile projectile =
                    Main.projectile[Projectile.NewProjectile(
                        position.X, position.Y, (float)(projectilelist[i].speed*Math.Sin(Math.Atan2(speedX,speedY))),
                        (float)(projectilelist[i].speed * Math.Cos(Math.Atan2(speedX, speedY))),
                        ProjectileType<CustomProjectile>(),
                        0, 0, player.whoAmI)];

                //edits that projectile to match the projectile in the list
                CustomProjectile currentproj = (CustomProjectile)projectile.modProjectile;
                
                currentproj.cloneTarget(projectilelist[i]);
                currentproj.initialize();
                currentproj.postInitialize(damage / WandAbstract.BASEDAMAGEFORMODIFIER, knockBack / WandAbstract.BASEKNOCKBACKFORMODIFIER);
            }
        }


        private void ClearCurrentcast()
        {
            projectilelist.Clear();
            projectilelist.Add(new CustomProjectile());
            ExpectingComponent = true;
        }



    }
}
