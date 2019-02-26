using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.Menus.Controls;
using Zeus.Hermes;

namespace Village.VillageGame.Menus
{
    public class Menu
    {
        private SpriteBatch spriteBatch;
        private Color backgroundColor = Color.CornflowerBlue;
        private List<BasicButton> _buttons = new List<BasicButton>();
        private List<Label> _labels = new List<Label>();
        private List<LabelLog> _labelLogs = new List<LabelLog>();
        private readonly GraphicsDevice GraphicsDevice;

        public List<BasicButton> Buttons { get => _buttons; set => _buttons = value; }
        public List<Label> Labels { get => _labels; set => _labels = value; }
        public List<LabelLog> LabelLogs { get => _labelLogs; set => _labelLogs = value; }
        public Color BackgroundColor { get => backgroundColor; set => backgroundColor = value; }

        public Menu(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Fügt dem Menu ein Button hinzu.
        /// Dieser Buton wird in jedem Update-Aufruf geupdatet
        /// und automatisch mit dem Rest des Menüs gezeichnet.
        /// </summary>
        /// <param name="button">Dieser Button wird hinzugefügt.</param>
        public void Add(BasicButton button) => Buttons.Add(button);

        /// <summary>
        /// Fügt dieses Label dem Menü hinzu.
        /// </summary>
        /// <param name="label">Füg.Mich.Zu.</param>
        public void Add(Label label) => Labels.Add(label);

        /// <summary>
        /// Fügt dem Menü ein neues LabelLog hinzu.
        /// </summary>
        /// <param name="log">Das neuhinzugefügte Label log.</param>
        public void Add(LabelLog log) => LabelLogs.Add(log);

        /// <summary>
        /// Entfernt alle bis jetzt hinzugefügten Elemente.
        /// </summary>
        public void Clear()
        {
            Buttons = new List<BasicButton>();
            Labels = new List<Label>();
            LabelLogs = new List<LabelLog>();
        }

        public virtual void Update()
        {
            MouseState msState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (BasicButton button in Buttons)
            {
                button.Update(msState, keyboardState);
            }
        }

        public virtual void Draw()
        {
            GraphicsDevice.Clear(backgroundColor);

            for (int i = 0; i < LabelLogs.Count; i++)
            {
                LabelLogs[i].Draw(spriteBatch);
            }

            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].Draw(spriteBatch);
            }

            for (int i = 0; i < Labels.Count; i++)
            {
                Labels[i].Draw(spriteBatch, SpriteEffects.None);
            }
        }
    }
}
