using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorToggle : EditorToggle
    {
        // Internal
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

        // Constructor
        public WPFEditorToggle(Panel parent, string text, bool on)
        {
            checkBox = new CheckBox();
            checkBox.Content = text;
            checkBox.IsChecked = on;

            checkBox.FontSize = DefaultFontSize;
            checkBox.MinHeight = DefaultLineHeight;

            parent.Children.Add(checkBox);
        }

        public WPFEditorToggle(ItemsControl parent, string text, bool on)
        {
            checkBox = new CheckBox();
            checkBox.Content = text;
            checkBox.IsChecked = on;

            checkBox.FontSize = DefaultFontSize;
            checkBox.MinHeight = DefaultLineHeight;

            parent.Items.Add(checkBox);
        }
    }
}
