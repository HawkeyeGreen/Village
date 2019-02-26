using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Village.VillageGame
{
    public static class KeyBinds
    {
        private static Keys cameraMoveUp = Keys.Up;
        private static Keys cameraMoveDown = Keys.Down;
        private static Keys cameraMoveRight = Keys.Right;
        private static Keys cameraMoveLeft = Keys.Left;

        private static Keys moveZLevelUp = Keys.PageUp;
        private static Keys moveZLevelDown = Keys.PageDown;

        public static Keys CameraMoveUp
        {
            get => cameraMoveUp;
            set => cameraMoveUp = value;
        }

        public static Keys CameraMoveDown
        {
            get => cameraMoveDown;
            set => cameraMoveDown = value;
        }

        public static Keys CameraMoveLeft
        {
            get => cameraMoveLeft;
            set => cameraMoveLeft = value;
        }

        public static Keys CameraMoveRight
        {
            get => cameraMoveRight;
            set => cameraMoveRight = value;
        }

        public static Keys MoveZLevelUp
        {
            get => moveZLevelUp;
            set => moveZLevelUp = value;
        }

        public static Keys MoveZLevelDown
        {
            get => moveZLevelDown;
            set => moveZLevelDown = value;
        }
    }
}
