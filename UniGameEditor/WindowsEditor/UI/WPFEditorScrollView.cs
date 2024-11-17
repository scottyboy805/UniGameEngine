using System.Windows.Controls;
using System.Windows.Input;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorScrollView : WPFEditorLayoutControl
    {
        // Internal
        internal ScrollViewer scrollViewer = null;
        internal StackPanel scrollPanel = null;

        // Properties
        public override Panel Panel => scrollPanel;

        // Constructor
        public WPFEditorScrollView(Panel parent, bool horizontal, bool vertical)
        {
            scrollViewer = new ScrollViewer();
            dragDrop = new WPFDragDrop(scrollViewer);
            scrollViewer.Content = scrollPanel = new StackPanel();
            scrollViewer.HorizontalScrollBarVisibility = horizontal == true
                ? ScrollBarVisibility.Auto
                : ScrollBarVisibility.Hidden;
            scrollViewer.VerticalScrollBarVisibility = vertical == true
                ? ScrollBarVisibility.Auto
                : ScrollBarVisibility.Hidden;

            // Add handler
            scrollViewer.PreviewMouseWheel += PreviewMouseWheel;

            parent.Children.Add(scrollViewer);
        }

        public WPFEditorScrollView(ItemsControl parent, bool horizontal, bool vertical)
        {
            scrollViewer = new ScrollViewer();
            dragDrop = new WPFDragDrop(scrollViewer);
            scrollViewer.Content = scrollPanel = new StackPanel();
            scrollViewer.HorizontalScrollBarVisibility = horizontal == true
                ? ScrollBarVisibility.Auto
                : ScrollBarVisibility.Hidden;
            scrollViewer.VerticalScrollBarVisibility = vertical == true
                ? ScrollBarVisibility.Auto
                : ScrollBarVisibility.Hidden;

            // Add handler
            scrollViewer.PreviewMouseWheel += PreviewMouseWheel;

            parent.Items.Add(scrollViewer);
        }

        // Methods
        private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (e.Delta * 0.1f));
            e.Handled = true;
        }
    }
}
