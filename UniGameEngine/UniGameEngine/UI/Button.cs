﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.Threading;
using UniGameEngine.Graphics;
using UniGameEngine.Scene;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace UniGameEngine.UI
{
    [DataContract]
    public class Button : Image
    {
        // Events
        [DataMember]
        public GameEvent OnClicked = new GameEvent();

        // Private
        [DataMember(Name = "HighlightColor")]
        private Color highlightColor = new Color(0.95f, 0.95f, 0.95f, 1f);
        [DataMember(Name = "PressedColor")]
        private Color pressedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        [DataMember(Name = "InactiveColor")]
        private Color inactiveColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        [DataMember(Name = "Interactable")]
        private bool interactable = true;

        // Properties
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

        // Constructor
        public Button()
        {
        }

        // Methods
        public virtual void Perform()
        {
            // Trigger event
            OnClicked.Raise();
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
            Perform();
        }

        public static Button Create(GameObject parent, string text = null)
        {
            // Create button object
            Button button = parent.CreateObject<Button>("Button");
            CreateDefaultButton(button, text);

            return button;
        }

        public static Button Create(GameScene scene, string text = null)
        {
            // Create button object
            Button button = scene.CreateObject<Button>("Button");
            CreateDefaultButton(button, text);            

            return button;
        }

        internal static void CreateDefaultButton(Button button, string text)
        {
            // Get sprite
            button.Size = new Vector2(160, 40);
            button.Sprite = new Sprite(button.Game.Content.Load<Texture2D>("UI/Default"),
                new Rectangle(2, 2, 190, 49));

            // Create label
            Label label = button.GameObject.CreateObject<Label>("Label");
            label.Text = text;
            label.Transform.LocalPosition = new Vector3(5, 3, 0);
            label.Size = new Vector2(150, 34);
            label.Raycast = false;
        }
    }
}
