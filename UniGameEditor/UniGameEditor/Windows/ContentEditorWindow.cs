using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class ContentEditorWindow : EditorWindow
    {
        // Private
        private EditorTreeView contentTreeView = null;

        // Constructor
        public ContentEditorWindow()
        {
            title = "Content";
        }

        // Methods
        protected internal override void OnShow()
        {
            // Add tree view
            contentTreeView = rootControl.AddTreeView();

            // Refresh content
            RefreshContentTree(Editor.ContentDirectory);
        }

        private void RefreshContentTree(string directory, EditorTreeNode parent = null)
        {
            // Get all directories
            foreach (string subDir in Directory.EnumerateDirectories(directory))
            {
                // Get folder name
                string folderName = Path.GetFileName(subDir);

                // Add folder node
                EditorTreeNode folderNode = parent != null
                    ? parent.AddNode(folderName)
                    : contentTreeView.AddNode(folderName);

                // Add children
                RefreshContentTree(subDir, folderNode);
            }

            // Get all files
            foreach (string subFile in Directory.EnumerateFiles(directory, "*.*"))
            {
                // Get file name
                string fileName = Path.GetFileName(subFile);

                // Add file node
                EditorTreeNode fileNode = parent != null
                    ? parent.AddNode(fileName)
                    : contentTreeView.AddNode(fileName);
            }
        }
    }
}
