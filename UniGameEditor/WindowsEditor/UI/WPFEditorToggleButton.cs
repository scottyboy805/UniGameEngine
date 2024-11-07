using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorToggleButton : EditorToggleButton
    {
        // Internal
        internal ToggleButton toggleButton = null;
        internal IconTextContent content = null;

        // Properties
        public override float Width
        {
            get => (float)toggleButton.Width;
            set => toggleButton.Width = value;
        }
        public override float Height
        {
            get => (float)toggleButton.Height;
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

        // Constructor
        public WPFEditorToggleButton(Panel parent, string text, bool on)
        {
            toggleButton = new ToggleButton();
            toggleButton.IsChecked = on;
            content = new IconTextContent(parent, toggleButton, text);
        }

        public WPFEditorToggleButton(ItemsControl parent, string text, bool on)
        {
            toggleButton = new ToggleButton();
            toggleButton.IsChecked = on;
            content = new IconTextContent(parent, toggleButton, text);
        }
    }
}
