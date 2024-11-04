
using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class HierarchyEditorWindow : EditorWindow
    {
        // Constructor
        public HierarchyEditorWindow()
        {
            title = "Hierarchy";
        }

        // Methods
        protected internal override void OnOpenWindow()
        {
            // Create scene
            EditorLayoutControl hLayout = RootControl.AddHorizontalLayout();

            hLayout.AddLabel("My Scene Name");

            RootControl.AddLabel("Hello World");
        }
    }
}
