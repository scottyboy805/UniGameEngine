using ModernWpf.Controls;
using System.Windows;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorToggle : EditorToggle
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal CheckBox checkBox = null;

        // Properties
        public override float Width
        {
            get => (float)checkBox.ActualWidth;
            set => checkBox.Width = value;
        }
        public override float Height
        {
            get => (float)checkBox.ActualHeight;
            set => checkBox.Height = value;
        }

        public override string Text
        {
            get => (string)checkBox.Content;
            set => checkBox.Content = value;
        }
        public override bool IsChecked
        {
            get => (bool)checkBox.IsChecked;
            set => checkBox.IsChecked = value;
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
                checkBox.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorToggle(Panel parent, string text, bool on)
        {
            checkBox = new CheckBox();
            dragDrop = new WPFDragDrop(checkBox);
            checkBox.Content = text;
            checkBox.IsChecked = on;

            checkBox.FontSize = DefaultFontSize;
            checkBox.MinHeight = DefaultLineHeight;

            parent.Children.Add(checkBox);
        }

        public WPFEditorToggle(ItemsControl parent, string text, bool on)
        {
            checkBox = new CheckBox();
            dragDrop = new WPFDragDrop(checkBox);
            checkBox.Content = text;
            checkBox.IsChecked = on;

            checkBox.FontSize = DefaultFontSize;
            checkBox.MinHeight = DefaultLineHeight;

            parent.Items.Add(checkBox);
        }
    }
}
