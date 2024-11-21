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
        internal IconTextContent content = null;

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

        public override string Text
        {
            get => content.Text;
            set => content.Text = value;
        }

        public override string Tooltip
        {
            get => content.Tooltip;
            set => content.Tooltip = value;
        }

        public override EditorIcon Icon
        {
            get => content.Icon;
            set => content.Icon = value;
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

        // Constructor
        public WPFEditorButton(Panel parent, string text)
        {
            button = new Button();
            content = new IconTextContent(parent, button, text);
            dragDrop = new WPFDragDrop(content.mainControl);
        }

        public WPFEditorButton(ItemsControl parent, string text)
        {
            button = new Button();
            content = new IconTextContent(parent, button, text);
            dragDrop = new WPFDragDrop(content.mainControl);
        }

        // Methods
        public override void Perform()
        {
            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}
