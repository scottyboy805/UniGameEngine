using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using UniGameEngine.Graphics;
using UniGameEngine.Scene;

namespace UniGameEngine.UI
{
    [DataContract]
    public class Image : UIGraphic
    {
        // Private
        [DataMember(Name = "Sprite")]
        private Sprite sprite = null;
        [DataMember(Name = "Color")]
        private Color color = Color.White;

        // Properties
        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        // Methods
        protected override void DrawGraphic(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, Vector2 pivot)
        {
            DrawGraphic(spriteBatch, position, rotation, scale, pivot, color);
        }

        protected void DrawGraphic(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, Vector2 pivot, Color color)
        {
            // Get draw texture
            Texture2D drawTexture = sprite != null && sprite.Texture != null
                ? sprite.Texture
                : White;

            // Get source rect
            Rectangle drawRect = sprite != null
                ? sprite.SourceRect
                : drawTexture.Bounds;

            // Adjust scale to fit content into size
            scale = GetAdjustedScale(drawRect.Size.ToVector2());

            // Draw image
            spriteBatch.Draw(drawTexture,
                position,
                drawRect,
                color,
                rotation,
                pivot,
                scale,
                SpriteEffects.None,
                0f);
        }

        public static Image Create(GameObject parent)
        {
            // Create image object
            Image image = parent.CreateObject<Image>("Image");
            CreateDefaultImage(image);

            return image;
        }

        public static Image Create(GameScene scene)
        {
            // Create image object
            Image image = scene.CreateObject<Image>("Image");
            CreateDefaultImage(image);

            return image;
        }

        internal static void CreateDefaultImage(Image image)
        {
            // Get sprite
            image.Size = new Vector2(100, 100);
            image.sprite = new Sprite(image.Game.Content.Load<Texture2D>("UI/Default"),
                new Rectangle(194, 2, 100, 100));
        }
    }
}
