using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Village.VillageGame.Menus.Fonts;

namespace Village.VillageGame.Menus
{
    public enum LogDirection
    {
        TopToBottom,
        BottomToTop
    }

    public class LabelLog
    {
        LogDirection dir;

        private Vector2 basePosition;
        private readonly Font font;
        private readonly Rectangle area;
        private List<string> saveDump = new List<string>();
        private Queue<Label> lines = new Queue<Label>();

        private float lineHeight;

        public bool SaveOnDequeue { get; set; }

        public Color color { get; set; }

        public LabelLog(Vector2 position, int width, int height, Font font, LogDirection direction = LogDirection.BottomToTop)
        {
            this.font = font;
            area = new Rectangle((int)position.X, (int)position.Y, width, height);
            // Standard -> Flüchtiges Log.
            SaveOnDequeue = false;

            lineHeight = 0.0f;
            color = Color.Red;
            dir = direction;
            if(direction == LogDirection.BottomToTop)
            {
                basePosition = position;
                
            }
            else
            {
                basePosition = new Vector2(position.X, position.Y + height);
            }
        }

        public void AddEntry(string newEntry)
        {
            Label newLine = new Label(basePosition, newEntry, font, color, area, 1);

            while((lineHeight + newLine.LabelBounds.Height) > area.Height)
            {
                if(lines.Count <= 0)
                {
                    break;
                }
                RemoveLastLine();         
            }

            lines.Enqueue(newLine);
            RecalculateDrawPositions();
        }

        private void RemoveLastLine()
        {
            if(SaveOnDequeue)
            {
                saveDump.Add(lines.Peek().Text);
            }
            lineHeight -= lines.Peek().LabelBounds.Height;
            lines.Dequeue();
        }

        private void RecalculateDrawPositions()
        {
            lineHeight = 0.0f;
            Vector2 nextPosition = new Vector2(basePosition.X, basePosition.Y);
            foreach(Label label in lines)
            {
                lineHeight += label.LabelBounds.Height;
                label.Position = nextPosition;
                if (dir == LogDirection.TopToBottom)
                {
                    nextPosition = new Vector2(basePosition.X, nextPosition.Y - label.LabelBounds.Height + 3);
                }
                else
                {
                    nextPosition = new Vector2(basePosition.X, nextPosition.Y + label.LabelBounds.Height + 3);
                }                               
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Label label in lines)
            {
                label.Draw(spriteBatch, SpriteEffects.None);
            }
        }

    }
}
