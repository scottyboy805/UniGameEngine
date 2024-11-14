using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace UniGameEngine.Graphics
{
    [DataContract]
    public sealed class Sprite
    {
        // Private
        private const float defaultUnits = 100f;

        [DataMember(Name = "Texture")]
        private Texture2D texture = null;
        [DataMember(Name = "SourcePositionNormalized")]
        private Vector2 sourcePositionNormalized = new Vector2(0f, 0f);
        [DataMember(Name = "SourceSizeNormalized")]
        private Vector2 sourceSizeNormalized = new Vector2(1f, 1f);
        [DataMember(Name = "PivotNormalized")]
        private Vector2 pivotNormalized = new Vector2(0.5f, 0.5f);
        [DataMember(Name = "Units")]
        private float units = defaultUnits;

        private Rectangle sourceRect = default;
        private Vector2 sourcePivot = default;
        private float inverseUnits = 1f / defaultUnits;

        // Properties        
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Vector2 SourcePositionNormalized
        {
            get { return sourcePositionNormalized; }
        }

        public Vector2 SourceSizeNormalized
        {
            get { return sourceSizeNormalized; }
        }

        public Vector2 PivotNormalized
        {
            get { return pivotNormalized; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
        }

        public Vector2 SourcePivot
        {
            get { return sourcePivot; }
        }

        public int Width
        {
            get { return sourceRect.Width; }
        }

        public int Height
        {
            get { return sourceRect.Height; }
        }

        public float Units
        {
            get { return units; }
            set
            {
                units = value;
                inverseUnits = 1f / units;
            }
        }

        internal float InverseUnits
        {
            get { return inverseUnits; }
        }

        // Constructor
        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            RebuildSprite();
        }

        public Sprite(Texture2D texture, Rectangle sourceRect)
        {
            this.texture = texture;

            // Check for null
            if(texture != null)
            {
                this.sourcePositionNormalized = new Vector2
                {
                    X = (1f / texture.Width) * sourceRect.X,
                    Y = (1f / texture.Height) * sourceRect.Y,
                };
                this.sourceSizeNormalized = new Vector2
                {
                    X = (1f / texture.Width) * sourceRect.Width,
                    Y = (1f / texture.Height) * sourceRect.Height,
                };
            }
            
            // Rebuild the sprite
            RebuildSprite();
        }

        // Methods
        private void RebuildSprite()
        {
            // Check for texture
            if(texture == null)
            {
                sourceRect = default;
                sourcePivot = default;
                return;
            }

            // Update source rect
            sourceRect = new Rectangle
            {
                X = (int)(texture.Width * sourcePositionNormalized.X),
                Y = (int)(texture.Height * sourcePositionNormalized.Y),
                Width = (int)(texture.Width * sourceSizeNormalized.X),
                Height = (int)(texture.Height * sourceSizeNormalized .Y),
            };

            // Update source pivot
            sourcePivot = new Vector2
            {
                X = (int)(sourceRect.Width * pivotNormalized.X),
                Y = (int)(sourceRect.Height * pivotNormalized.Y)
            };
        }

        public static implicit operator Sprite(Texture2D texture)
        {
            return new Sprite(texture);
        }
    }
}
