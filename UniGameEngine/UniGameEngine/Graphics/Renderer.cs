using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace UniGameEngine.Graphics
{
    public abstract class Renderer : Component, IGameDraw
    {
        // Protected
        [DataMember(Name = "DrawOrder")]
        protected int drawOrder = 0;

        // Properties
        public abstract BoundingBox Bounds { get; }

        public int DrawOrder
        {
            get { return drawOrder; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return Game.GraphicsDevice; }
        }

        // Methods
        public abstract void OnDraw(Camera camera);
    }
}
