using System.Windows;
using System.Windows.Controls;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorStackLayout : WPFEditorLayoutControl
    {
        // Constructor
        public WPFEditorStackLayout(Panel parent, Orientation orientation)
            : base(parent, new StackPanel())
        {
            UpdateOrientation(orientation);
        }

        public WPFEditorStackLayout(ItemsControl parent, Orientation orientation)
            : base(parent, new StackPanel())
        {
            UpdateOrientation(orientation);
        }

        // Methods
        private void UpdateOrientation(Orientation orientation)
        {
            StackPanel stackPanel = (StackPanel)panel;
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
