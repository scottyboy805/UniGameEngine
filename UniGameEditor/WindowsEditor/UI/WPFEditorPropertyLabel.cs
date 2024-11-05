using System.Windows;
using System.Windows.Controls;
using UniGameEditor;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorPropertyLabel : EditorPropertyLabel
    {
        // Internal
        internal Label label = null;

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
        public WPFEditorPropertyLabel(Panel parent, SerializedProperty property, string text)
            : base(property)
        {
            label = new Label();
            label.Content = text;
            label.VerticalAlignment = VerticalAlignment.Center;

            label.FontSize = DefaultFontSize;
            label.MinHeight = DefaultLineHeight;

            parent.Children.Add(label);
        }

        public WPFEditorPropertyLabel(ItemsControl parent, SerializedProperty property, string text)
            : base(property)
        {
            label = new Label();
            label.Content = text;
            label.VerticalAlignment = VerticalAlignment.Center;

            label.FontSize = DefaultFontSize;
            label.MinHeight = DefaultLineHeight;

            parent.Items.Add(label);
        }
    }
}
