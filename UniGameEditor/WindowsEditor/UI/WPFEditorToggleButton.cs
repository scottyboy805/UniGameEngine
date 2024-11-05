using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorToggleButton : EditorToggleButton
    {
        // Internal
        internal ToggleButton toggleButton = null;

        // Properties
        public override float Width
        {
            get => (float)toggleButton.Width;
            set => toggleButton.Width = value;
        }
        public override float Height
        {
            get => (int)toggleButton.Height;
            set => toggleButton.Height = value;
        }

        public override string Text
        {
            get => (string)toggleButton.Content;
            set => toggleButton.Content = value;
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
            toggleButton.Content = text;
            toggleButton.IsChecked = on;

            toggleButton.FontSize = DefaultFontSize;
            toggleButton.MinHeight = DefaultLineHeight;

            parent.Children.Add(toggleButton);
        }

        public WPFEditorToggleButton(ItemsControl parent, string text, bool on)
        {
            toggleButton = new ToggleButton();
            toggleButton.Content = text;
            toggleButton.IsChecked = on;

            toggleButton.FontSize = DefaultFontSize;
            toggleButton.MinHeight = DefaultLineHeight;

            parent.Items.Add(toggleButton);
        }
    }
}
