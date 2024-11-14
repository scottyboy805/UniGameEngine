using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using UniGameEngine.Graphics;
using UniGameEngine.UI.Events;

namespace UniGameEngine.UI
{
    public abstract class UIGraphic : UIComponent, IGameDraw, IUIPointerEvent, IUIPressEvent
    {
        // Private
        private static Texture2D white = null;

        [DataMember(Name = "Raycast")]
        private bool raycast = true;

        private bool isPointerOver = false;
        private bool isPressed = false;

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

        public bool Raycast
        {
            get { return raycast; }
            set { raycast = value; }
        }

        public bool IsPointerOver
        {
            get { return isPointerOver; }
        }

        public bool IsPressed
        {
            get { return isPressed; }
        }

        int IGameDraw.DrawOrder => 0;

        // Methods
        protected abstract void DrawGraphic(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, Vector2 pivot);

        public bool PerformRaycast(in Point pointer)
        {
            // Get world position
            return Rect.Contains(pointer);
        }

        void IGameDraw.OnDraw(Camera camera)
        {
            // Check for no canvas or disabled canvas
            if (Canvas == null || Canvas.UIBatch == null)
                return;

            // Get the batch
            SpriteBatch uiBatch = Canvas.UIBatch;

            // Get the transform
            Vector2 position, scale;
            float rotation;
            GetTransform(out position, out rotation, out scale);

            // Check for zero
            if (scale.X <= 0f || scale.Y <= 0f)
                return;

            // Send draw call
            DrawGraphic(uiBatch, position, rotation, scale, Pivot);
        }

        public virtual void OnPointerEnter()
        {
            isPointerOver = true;
        }

        public virtual void OnPointerExit()
        {
            isPointerOver = false;
            isPressed = false;
        }

        public virtual void OnPressBegin()
        {
            isPressed = true;
        }

        public virtual void OnPressEnd()
        {
            isPressed = false;
        }
    }
}
