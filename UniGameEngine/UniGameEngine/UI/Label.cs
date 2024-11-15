using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;

namespace UniGameEngine.UI
{
    [Flags]
    public enum FontStyle
    {
        Normal = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
    }

    [DataContract]
    public class Label : UIGraphic
    {
        // Private
        [DataMember(Name = "Font")]
        private FontSystem font = null;
        [DataMember(Name = "FontSize")]
        private int fontSize = 24;
        [DataMember(Name = "Text")]
        private string text = "";
        [DataMember(Name = "Style")]
        private FontStyle style = FontStyle.Normal;
        [DataMember(Name = "Effect")]
        private FontSystemEffect effect = FontSystemEffect.None;
        [DataMember(Name = "Color")]
        private Color color = Color.Black;

        private SpriteFontBase drawFont = null;

        // Properties
        public FontSystem Font
        {
            get { return font; }
            set { font = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set 
            { 
                fontSize = value;
                drawFont = null;
            }
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

        public FontStyle Style
        {
            get { return style; }
            set 
            { 
                style = value; 
                drawFont = null;
            }
        }

        public FontSystemEffect Effect
        {
            get { return effect; }
            set { effect = value; }
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
        protected override void DrawGraphic(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, Vector2 pivot)
        {
            // Check for font
            if (font == null || text.Length == 0)
                return;
            
            // Create font on demand
            if (drawFont == null)
                drawFont = font.GetFont(fontSize);

            // Check for underline
            TextStyle textStyle = (style & FontStyle.Underline) != 0
                ? TextStyle.Underline
                : TextStyle.None;

            // Draw text
            spriteBatch.DrawString(drawFont, text, position, color, rotation, pivot, scale, 0f, 0f, 0f, textStyle, effect);
            //spriteBatch.DrawString(drawFont, text, position, color, rotation, pivot, scale, SpriteEffects.None, 0f);
        }
    }
}
