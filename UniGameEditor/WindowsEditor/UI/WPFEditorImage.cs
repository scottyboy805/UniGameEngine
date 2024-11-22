using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorImage : EditorImage
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal Image image = null;
        internal EditorIcon icon = null;

        // Properties
        public override float Width
        {
            get => (float)image.ActualWidth;
            set => image.Width = value;
        }

        public override float Height
        {
            get => (float)image.ActualHeight;
            set => image.Height = value;
        }

        public override IDragHandler DragHandler
        {
            get => dragDrop.DragHandler;
            set => dragDrop.DragHandler = value;
        }

        public override IDropHandler DropHandler
        {
            get => dragDrop.DropHandler;
            set => image.AllowDrop = value != null;
        }

        public override EditorMenu ContextMenu
        {
            get => contextMenu;
            set
            {
                contextMenu = value;
                image.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        public override EditorIcon Icon
        {
            get => icon;
            set
            {
                // Check source
                image.Source = value is WPFEditorIcon
                    ? ((WPFEditorIcon)value).image
                    : null;
                image.IsEnabled = image.Source != null;
                icon = value;
            }
        }

        public override string Tooltip
        {
            get => (string)image.ToolTip;
            set => image.ToolTip = value;
        }

        // Constructor
        public WPFEditorImage(Panel parent, EditorIcon icon)
        {
            image = new Image();
            InitializeImage(icon);

            parent.Children.Add(image);
        }

        public WPFEditorImage(ItemsControl parent, EditorIcon icon)
        {
            image = new Image();
            InitializeImage(icon);

            parent.Items.Add(image);
        }

        // Methods
        private void InitializeImage(EditorIcon icon)
        {
            dragDrop = new WPFDragDrop(image);

            // Set content
            image.MaxHeight = DefaultLineHeight;
            Icon = icon;
        }
    }
}
