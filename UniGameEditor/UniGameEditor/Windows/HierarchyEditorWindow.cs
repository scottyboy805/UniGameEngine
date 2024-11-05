
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
        protected internal override void OnShow()
        {
            // Create scene
            EditorLayoutControl hLayout = RootControl.AddHorizontalLayout();

            hLayout.AddLabel("My Scene Name");

            RootControl.AddLabel("Hello World");

            // Add tree view
            EditorTreeView tree = RootControl.AddTreeView();
            tree.AddNode("Test1").AddNode("Child1");
            tree.AddNode("Test2").AddNode("Child2");
        }
    }
}
