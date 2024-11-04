using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorLabel : EditorLabel
    {
        // Internal
        private Label label = null;

        // Properties
        public override float Width
        {
            get => (float)label.Width;
            set => label.Width = value;
        }
        public override float Height
        {
            get => (int)label.Height;
            set => label.Height = value;
        }

        public override string Text
        {
            get => (string)label.Content;
            set => label.Content = value;
        }

        // Constructor
        public WPFEditorLabel(Panel parent, string text)
        {
            label = new Label();
            label.Content = text;

            parent.Children.Add(label);
        }
    }
}
