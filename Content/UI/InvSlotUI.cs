using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using CustomWands.Content.Wands;
using Terraria.ID;

namespace CustomWands.Content.UI
{

	//this particular inv slot acts somewhat similar to vanilla inv slots
	//but only works for non-stackables (todo: maybe adjust this to work more generic for other purposes)
	//and to make it easier to select what items can go into those slots
	class InvSlotUI : UIPanel
	{
		//
		public const float panelwidth = 45f;
		public const float panelheight = 45f;
		internal const float panelpadding = 0f;

		public string HintText { get; set; }
		public Texture2D HintTexture { get; set; }
		public Item item;
		protected string HintOnHover { get; set; }

		public InvSlotUI()
		{
			base.Width.Set(panelwidth, 0f);
			base.Height.Set(panelheight, 0f);
			base.SetPadding(panelpadding);

			item = new Item();
			HintTexture = null;
			HintOnHover = null;
			HintText = null;
			OnClick += InvSlotOnClick;
		}

		public virtual bool CanInsertItem(Item item)
		{
			//acts as a normal inventory slot unless this method is overridden
			return true;
		}

		private void InvSlotOnClick(UIMouseEvent evt, UIElement e)
		{

			// Slot has an item
			if (!item.IsAir)
			{
				// Only slot has an item
				if (Main.mouseItem.IsAir)
				{
					Main.PlaySound(SoundID.Grab);
					Main.playerInventory = true;
					Main.mouseItem = item.Clone();

					item.TurnToAir();
				}
				// Mouse has an item
				// Can take mouse item
				else if (CanInsertItem(Main.mouseItem))
				{
					Main.PlaySound(SoundID.Grab);
					Main.playerInventory = true;

					// Swap mouse item and slot item
					var tmp = item.Clone();
					var tmp2 = Main.mouseItem.Clone();

					Main.mouseItem = tmp;

					item = tmp2;

				}
			}
			// Slot has no item
			// Slot can take mouse item
			else if (CanInsertItem(Main.mouseItem))
			{
				Main.PlaySound(SoundID.Grab);
				Main.playerInventory = true;
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
			}
		}

		//directly replaces the item that was in the slot
		public void SetItem(Item newitem)
		{
			item = newitem.Clone();
		}

		//returns a clone of item  in the slot without removing it from the slot
		public Item GetItem()
		{
			if (!item.IsAir)
			{
				return item.Clone();
			} else
			{
				return null;
			}
			
		}


		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			Texture2D texture2D;
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Color drawColor;

			if (HintTexture != null
				&& item.IsAir)
			{
				texture2D = HintTexture;
				drawColor = Color.LightGray * 0.5f;
				if (base.IsMouseHovering)
				{
					Main.hoverItemName = HintText ?? string.Empty;
				}
			}
			else if (item.IsAir)
			{
				return;
			}
			else
			{
				texture2D = Main.itemTexture[item.type];
				drawColor = this.item.GetAlpha(Color.White);
				if (base.IsMouseHovering)
				{
					Main.hoverItemName = item.Name;
					Main.HoverItem = item.Clone();
				}
			}

			var frame =
					!item.IsAir && Main.itemAnimations[item.type] != null
						? Main.itemAnimations[item.type].GetFrame(texture2D)
						: texture2D.Frame(1, 1, 0, 0);

			float drawScale = 1f;
			if ((float)frame.Width > innerDimensions.Width
				|| (float)frame.Height > innerDimensions.Width)
			{
				if (frame.Width > frame.Height)
				{
					drawScale = innerDimensions.Width / (float)frame.Width;
				}
				else
				{
					drawScale = innerDimensions.Width / (float)frame.Height;
				}
			}

			var unreflectedScale = drawScale;
			var tmpcolor = Color.White;
			// 'Breathing' effect
			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, item.type);

			Vector2 drawPosition = new Vector2(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			//todo: globalitem?
			if (item.modItem == null
				|| item.modItem.PreDrawInInventory(spriteBatch, drawPosition, frame, drawColor, drawColor, Vector2.Zero, drawScale))
			{
				spriteBatch.Draw(texture2D, drawPosition, new Rectangle?(frame), drawColor, 0f,
					Vector2.Zero, drawScale, SpriteEffects.None, 0f);

				if (this.item?.color != default(Color))
				{
					spriteBatch.Draw(texture2D, drawPosition, new Rectangle?(frame), drawColor, 0f,
						Vector2.Zero, drawScale, SpriteEffects.None, 0f);
				}
			}

			item.modItem?.PostDrawInInventory(spriteBatch, drawPosition, frame, drawColor, drawColor, Vector2.Zero, drawScale);

		}


	}
}

