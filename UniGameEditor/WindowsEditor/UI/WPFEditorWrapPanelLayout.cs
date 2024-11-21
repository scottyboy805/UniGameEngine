using System.Windows.Controls;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorWrapPanelLayout : WPFEditorLayoutControl
    {
        // Constructor
        public WPFEditorWrapPanelLayout(Panel parent)
            : base(parent, new WrapPanel())
        {
        }

        public WPFEditorWrapPanelLayout(ItemsControl parent)
            : base(parent, new WrapPanel()) 
        {
        }
    }
}
