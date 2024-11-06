using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class ContentEditorWindow : EditorWindow
    {
        // Private
        private EditorTreeView contentTreeView = null;
        private EditorIcon folderNormalIcon = null;
        private EditorIcon folderOpenIcon = null;

        // Constructor
        public ContentEditorWindow()
        {
            icon = EditorIcon.FindIcon("Content");
            title = "Content";
        }

        // Methods
        protected internal override void OnShow()
        {
            // Load icons
            folderNormalIcon = EditorIcon.FindIcon("FolderNormal");
            folderOpenIcon = EditorIcon.FindIcon("FolderOpen");


            // Add scroll 
            EditorLayoutControl scrollLayout = RootControl.AddScrollLayout();

            // Add tree view
            contentTreeView = scrollLayout.AddTreeView();

            // Refresh content
            RefreshContentTree(Editor.ContentDirectory);
        }

        private void RefreshContentTree(string directory, EditorTreeNode parent = null)
        {
            // Get folder name
            string rootName = Path.GetFileName(directory);

            // Create directory
            EditorTreeNode rootNode = parent != null
                ? parent.AddNode(rootName)
                : contentTreeView.AddNode(rootName);

            // Make folder
            rootNode.Icon = folderNormalIcon;

            // Add listeners
            rootNode.OnExpanded += OnTreeNodeExpanded;

            // Get all directories
            foreach (string subDir in Directory.EnumerateDirectories(directory))
            {
                // Add children
                RefreshContentTree(subDir, rootNode);
            }

            // Get all files
            foreach (string subFile in Directory.EnumerateFiles(directory, "*.*"))
            {
                // Get file name
                string fileName = Path.GetFileName(subFile);

                // Add file node
                EditorTreeNode fileNode = rootNode.AddNode(fileName);
            }
        }

        private void OnTreeNodeExpanded(EditorTreeNode node, bool expanded)
        {
            node.Icon = expanded == false
                ? folderNormalIcon 
                : folderOpenIcon;
        }
    }
}
