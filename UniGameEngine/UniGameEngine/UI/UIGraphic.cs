using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UniGameEngine.Graphics;

namespace UniGameEngine.UI
{
    public abstract class UIGraphic : UIComponent, IGameDraw
    {
        // Private
        private static Texture2D white = null;

        // Properties
        public Texture2D White
        {
            get
            {
                if(white == null)
                {
                    white = new Texture2D(Game.GraphicsDevice, 1, 1);
                    white.SetData(new Color[] { Color.White });
                }
                return white;
            }
        }

        int IGameDraw.DrawOrder => 0;

        // Methods
        protected abstract void DrawGraphic(SpriteBatch spriteBatch, Rectangle rect);

        void IGameDraw.OnDraw(Camera camera)
        {
            // Check for no canvas or disabled canvas
            if (Canvas == null || Canvas.UIBatch == null)
                return;

            // Get the batch
            SpriteBatch uiBatch = Canvas.UIBatch;

            // Get the rect
            Rectangle bounds = Rect;

            // Check for invalid
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            // Send draw call
            DrawGraphic(uiBatch, bounds);
        }
    }
}
