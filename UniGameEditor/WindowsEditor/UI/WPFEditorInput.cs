using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorInput : EditorInput
    {
        // Internal
        internal TextBox textBox = null;

        // Properties
        public override float Width
        {
            get => (float)textBox.Width;
            set => textBox.Width = value;
        }
        public override float Height
        {
            get => (float)textBox.Height;
            set => textBox.Height = value;
        }

        public override string Text
        {
            get => textBox.Text;
            set => textBox.Text = value;
        }

        // Constructor
        public WPFEditorInput(Panel parent, string text)
        {
            textBox = new TextBox();
            textBox.Text = text;

            textBox.FontSize = DefaultFontSize;
            textBox.MinHeight = DefaultLineHeight;

            parent.Children.Add(textBox);
        }

        public WPFEditorInput(ItemsControl parent, string text)
        {
            textBox = new TextBox();
            textBox.Text = text;

            textBox.FontSize = DefaultFontSize;
            textBox.MinHeight = DefaultLineHeight;

            parent.Items.Add(textBox);
        }
    }
}
