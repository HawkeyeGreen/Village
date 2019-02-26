using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Village.VillageGame.Menus.Fonts
{
    public class FontManager
    {
        private static FontManager Instance;
        public Dictionary<Font, SpriteFont> Fonts
        {
            get;
        }

        private FontManager(ContentManager Content)
        {
            Fonts = new Dictionary<Font, SpriteFont>
            {
                [Font.Test] = Content.Load<SpriteFont>("TestFont")
            };
        }

        public static void CreateInstance(ContentManager Content)
        {
            Instance = new FontManager(Content);
        }

        public static FontManager GetInstance()
        {
            if(Instance != null)
            {
                return Instance;
            }
            throw new NullReferenceException();
        }
    }
}
