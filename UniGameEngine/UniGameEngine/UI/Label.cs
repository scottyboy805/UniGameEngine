using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UniGameEngine.UI
{
    public class Label : UIGraphic
    {
        // Private
        private SpriteFont font = null;
        private string text = "";
        private Color color = Color.Black;

        // Properties
        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        public string Text
        {
            get { return text; }
            set 
            { 
                text = value;

                // Check for null
                if (text == null)
                    text = "";
            }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        // Constructor
        public Label()
        {
            Size = new Vector2(160, 50);
        }

        // Methods
        protected override void DrawGraphic(SpriteBatch spriteBatch, Rectangle rect)
        {
            // Check for font
            if (font == null || text.Length == 0)
                return;

            // Draw text
            spriteBatch.DrawString(font, text, rect.Location.ToVector2(), color);
        }
    }
}
