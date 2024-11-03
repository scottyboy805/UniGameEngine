using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace UniGameEngine.UI
{
    [DataContract]
    public class Image : UIGraphic
    {
        // Private
        [DataMember(Name = "Texture")]
        private Texture2D texture = null;
        [DataMember(Name = "Color")]
        private Color color = Color.White;

        // Properties
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }


        // Methods
        protected override void DrawGraphic(SpriteBatch spriteBatch, Rectangle rect)
        {
            // Get draw texture
            Texture2D drawTexture = texture != null
                ? texture
                : White;

            // Draw image
            spriteBatch.Draw(drawTexture, rect, color);
        }
    }
}
