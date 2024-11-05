using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorButton : EditorButton
    {
        // Internal
        internal Button button = null;

        // Properties
        public override float Width
        {
            get => (float)button.Width;
            set => button.Width = value;
        }
        public override float Height
        {
            get => (int)button.Height;
            set => button.Height = value;
        }

        public override string Text
        {
            get => (string)button.Content;
            set => button.Content = value;
        }

        // Constructor
        public WPFEditorButton(Panel parent, string text)
        {
            button = new Button();
            button.Content = text;

            button.FontSize = DefaultFontSize;
            button.MinHeight = DefaultLineHeight;

            parent.Children.Add(button);
        }

        public WPFEditorButton(ItemsControl parent, string text)
        {
            button = new Button();
            button.Content = text;

            button.FontSize = DefaultFontSize;
            button.MinHeight = DefaultLineHeight;

            parent.Items.Add(button);
        }

        // Methods
        public override void Perform()
        {
            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}
