using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Village.VillageGame.Menus.Fonts;
using Zeus.Hermes;

namespace Village.VillageGame.Menus
{
    public class Label
    {
        /// <summary>
        /// Der Text, welchen das Label anzeigt.
        /// </summary>
        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                textSize = FontManager.GetInstance().Fonts[font].MeasureString(text);
            }
        }

        public int NumberOfLines
        {
            get
            {
                if(linedText != null)
                {
                    return linedText.Count;
                }
                return 0;
            }
        }

        public bool MultiLine
        {
            get;
            set;
        }

        public float Rotation
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Vector2 TextSize
        {
            get => textSize;
        }

        public Rectangle LabelBounds => labelBounds;

        public Color color
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        public Font Font
        {
            get => font;
            set => font = value;
        }

        public bool Visible { get; set; } = true;

        private Font font;
        private Vector2 textSize;
        private float scale;
        private Rectangle labelBounds;

        private List<string> linedText = null;
        
        /// <summary>
        /// Initialisiert das Label. Dadurch ist es voll funktionstüchtig.
        /// </summary>
        public Label(Vector2 pos, string txt, Font chosenFont, Color clr, Rectangle BoundingBox, float scale, bool scaling = false)
        {
            Enabled = true;
            Position = pos;
            text = txt;
            font = chosenFont;
            textSize = FontManager.GetInstance().Fonts[font].MeasureString(text);
            color = clr;
            if(scaling)
            {
                float scaleX = BoundingBox.Width / textSize.X;
                float scaleY = BoundingBox.Height / textSize.Y;
                Scale = MathHelper.Min(scaleX, scaleY);
                labelBounds = BoundingBox;
            }
            else
            {
                Scale = scale;
                labelBounds = new Rectangle(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), Convert.ToInt32(Math.Ceiling(textSize.X)), Convert.ToInt32(Math.Ceiling(textSize.Y)));
                if (BoundingBox.Width < textSize.X)
                {
                    linedText = LineText(text, BoundingBox.Width);
                    labelBounds.Height = Convert.ToInt32(Math.Ceiling(textSize.Y * linedText.Count));
                    MultiLine = true;
                }
            }


            if(Scale <= 0.0f)
            {
                Scale = 1;
            }
        }

        public List<string> LineText(string origin, int maxWidth)
        {
            List<string> Return = new List<string>();

            for(int index = origin.Length - 1; index > 0; index--)
            {
                if (char.IsWhiteSpace(origin[index]))
                {
                    string candidate, candidateRest;
                    candidate = origin.Substring(0, index + 1);
                    candidateRest = origin.Substring(index);
                    if(FontManager.GetInstance().Fonts[font].MeasureString(candidate).X <= maxWidth)
                    {
                        Return.Add(candidate.Trim());
                        if(FontManager.GetInstance().Fonts[font].MeasureString(candidateRest).X <= maxWidth)
                        {
                            Return.Add(candidateRest.Trim());
                        }
                        else
                        {
                            Return.AddRange(LineText(candidateRest, maxWidth));
                        }
                        break;
                    }
                }
            }
            if(Return.Count == 0)
            {
                Return.Add(origin.TrimStart());
            }
            return Return;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            if(Enabled && Visible)
            {
                if(MultiLine && linedText != null)
                {
                    for(int y = 0; y < linedText.Count; y++)
                    {
                        spriteBatch.DrawString(FontManager.GetInstance().Fonts[font], linedText[y], new Vector2(Position.X, Position.Y + y * (TextSize.Y + 1)), color, Rotation, new Vector2(), scale, spriteEffects, 0.0f);
                    }
                }
                else
                {
                    spriteBatch.DrawString(FontManager.GetInstance().Fonts[font], Text, Position, color, Rotation, new Vector2(), scale, spriteEffects, 0.0f);
                }
            }
        }

        public bool ToggleVisible()
        {
            Visible ^= true;
            return Visible;
        }

        public bool ToggleEnabled()
        {
            Enabled ^= true;
            return Enabled;
        }
    }
}
