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

        // Constructor
        public WPFEditorButton(Panel parent, string text)
        {
            button = new Button();
            content = new IconTextContent(parent, button, text);
        }

        public WPFEditorButton(ItemsControl parent, string text)
        {
            button = new Button();
            content = new IconTextContent(parent, button, text);
        }

        // Methods
        public override void Perform()
        {
            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}
