using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;

namespace CustomWands.Content.GFX
{
    public static class GFX
    {

        private const string PROJECTILES_DIRECTORY = "Content/GFX/ProjectileArt/";

        public static Texture2D SPARKBOLT;
        public static Texture2D BOUNCINGBOLT;

        private const string _SPARKBOLT = PROJECTILES_DIRECTORY + "SparkBoltProjectile";
        private const string _BOUNCINGBOLT = PROJECTILES_DIRECTORY + "BouncingBoltProjectile";


        public static void LoadGfx()
        {
            //TODO: learn more about inspection so that this doesn't need to be manually added to every time and can just construct the textures based on some rules

            Mod loader = ModLoader.GetMod("CustomWands");

            SPARKBOLT = loader.GetTexture(_SPARKBOLT);
            BOUNCINGBOLT = loader.GetTexture(_BOUNCINGBOLT);
        }

        public static void UnloadGfx()
        {
            SPARKBOLT = null;
            BOUNCINGBOLT = null;
        }
    }
}