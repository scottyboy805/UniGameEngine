using System.Windows;
using System.Windows.Controls;
using UniGameEditor;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorPropertyLabel : EditorPropertyLabel
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal Label label = null;

        // Properties
        public override float Width
        {
            get => (float)label.Width;
            set => label.Width = value;
        }

        public override float Height
        {
            get => (float)label.Height;
            set => label.Height = value;
        }

        public override string Text
        {
            get => (string)label.Content;
            set => label.Content = value;
        }

        public override string Tooltip
        {
            get => (string)label.ToolTip;
            set => label.ToolTip = value;
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
                label.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorPropertyLabel(Panel parent, SerializedProperty property, string text)
            : base(property)
        {
            label = new Label();
            InitializeLabel(text);

            parent.Children.Add(label);
        }

        public WPFEditorPropertyLabel(ItemsControl parent, SerializedProperty property, string text)
            : base(property)
        {
            label = new Label();
            InitializeLabel(text);

            parent.Items.Add(label);
        }

        // Methods
        private void InitializeLabel(string text)
        {
            dragDrop = new WPFDragDrop(label);

            // Set content
            label.Content = text;
            label.FontSize = DefaultFontSize;
            label.Height = DefaultLineHeight;
            label.VerticalContentAlignment = VerticalAlignment.Center;

            // Trigger initialized
            OnInitialized();
        }
    }
}
