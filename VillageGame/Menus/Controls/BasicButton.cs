using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.Menus.Controls
{
    public class BasicButton
    {
        private bool pressed = false;
        public bool Pressed => pressed;

        private bool mouseOver = false;
        public bool MouseOver => mouseOver;

        private bool selected = false;
        public bool Selected
        {
            get => selected;
            set => selected = value;
        }

        private bool enabled = true;
        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        private bool visible = true;
        public bool Visible
        {
            get => visible;
            set => visible = value;
        }

        private Rectangle buttonArea;
        private Label label;

        private KeyboardState oldKBState;
        private MouseState oldMState;

        private Keys boundKey;
        public Keys BoundKey => boundKey;

        public string Text => label.Text;

        public BasicButton(string text, Rectangle buttonArea, Keys bKey)
        {
            label = new Label(buttonArea.Location.ToVector2(), text, Fonts.Font.Test, Color.White, buttonArea, 1.0f, true);
            this.buttonArea = buttonArea;
            boundKey = bKey;
            oldKBState = Keyboard.GetState();
            oldMState = Mouse.GetState();
        }

        public void Update(MouseState mState, KeyboardState kbState)
        {
            if (enabled)
            {
                pressed = false;
                mouseOver = false;

                if (buttonArea.Contains(mState.Position))
                {
                    mouseOver = true;
                    if (mState.LeftButton == ButtonState.Pressed && oldMState.LeftButton == ButtonState.Released)
                    {
                        pressed = true;
                    }
                }

                if (oldKBState.IsKeyUp(boundKey) && kbState.IsKeyDown(boundKey))
                {
                    pressed = true;
                }

                oldKBState = kbState;
                oldMState = mState;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (enabled && visible)
            {
                if(mouseOver)
                {
                    label.color = Color.Gold;
                }
                else
                {
                    label.color = Color.White;
                }
                label.Draw(spriteBatch, SpriteEffects.None);
            }
        }

        public bool ToggleVisibility()
        {
            visible ^= true;
            return visible;
        }

        public bool ToggleEnable()
        {
            enabled ^= true;
            return enabled;
        }
    }
}
