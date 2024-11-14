using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace UniGameEngine.Graphics
{
    [DataContract]
    public sealed class SpriteRenderer : Renderer
    {
        // Private
        [DataMember(Name = "Sprite")]
        private Sprite sprite = null;
        [DataMember(Name = "Color")]
        private Color color = Color.White;
        [DataMember(Name = "SpriteFlip")]
        private SpriteEffects spriteEffects = SpriteEffects.None;

        // Properties
        public override BoundingBox Bounds
        {
            get
            {
                return default;
            }
        }

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

        public SpriteEffects SpriteEffects
        {
            get { return spriteEffects; }
            set { spriteEffects = value; }
        }

        // Methods
        public override void OnDraw(Camera camera)
        {
            // Check for no sprite
            if (sprite == null || sprite.Texture == null)
                return;

            // Check for batch
            if (camera.spriteBatch == null)
                return;

            // Create sprite matrix - convert from sprite coords to screen
            Matrix spriteMatrix = Matrix.CreateScale(sprite.Units)
                * Matrix.CreateTranslation(Game.RenderWidth / 2, Game.RenderHeight / 2, 0f);

            // Get matrix
            Matrix worldMatrix = Transform.LocalToWorldMatrix * spriteMatrix;

            // Extract draw matrix
            Vector2 drawPosition, drawScale;
            float drawRotation;
            worldMatrix.Decompose(out drawScale, out drawRotation, out drawPosition);

            // Draw call
            camera.spriteBatch.Draw(sprite.Texture,
                drawPosition,
                sprite.SourceRect,
                color,
                drawRotation,
                sprite.SourcePivot,
                drawScale * Sprite.InverseUnits,
                spriteEffects,
                drawOrder);
        }
    }
}
