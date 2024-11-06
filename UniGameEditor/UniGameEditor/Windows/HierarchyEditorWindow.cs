
using System.ComponentModel.Design;
using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class HierarchyEditorWindow : EditorWindow
    {
        // Constructor
        public HierarchyEditorWindow()
        {
            icon = EditorIcon.FindIcon("Hierarchy");
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
            tree.AddNode("Test1").AddNode("Child1").Icon = EditorIcon.FindIcon("FolderNormal");
            tree.AddNode("Test2").AddNode("Child2").Icon = EditorIcon.FindIcon("FolderOpen");
        }
    }
}
