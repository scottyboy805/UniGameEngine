using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorButton : EditorButton
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal Button button = null;
        internal WPFEditorLayoutControl layout = null;

        // Properties
        public override float Width
        {
            get => (float)button.ActualWidth;
            set => button.Width = value;
        }

        public override float Height
        {
            get => (float)button.ActualHeight;
            set => button.Height = value;
        }

        public override IDragHandler DragHandler
        {
            get => dragDrop.DragHandler;
            set => dragDrop.DragHandler = value;
        }

        public override IDropHandler DropHandler
        {
            get => dragDrop.DropHandler;
            set => button.AllowDrop = value != null;
        }

        public override EditorMenu ContextMenu 
        { 
            get => contextMenu; 
            set
            {
                contextMenu = value;
                button.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        public override EditorLayoutControl Content
        {
            get { return layout; }
        }

        public override string Tooltip
        {
            get => (string)button.ToolTip;
            set => button.ToolTip = value;
        }

        // Constructor
        public WPFEditorButton(Panel parent)
        {
            button = new Button();
            InitializeButton();

            parent.Children.Add(button);
        }

        public WPFEditorButton(ItemsControl parent)
        {
            button = new Button();
            InitializeButton();

            parent.Items.Add(button);
        }

        // Methods
        private void InitializeButton()
        {
            dragDrop = new WPFDragDrop(button);
            layout = new WPFEditorLayoutControl((Panel)null, new StackPanel
            {
                Orientation = Orientation.Horizontal,
                MinHeight = DefaultLineHeight,
            });

            // Set content
            button.Content = layout.panel;
        }

        public override void Perform()
        {
            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}
