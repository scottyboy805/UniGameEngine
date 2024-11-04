
using System.Windows;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorStackLayout : EditorLayoutControl
    {
        // Internal
        private StackPanel stackPanel = null;

        // Properties
        public override float Width
        {
            get => (float)stackPanel.Width;
            set => stackPanel.Width = value;
        }
        public override float Height
        {
            get => (int)stackPanel.Height;
            set => stackPanel.Height = value;
        }

        // Constructor
        public WPFEditorStackLayout(Panel parent, Orientation orientation)
        {
            stackPanel = new StackPanel();
            stackPanel.Orientation = orientation;
            parent.Children.Add(stackPanel);
        }

        // Methods
        public override EditorLabel AddLabel(string text)
        {
            return new WPFEditorLabel(stackPanel, text);
        }

        public override EditorRenderView AddRenderView(Action OnRender)
        {
            return new WPFEditorRenderView(stackPanel, OnRender);
        }

        public override EditorLayoutControl AddHorizontalLayout()
        {
            return new WPFEditorStackLayout(stackPanel, Orientation.Horizontal);
        }

        public override EditorLayoutControl AddVerticalLayout()
        {
            return new WPFEditorStackLayout(stackPanel, Orientation.Vertical);
        }
    }
}
