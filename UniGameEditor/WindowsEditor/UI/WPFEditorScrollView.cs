using System.Windows.Controls;
using System.Windows.Input;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorScrollView : WPFEditorLayoutControl
    {
        // Internal
        internal ScrollViewer scrollViewer = null;

        // Constructor
        public WPFEditorScrollView(Panel parent, bool horizontal, bool vertical)
            : base((Panel)null, new DockPanel())
        {
            ((DockPanel)panel).VerticalAlignment = System.Windows.VerticalAlignment.Top;

            scrollViewer = new ScrollViewer();
            scrollViewer.Content = panel;
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
            : base((ItemsControl)null, new DockPanel())
        {
            scrollViewer = new ScrollViewer();
            scrollViewer.Content = panel;
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
