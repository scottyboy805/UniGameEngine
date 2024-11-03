
using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;

namespace UniGameEngine.UI
{
    public enum UIAnchor
    {
        Center,
        TopLeft,
        TopRight,
        TopCenter,
        BottomLeft,
        BottomRight,
        BottomCenter,
        CenterLeft,
        CenterRight,
    };

    public abstract class UIComponent : Component
    {
        // Private
        private UICanvas canvas = null;

        [DataMember(Name = "Size")]
        private Vector2 size = new Vector2(100f, 100f);
        [DataMember(Name = "Pivot")]
        private Vector2 pivot = new Vector2(0.5f, 0.5f);
        [DataMember(Name = "Anchor")]
        private UIAnchor anchor = UIAnchor.Center;

        // Properties
        public UICanvas Canvas
        {
            get
            {
                if(canvas == null)
                    canvas = GameObject.GetComponentInParent<UICanvas>(true);

                return canvas;
            }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Vector2 Pivot
        {
            get { return pivot; }
            set
            {
                pivot = value;
                pivot.X = Math.Clamp(pivot.X, 0f, 1f);
                pivot.Y = Math.Clamp(pivot.Y, 0f, 1f);
            }
        }

        public UIAnchor Anchor
        {
            get { return anchor; }
            set { anchor = value; }
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle(
                    (int)Transform.WorldPosition.X,
                    (int)Transform.WorldPosition.Y,
                    (int)Size.X, (int)Size.Y);
            }
        }
        
        // Methods
        protected override void RegisterSubSystems()
        {
            // Register for draw
            if (this is IGameDraw && Canvas != null)
                Canvas.uiDrawCalls.Add((IGameDraw)this);

            // Register for update
            if (this is IGameUpdate)
                Scene.sceneUpdateCalls.Add((IGameUpdate)this);
        }
        protected override void UnregisterSubSystems()
        {
            // Unregister draw
            if (this is IGameDraw && Canvas != null)
                Canvas.uiDrawCalls.Remove((IGameDraw)this);

            // Unregister update
            if (this is IGameUpdate)
                Scene.sceneUpdateCalls.Remove((IGameUpdate)this);
        }
    }
}
