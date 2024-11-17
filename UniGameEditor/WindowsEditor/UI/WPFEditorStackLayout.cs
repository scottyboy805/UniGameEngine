using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using System.Windows;
using System.Windows.Controls;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorStackLayout : WPFEditorLayoutControl
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal StackPanel stackPanel = null;

        // Properties
        public override Panel Panel => stackPanel;

        // Constructor
        public WPFEditorStackLayout(Panel parent, Orientation orientation)
        {
            stackPanel = new StackPanel();
            dragDrop = new WPFDragDrop(stackPanel);
            UpdateOrientation(orientation);

            parent.Children.Add(stackPanel);
        }

        public WPFEditorStackLayout(ItemsControl parent, Orientation orientation)
        {
            stackPanel = new StackPanel();
            dragDrop = new WPFDragDrop(stackPanel);
            UpdateOrientation(orientation);

            parent.Items.Add(stackPanel);
        }

        // Methods
        private void UpdateOrientation(Orientation orientation)
        {
            stackPanel.Orientation = orientation;

            // Check direction
            if (orientation == Orientation.Horizontal)
            {
                stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                stackPanel.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                stackPanel.VerticalAlignment = VerticalAlignment.Stretch;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }
    }
}
