using System.Collections.Generic;
using CustomWands.Content;
using CustomWands.Content.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using CustomWands.Content.Wands;
using Terraria.GameInput;
using CustomWands.Content.GFX;

namespace CustomWands
{
    public class CustomWands : Mod
    {
        public static ModHotKey EditWandHotKey;
        internal UserInterface MyInterface;
        internal WandEditorUI MyUIstate;

        public override void Load()
        {
            // this makes sure that the UI doesn't get opened on the server
            // the server can't see UI, can it? it's just a command prompt
            if (!Main.dedServ)
            {
                EditWandHotKey = RegisterHotKey("Open Wand Interface", "y");
                MyInterface = new UserInterface();
                MyUIstate = new WandEditorUI();
                MyUIstate.Activate();
                MyInterface.SetState(MyUIstate);
                GFX.LoadGfx();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (EditWandHotKey.JustPressed)
            {
                MyUIstate.visible = !MyUIstate.visible;
                MyInterface.SetState(MyUIstate);
            }

            if (!Main.gameMenu && MyUIstate.visible)
            {
                MyInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "CustomWands: Wand Editor",
                    delegate {
                        if (MyUIstate.visible)
                        {
                            MyInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void Unload()
        {
            base.Unload();
            MyInterface = null;
            MyUIstate.UnloadUI();
            GFX.UnloadGfx();
        }



    }
}