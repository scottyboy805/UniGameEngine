using ModernWpf.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorToggleButton : EditorToggleButton
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal ToggleButton toggleButton = null;
        internal IconTextContent content = null;

        // Properties
        public override float Width
        {
            get => (float)toggleButton.ActualWidth;
            set => toggleButton.Width = value;
        }
        public override float Height
        {
            get => (float)toggleButton.ActualHeight;
            set => toggleButton.Height = value;
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

        public override bool IsChecked
        {
            get => (bool)toggleButton.IsChecked;
            set => toggleButton.IsChecked = value;
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

        // Constructor
        public WPFEditorToggleButton(Panel parent, string text, bool on)
        {
            toggleButton = new ToggleButton();
            dragDrop = new WPFDragDrop(toggleButton);
            toggleButton.IsChecked = on;
            content = new IconTextContent(parent, toggleButton, text);
        }

        public WPFEditorToggleButton(ItemsControl parent, string text, bool on)
        {
            toggleButton = new ToggleButton();
            dragDrop = new WPFDragDrop(toggleButton);
            toggleButton.IsChecked = on;
            content = new IconTextContent(parent, toggleButton, text);
        }
    }
}
