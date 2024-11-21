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
        private EditorTreeView contentFolderTreeView = null;
        private EditorTreeView contentFileTreeView = null;
        private EditorIcon folderNormalIcon = null;
        private EditorIcon folderOpenIcon = null;
        private Dictionary<string, bool> foldersExpanded = new Dictionary<string, bool>();
        private string selectedFolder = null;

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

            // Create main split
            EditorSplitViewLayoutControl split = RootControl.AddHorizontalSplitLayout();

            // Add scroll 
            EditorLayoutControl scrollLayoutLeft = split.LayoutA.AddScrollLayout();// RootControl.AddScrollLayout();
            EditorLayoutControl scrollLayoutRight = split.LayoutB.AddScrollLayout();

            // Add tree view
            contentFolderTreeView = scrollLayoutLeft.AddTreeView();
            contentFileTreeView = scrollLayoutRight.AddTreeView();

            // Add listener
            Editor.OnProjectLoaded += OnProjectLoaded;

            // Refresh content
            RefreshContentFolderTree();
        }

        protected internal override void OnHide()
        {
            // Remove listener
            Editor.OnProjectLoaded -= OnProjectLoaded;

            if(Editor.ContentDatabase != null)
                Editor.ContentDatabase.OnContentModified -= RefreshContentFolderTree;
        }

        private void OnProjectLoaded()
        {
            RefreshContentFolderTree();
            RefreshContentFileTree();

            // Add listeners
            Editor.Selection.OnSelectionChanged += OnSelectionChanged;
            Editor.ContentDatabase.OnContentModified += RefreshContentFolderTree;
        }

        private void OnSelectionChanged()
        {
            // Get folder selection
            FolderObject folderObject = Editor.Selection.GetSelected<FolderObject>().FirstOrDefault();

            // Update search folder
            if (folderObject != null)
                selectedFolder = folderObject.ContentRelativePath;

            // Refresh files
            RefreshContentFileTree();
        }

        private void RefreshContentFolderTree()
        {
            // Clear old data
            contentFolderTreeView.ClearNodes();

            // Check for project open
            if (Editor.IsProjectOpen == true)
            {
                // Refresh content
                RefreshContentFolderTree(Editor.ContentDirectory);
            }
        }

        private void RefreshContentFileTree()
        {
            // Clear old data
            contentFileTreeView.ClearNodes();

            // Check for project open
            if(Editor.IsProjectOpen == true)
            {
                // Refresh content
                RefreshContentFileTree(selectedFolder);
            }
        }

        private void RefreshContentFolderTree(string directory, EditorTreeNode parent = null)
        {
            // Get folder name
            string rootName = Path.GetFileName(directory);

            // Get relative path
            string relativePath = Editor.ContentDatabase.GetContentRelativePath(directory);

            // Create directory
            EditorTreeNode rootNode = parent != null
                ? parent.AddNode(rootName)
                : contentFolderTreeView.AddNode(rootName);

            // Check expanded
            bool expanded;
            if (foldersExpanded.TryGetValue(relativePath, out expanded) == false)
                foldersExpanded[relativePath] = false;

            // Set expanded
            rootNode.IsExpanded = expanded;

            // Make folder
            rootNode.Icon = folderNormalIcon;

            // Set menu
            rootNode.ContextMenu = CreateContentFolderContextMenu(relativePath, parent == null);

            // Add listeners
            rootNode.OnSelected += (EditorTreeNode node) => Editor.Selection.Select(new FolderObject(relativePath));
            rootNode.OnExpanded += (EditorTreeNode node, bool expanded) =>
            {
                foldersExpanded[relativePath] = expanded;
                OnContentFolderTreeNodeExpanded(node, expanded);
            };

            // Add drop handler
            rootNode.DropHandler = new ContentDrop(Editor.ContentDatabase, relativePath);

            // Get all directories
            foreach (string subDir in Editor.ContentDatabase.SearchFolders(relativePath))
            {
                // Add children
                RefreshContentFolderTree(subDir, rootNode);
            }
        }

        private void RefreshContentFileTree(string directory)
        {
            // Search all files
            foreach (string contentGuid in Editor.ContentDatabase.SearchContent(directory))
            {
                // Get the content path and name
                string contentPath = Editor.ContentDatabase.GetContentPath(contentGuid);
                string contentName = Path.GetFileNameWithoutExtension(contentPath);

                // Create directory
                EditorTreeNode fileNode = contentFileTreeView.AddNode(contentName);


                // Make folder
                //rootNode.Icon = folderNormalIcon;

                //// Set menu
                //rootNode.ContextMenu = CreateContentFolderContextMenu(relativePath, parent == null);

                // Add listeners
                //rootNode.OnSelected += (EditorTreeNode node) => Editor.Selection.Select(new FolderObject(contentRelativePath));
                

                // Add drop handler
                //rootNode.DropHandler = new ContentDrop(Editor.ContentDatabase, relativePath);
            }
        }

        private void OnContentFolderTreeNodeExpanded(EditorTreeNode node, bool expanded)
        {
            node.Icon = expanded == false
                ? folderNormalIcon 
                : folderOpenIcon;
        }

        private EditorMenu CreateContentFolderContextMenu(string directoryPath, bool isRoot)
        {
            // Create the menu
            EditorMenu menu = EditorMenu.Create();

            menu.AddItem("Open In Explorer").OnClicked += () =>
            {
                Editor.ContentDatabase.OpenContent(directoryPath);
            };

            // Dont allow modify options on root folder
            if (isRoot == false)
            {
                menu.AddSeparator();
                menu.AddItem("Delete").OnClicked += () =>
                {
                    if (Editor.ShowDialog("Delete Content Folder", "You cannot undo this action and all content within will be deleted!", DialogOptions.YesNo) == true)
                        Editor.ContentDatabase.DeleteContent(directoryPath);
                };
                menu.AddItem("Rename").OnClicked += () =>
                {

                };
            }

            return menu;
        }
    }
}
