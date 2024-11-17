
using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;
using UniGameEngine.UI.Events;

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
        private UIEventDispatcher dispatcher = null;

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

        public UIEventDispatcher Dispatcher
        {
            get
            {
                if (dispatcher == null)
                    dispatcher = GameObject.GetComponentInParent<UIEventDispatcher>(true);

                return dispatcher;
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
        public bool GetTransform(out Vector2 position, out float rotation, out Vector2 scale)
        {
            // Get matrix
            Matrix worldMatrix = Transform.LocalToWorldMatrix;

            // Decompose
            return worldMatrix.Decompose(out scale, out rotation, out position);
        }

        public Vector2 GetAdjustedScale(Vector2 contentSize)
        {
            return new Vector2
            {
                X = (1f / contentSize.X) * size.X,
                Y = (1f / contentSize.Y) * size.Y
            };
        }

        protected override void RegisterSubSystems()
        {
            // Register for draw
            if (this is IGameDraw && Canvas != null)
                Canvas.uiDrawCalls.Add((IGameDraw)this);

            // Register for update
            if (this is IGameUpdate)
                Scene.sceneUpdateCalls.Add((IGameUpdate)this);

            // Register for UI events
            if (this is UIGraphic && Dispatcher != null)
                Dispatcher.AddRaycastTarget((UIGraphic)this);
        }
        protected override void UnregisterSubSystems()
        {
            // Unregister draw
            if (this is IGameDraw && Canvas != null)
                Canvas.uiDrawCalls.Remove((IGameDraw)this);

            // Unregister update
            if (this is IGameUpdate)
                Scene.sceneUpdateCalls.Remove((IGameUpdate)this);

            // Unregister UI event
            if (this is UIGraphic && Dispatcher != null)
                Dispatcher.AddRaycastTarget((UIGraphic)this);
        }
    }
}
