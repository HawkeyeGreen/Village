using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Village.VillageGame.DatabaseManagement;
using Village.VillageGame.Menus.Controls;
using Village.VillageGame.Menus.Fonts;
using Village.VillageGame.TagSystem;
using Village.VillageGame.World.ReactionSystem;

namespace Village.VillageGame.Menus
{
    public class TestArena : Menu
    {
        public TestArena(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) : base(spriteBatch, graphicsDevice)
        {
            Localization.Local.GetInstance().ChangeLanguage(Localization.Language.ENG);

            BackgroundColor = Color.Black;

            Material material = new Material("HumanSkinMaterial");

            Add(new Label(new Vector2(), material.DisplayName , Font.Test, Color.NavajoWhite, new Rectangle(0, 0, 200, 50), 1));
            Add(new Label(new Vector2(0, 50), material.DisplayDescription, Font.Test, Color.NavajoWhite, new Rectangle(0, 0, 300, 50), 1));
        }
    }
}
