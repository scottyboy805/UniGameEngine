using System.Windows;
using System.Windows.Controls;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorStackLayout : WPFEditorLayoutControl
    {
        // Internal
        private StackPanel stackPanel = null;

        // Properties
        public override Panel Panel => stackPanel;

        // Constructor
        public WPFEditorStackLayout(Panel parent, Orientation orientation)
        {
            stackPanel = new StackPanel();
            UpdateOrientation(orientation);

            parent.Children.Add(stackPanel);
        }

        public WPFEditorStackLayout(ItemsControl parent, Orientation orientation)
        {
            stackPanel = new StackPanel();
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
            }
            else
            {
                stackPanel.VerticalAlignment = VerticalAlignment.Stretch;
            }
        }
    }
}
