using System.Windows.Controls;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorWrapPanelLayout : WPFEditorLayoutControl
    {
        // Internal
        internal WrapPanel wrapPanel = null;

        // Properties
        public override Panel Panel => wrapPanel;

        // Constructor
        public WPFEditorWrapPanelLayout(Panel parent)
        {
            wrapPanel = new WrapPanel();
            parent.Children.Add(wrapPanel);
        }

        public WPFEditorWrapPanelLayout(ItemsControl parent)
        {
            wrapPanel = new WrapPanel();
            parent.Items.Add(wrapPanel);
        }
    }
}
