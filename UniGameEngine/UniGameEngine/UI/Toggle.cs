using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using UniGameEngine.Graphics;
using UniGameEngine.Scene;
using static System.Formats.Asn1.AsnWriter;

namespace UniGameEngine.UI
{
    public class Toggle : Image
    {
        // Events
        public GameEvent<bool> OnToggled = new GameEvent<bool>();

        // Private
        [DataMember(Name = "ToggleGraphic")]
        private UIGraphic toggleGraphic;
        [DataMember(Name = "HighlightColor")]
        private Color highlightColor = new Color(0.95f, 0.95f, 0.95f, 1f);
        [DataMember(Name = "PressedColor")]
        private Color pressedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        [DataMember(Name = "InactiveColor")]
        private Color inactiveColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        [DataMember(Name = "Interactable")]
        private bool interactable = true;
        [DataMember(Name = "On")]
        private bool on = true;

        // Properties
        public UIGraphic ToggleGraphic
        {
            get { return toggleGraphic; }
            set { toggleGraphic = value; }
        }

        public Color HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        public Color PressedColor
        {
            get { return pressedColor; }
            set { pressedColor = value; }
        }

        public Color InactiveColor
        {
            get { return inactiveColor; }
            set { inactiveColor = value; }
        }

        public bool Interactable
        {
            get { return interactable; }
            set { interactable = value; }
        }

        public bool On
        {
            get { return on; }
        }

        // Constructor
        public Toggle()
        {
        }

        // Methods
        public virtual void PerformToggle(bool sendEvent = true)
        {
            PerformToggle(!On, sendEvent);
        }

        public virtual void PerformToggle(bool on, bool sendEvent = true)
        {
            this.on = on;

            // Update graphic
            if (toggleGraphic != null)
                toggleGraphic.GameObject.Enabled = on;

            // Send event
            if (sendEvent == true)
                OnToggled.Raise(on);
        }

        protected override void DrawGraphic(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, Vector2 pivot)
        {
            // Get draw color
            Color drawColor = Color;

            // Check for inactive
            if (interactable == false)
            {
                drawColor = inactiveColor;
            }
            // Check for press
            else if (IsPressed == true)
            {
                drawColor = pressedColor;
            }
            // Check for over
            else if (IsPointerOver == true)
            {
                drawColor = highlightColor;
            }

            // Draw button
            DrawGraphic(spriteBatch, position, rotation, scale, pivot, drawColor);
        }

        public override void OnPressEnd()
        {
            base.OnPressEnd();
            PerformToggle();
        }

        public static Toggle Create(GameObject parent)
        {
            // Create toggle object
            Toggle toggle = parent.CreateObject<Toggle>("Toggle");
            CreateDefaultToggle(toggle);

            return toggle;
        }

        public static Toggle Create(GameScene scene)
        {
            // Create toggle object
            Toggle toggle = scene.CreateObject<Toggle>("Toggle");
            CreateDefaultToggle(toggle);            

            return toggle;
        }

        internal static void CreateDefaultToggle(Toggle toggle)
        {
            // Get sprite
            toggle.Size = new Vector2(40, 40);
            toggle.Sprite = new Sprite(toggle.Game.Content.Load<Texture2D>("UI/Default"),
                new Rectangle(2, 104, 36, 36));

            // Create toggle mark
            Image image = toggle.GameObject.CreateObject<Image>("On");            
            toggle.toggleGraphic = image;

            // Get checkmark sprite
            image.Transform.LocalPosition = new Vector3(6, 6, 0);
            image.Size = new Vector2(30, 30);
            image.Sprite = new Sprite(toggle.Game.Content.Load<Texture2D>("UI/Default"),
                new Rectangle(220, 103, 23, 23));

            // Disable raycast
            image.Raycast = false;
        }
    }
}
