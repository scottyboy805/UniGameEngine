using UniGameEditor.Content;
using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class ContentEditorWindow : EditorWindow
    {
        // Type
        private sealed class ContentDrop : IDropHandler
        {
            // Private
            private ContentDatabase contentDatabase = null;
            private string contentFolder = null;

            // Constructor
            public ContentDrop(ContentDatabase contentDatabase, string contentFolder)
            {
                this.contentDatabase = contentDatabase;
                this.contentFolder = contentFolder;

                // Format path correctly
                if (contentFolder == null || contentFolder == ".")
                {
                    this.contentFolder = "";
                }
                else
                {
                    if (contentFolder.EndsWith('/') == false)
                        this.contentFolder += '/';
                }
            }

            // Methods
            public bool CanDrop(DragDropType type, object dragData)
            {
                // Check for file drop
                return type == DragDropType.File;
            }

            public void PerformDrop(DragDropType type, object dragData)
            {
                // Get paths
                string[] paths = (string[])dragData;

                // Import the files
                foreach (string path in paths)
                    contentDatabase.ImportExternalContent(path, contentFolder + Path.GetFileName(path));
            }
        }

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

            // Add listener
            Editor.OnProjectLoaded += OnProjectLoaded;

            // Refresh content
            RefreshContentTree();
        }

        protected internal override void OnHide()
        {
            // Remove listener
            Editor.OnProjectLoaded -= OnProjectLoaded;

            if(Editor.ContentDatabase != null)
                Editor.ContentDatabase.OnContentModified -= RefreshContentTree;
        }

        private void OnProjectLoaded()
        {
            RefreshContentTree();
            Editor.ContentDatabase.OnContentModified += RefreshContentTree;
        }

        private void RefreshContentTree()
        {
            // Clear old data
            contentTreeView.ClearNodes();

            // Check for project open
            if (Editor.IsProjectOpen == true)
            {
                // Refresh content
                RefreshContentTree(Editor.ContentDirectory);
            }
        }

        private void RefreshContentTree(string directory, EditorTreeNode parent = null)
        {
            // Get folder name
            string rootName = Path.GetFileName(directory);

            // Get relative path
            string relativePath = Editor.ContentDatabase.GetContentRelativePath(directory);
            string contentRelativePath = string.IsNullOrEmpty(relativePath) == true || relativePath == "."
                ? "Content"
                : "Content/" + relativePath;

            // Create directory
            EditorTreeNode rootNode = parent != null
                ? parent.AddNode(rootName)
                : contentTreeView.AddNode(rootName);

            // Make folder
            rootNode.Icon = folderNormalIcon;

            // Add listeners
            rootNode.OnSelected += (EditorTreeNode node) => Editor.Selection.Select(new FolderObject(contentRelativePath));
            rootNode.OnExpanded += OnTreeNodeExpanded;

            // Add drop handler
            rootNode.DropHandler = new ContentDrop(Editor.ContentDatabase, relativePath);

            // Get all directories
            foreach (string subDir in Editor.ContentDatabase.SearchFolders(relativePath))
            {
                // Add children
                RefreshContentTree(subDir, rootNode);
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
