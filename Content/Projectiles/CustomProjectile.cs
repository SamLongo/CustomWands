using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CustomWands.Content.SpellComponents;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace CustomWands.Content.Projectiles
{
    public class CustomProjectile : ModProjectile
    {
        //a custom projectile should only own 1 type of ProjectileComponent
        //combined with any number of modifiercomponents
        //it can also own 1 CustomProjectile that is cast when this is killed
        //any child projectiles inherit modifiers from the first
        //but any modifiers after the first projectile are only given to children
        //if theres a chain of casts on kill then this needs to pass it down the chain if its asked to add a cast on kill

        List<SpellComponent> componentlist = new List<SpellComponent>(20);

        //flat bonuses and modifiers are added up during the initialize phase (damage hidden in projectile.damage)
        public float speed = 0;
        public float angle = 0;
        public int bounces = 0;
        //this is only important in the case of antigravity/reverse gravity component wanting to turn this off despite a projectile normally having graviy
        //don't exactly want to just add it multiple times cause then no gravity could become inverted gravity
        public bool hasgravity = true;


        //fractional bonuses are added up during the initialize phase and then applied at the postinitialize phase
        //basically multiple multiplicative bonuses stack together additively ie +50% twice is just +100% intead of +125%
        //sum up in a way that 0 is baseline for 100% damage and speed -1f is -100% or halved and 1f is +100f is +100% or doubled
        public float DamageModifierFraction = 0f;
        public float SpeedModifierFraction = 0f;


        public override void SetDefaults()
        {
            projectile.timeLeft = 0;
            base.SetDefaults();
        }

        public override void AI()
        {
            angle = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            //note that if the projectile should actually be rotated hitbox and all, the AI should handle that (only projectilecomponent unless some special case comes up)
            //this angle being made is a simplified one for the draw() function if nothing overrides it and its a projectile that has a direction to it
            foreach (SpellComponent component in componentlist)
            {
                component.DoAI(this);
            }
        }

        public void preinitalize()
        {
            foreach (SpellComponent component in componentlist)
            {
                component.DoPreinitValues(this);
            }
            DamageModifierFraction = 0f;
            SpeedModifierFraction = 0f;
    }

        public void initialize()
        {
            foreach (SpellComponent component in componentlist)
            {
                component.DoApplyValues(this);
            }
        }

        public void postInitialize(float damagemodifier, float knockbackmodifier)
        {
            //for % based modifications run after all the numbers have been put in
            //damage modifier come in as 1f being the baseline so they need to be changed to fit the scheme

            foreach (SpellComponent component in componentlist)
            {
                component.DoPostInitValues(this);
            } 

            damagemodifier -= 1f;

            DamageModifierFraction += damagemodifier;
            if(DamageModifierFraction < 0)
            {
                DamageModifierFraction = 1f/(-(DamageModifierFraction-1));
            } else
            {
                DamageModifierFraction += 1f;
            }

            if (SpeedModifierFraction < 0)
            {
                SpeedModifierFraction = 1f / (-(SpeedModifierFraction - 1));
            }
            else
            {
                SpeedModifierFraction += 1f;
            }

            projectile.damage = (int)(projectile.damage * DamageModifierFraction);
            projectile.knockBack *= knockbackmodifier;
            projectile.velocity = new Vector2(projectile.velocity.X * SpeedModifierFraction, projectile.velocity.Y * SpeedModifierFraction);
        }


        public void AddComponent(SpellComponent component)
        {
            componentlist.Add(component);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool delete = true; ;
            foreach (SpellComponent component in componentlist)
            {
                if (!component.DoOnTileCollide(this, oldVelocity))
                {
                    delete = false;
                }
                
            }
            return delete;
        }

        public override void Kill(int timeLeft)
        {

            foreach (SpellComponent component in componentlist)
            {
                component.DoKill(this, timeLeft);
            }
            //TODO: next projectile stuff

            componentlist.Clear(); //not sure if this is necessary but it might be needed for preventing memory leak
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            foreach (SpellComponent component in componentlist)
            {
                component.DoOnHitNPC(this, target, damage, knockback, crit);
            }
        }



        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            foreach (SpellComponent component in componentlist)
            {
                component.DoDraw(this, spriteBatch);
            }
            return false;
        }

        public override ModProjectile Clone()
        {
            CustomProjectile newprojectile = new CustomProjectile();
            for (int i = 0; i < componentlist.Count; i++)
            {
                newprojectile.componentlist.Add((SpellComponent)componentlist[i].Clone());
            }
            return newprojectile;
        }

        public void cloneTarget(CustomProjectile tobeCloned)
        {
            //kind of a horrifying hack that allows the projectile to be created before being loaded into the world and then passed to the projectile in the world using this function
            for (int i = 0; i < tobeCloned.componentlist.Count; i++)
            {

                componentlist.Add((SpellComponent)tobeCloned.componentlist[i].Clone());
            }


        }
    }
}
