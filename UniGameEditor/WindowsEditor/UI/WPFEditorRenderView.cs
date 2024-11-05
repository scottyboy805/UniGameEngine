using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed  class WPFEditorRenderView : EditorRenderView
    {
        // Type
        private sealed class WPFInteropHostView : WpfGame
        {
            // Private
            private Action onRender = null;

            // Internal
            internal IGraphicsDeviceService graphicsDeviceManager;
            internal WpfKeyboard keyboard = null;
            internal WpfMouse mouse = null;

            // Constructor
            public WPFInteropHostView(Action onRender)
            {
                this.onRender = onRender;
            }

            // Methods
            protected override void Initialize()
            {
                

                // Initialize device manager
                graphicsDeviceManager = new WpfGraphicsDeviceService(this);
                keyboard = new WpfKeyboard(this);
                mouse = new WpfMouse(this);

                base.Initialize();
            }

            protected override void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                keyboard.GetState();
                mouse.GetState();
            }

            protected override void Draw(GameTime gameTime)
            {
                GraphicsDevice.Clear(new Color(0.1f, 0.1f, 0.1f, 1f));

                // Call render
                if (onRender != null)
                    onRender();
            }
        }

        // Private
        private WPFInteropHostView renderView = null;

        public override float Width
        {
            get => (float)renderView.Width;
            set => renderView.Width = value;
        }
        public override float Height
        {
            get => (int)renderView.Height;
            set => renderView.Height = value;
        }

        // Constructor
        public WPFEditorRenderView(Panel parent, Action onRender)
        {
            renderView = new WPFInteropHostView(onRender);
            parent.Children.Add(renderView);
        }

        public WPFEditorRenderView(ItemsControl parent, Action onRender)
        {
            renderView = new WPFInteropHostView(onRender);
            parent.Items.Add(renderView);
        }

        public override void Render()
        {
            //renderView.TargetElapsedTime
        }
    }
}
