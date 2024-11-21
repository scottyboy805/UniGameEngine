using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModernWpf.Controls;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorRenderView : EditorRenderView
    {
        // Type
        private sealed class WPFInteropHostView : WpfGame
        {
            // Private
            private Game gameHost = null;
            private Action initializeCall;
            private Action loadCall;
            private Action<GameTime> updateCall;
            private Action<GameTime> drawCall;

            // Internal
            internal IGraphicsDeviceService graphicsDeviceManager;
            internal WpfKeyboard keyboard = null;
            internal WpfMouse mouse = null;

            // Constructor
            public WPFInteropHostView(Game gameHost)
            {
                this.gameHost = gameHost;

                Type gameHostType = gameHost.GetType();
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                // Get delegates
                initializeCall = gameHostType.GetMethod(nameof(Initialize), flags).CreateDelegate<Action>(gameHost);
                loadCall = gameHostType.GetMethod(nameof(LoadContent), flags).CreateDelegate<Action>(gameHost);
                updateCall = gameHostType.GetMethod(nameof(Update), flags).CreateDelegate<Action<GameTime>>(gameHost);
                drawCall = gameHostType.GetMethod(nameof(Draw), flags).CreateDelegate<Action<GameTime>>(gameHost);
            }

            // Methods
            protected override void Initialize()
            {
                // Initialize device manager
                graphicsDeviceManager = new WpfGraphicsDeviceService(this);                
                keyboard = new WpfKeyboard(this);
                mouse = new WpfMouse(this);

                // Add graphics
                gameHost.Services.RemoveService(typeof(IGraphicsDeviceService));
                gameHost.Services.AddService(graphicsDeviceManager);
                
                base.Initialize();

                // Call initialize
                initializeCall();
            }

            protected override void LoadContent()
            {
                // Call load
                loadCall();
            }

            protected override void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                keyboard.GetState();
                mouse.GetState();

                // Call update
                updateCall(gameTime);
            }

            protected override void Draw(GameTime gameTime)
            {
                GraphicsDevice.Clear(new Color(0.1f, 0.1f, 0.1f, 1f));

                // Call draw
                drawCall(gameTime);
            }
        }

        // Private
        internal WPFDragDrop dragDrop = null;
        private WPFInteropHostView renderView = null;

        public override float Width
        {
            get => (float)renderView.ActualWidth;
            set => renderView.Width = value;
        }
        public override float Height
        {
            get => (float)renderView.ActualHeight;
            set => renderView.Height = value;
        }

        public override IDragHandler DragHandler
        {
            get => dragDrop.DragHandler;
            set => dragDrop.DragHandler = value;
        }

        public override IDropHandler DropHandler
        {
            get => dragDrop.DropHandler;
            set => dragDrop.DropHandler = value;
        }

        public override EditorMenu ContextMenu
        {
            get => contextMenu;
            set
            {
                contextMenu = value;
                renderView.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorRenderView(Panel parent, Game gameHost)
        {
            renderView = new WPFInteropHostView(gameHost);
            dragDrop = new WPFDragDrop(renderView);
            //renderView.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            //renderView.MinHeight = 150;
            parent.Children.Add(renderView);
        }

        public WPFEditorRenderView(ItemsControl parent, Game gameHost)
        {
            renderView = new WPFInteropHostView(gameHost);
            dragDrop = new WPFDragDrop(renderView);
            parent.Items.Add(renderView);
        }

        public override void Render()
        {
            //renderView.TargetElapsedTime
        }
    }
}
