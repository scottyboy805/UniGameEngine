using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UniGameEngine.Graphics;

namespace UniGameEngine.UI
{
    [DataContract]
    public sealed class UICanvas : Component, IGameDraw
    {
        // Internal
        internal HashSet<IGameDraw> uiDrawCalls = new HashSet<IGameDraw>();

        private SpriteBatch spriteBatch = null;
        private SamplerState spriteSampler = null;

        [DataMember(Name = "Camera")]
        private Camera camera = null;
        [DataMember(Name = "DrawOrder")]
        private int drawOrder = 1000;
        [DataMember(Name = "ReferenceSize")]
        private Vector2 referenceSize = new Vector2(1280, 720);

        // Properties
        public int DrawOrder
        {
            get { return drawOrder; }
            set { drawOrder = value; }
        }

        internal SpriteBatch UIBatch
        {
            get { return spriteBatch; }
        }

        // Constructor
        public UICanvas()
        {
            spriteSampler = new SamplerState
            { 
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                //Filter = TextureFilter.PointMipLinear,
            };
        }

        // Methods
        public void OnDraw(Camera camera)
        {
            // Check for specific camera
            if (this.camera != null && camera != this.camera)
                return;

            // Create scale size
            Matrix referenceMatrix = Matrix.CreateScale(
                (1f / referenceSize.X) * camera.RenderWidth,
                (1f / referenceSize.Y) * camera.RenderHeight,
                1f);

            // Begin batch
            spriteBatch.Begin(samplerState: spriteSampler, transformMatrix: referenceMatrix);
            {
                // Draw all elements
                foreach(IGameDraw drawCall in uiDrawCalls)
                {
                    // Draw the element
                    drawCall.OnDraw(camera);
                }
            }
            // End batch
            spriteBatch.End();
        }

        protected override void OnEnable()
        {
            // Create batch
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        protected override void OnDisable()
        {
            // Release batch
            spriteBatch.Dispose();
            spriteBatch = null;
        }
    }
}
