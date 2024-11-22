using UniGameEditor.UI;
using UniGameEngine;
using UniGameEngine.Scene;

namespace UniGameEditor.Windows
{
    internal sealed class HierarchyEditorWindow : EditorWindow
    {
        // Type
        private sealed class HierarchyScene
        {
            // Public
            public EditorFoldout Foldout;
            public EditorTreeView Tree;
        }

        private sealed class HierarchySceneDrop : IDropHandler
        {
            // Private
            private GameScene scene = null;

            // Constructor
            public HierarchySceneDrop(GameScene scene)
            {
                this.scene = scene;
            }

            // Methods
            public bool CanDrop(DragDropType type, object dragData)
            {
                return type == DragDropType.Object && dragData is GameObject;
            }

            public void PerformDrop(DragDropType type, object dragData)
            {
                // Get the game object
                GameObject go = (GameObject)dragData;

                // Check for scene
                if(go.scene == scene)
                {
                    // Clear parent
                    go.Transform.Parent = null;
                }
                else
                {
                    // Change scenes
                    //go.Scene = scene;
                }
            }
        }

        private sealed class HierarchyGameObjectDragDrop : IDragHandler, IDropHandler
        {
            // Private
            private GameObject gameObject = null;

            // Constructor
            public HierarchyGameObjectDragDrop(GameObject gameObject)
            {
                this.gameObject = gameObject;
            }

            // Methods
            public bool PerformDrag(out object dragData, out DragDropVisual visual)
            {
                dragData = gameObject;
                visual = DragDropVisual.Move;
                return gameObject != null;
            }

            public bool CanDrop(DragDropType type, object dragData)
            {
                return type == DragDropType.Object && dragData is GameObject;
            }

            public void PerformDrop(DragDropType type, object dragData)
            {
                ((GameObject)dragData).Transform.Parent = gameObject.Transform;
            }
        }

        // Private
        //private EditorTreeView hierarchyTree = null;
        private Dictionary<GameScene, HierarchyScene> scenes = new Dictionary<GameScene, HierarchyScene>();

        // Constructor
        public HierarchyEditorWindow()
        {
            icon = EditorIcon.FindIcon("Hierarchy");
            title = "Hierarchy";
        }

        // Methods
        protected internal override void OnShow()
        {
            //// Create scene
            //EditorLayoutControl hLayout = RootControl.AddHorizontalLayout();

            //hLayout.AddLabel("My Scene Name");

            //RootControl.AddLabel("Hello World");


            // Add listener
            Editor.OnSceneLoaded += AddScene;
            Editor.OnSceneUnloaded += RemoveScene;
        }

        protected internal override void OnHide()
        {
            // Remove listener
            Editor.OnSceneLoaded -= AddScene;
            Editor.OnSceneUnloaded -= RemoveScene;
        }

        private void AddScene(GameScene scene)
        {
            if(scenes.ContainsKey(scene) == false)
            {
                // Create the control
                EditorFoldout foldout = RootControl.AddFoldoutLayout();
                foldout.Header.AddLabel(scene.Name);

                // Set expanded
                foldout.IsExpanded = scene.expanded;

                // Support drop
                foldout.DropHandler = new HierarchySceneDrop(scene);

                // Listen for expanded changed
                foldout.OnExpanded += (EditorFoldout foldout, bool expanded) => scene.expanded = expanded;

                // Create the scene tree view
                scenes.Add(scene, new HierarchyScene
                {
                    Foldout = foldout,
                    Tree = foldout.AddTreeView(),
                });

                // Rebuild the scene
                RebuildHierarchy(scene);

                // Add listener for modified
                scene.OnSceneModified += () => RebuildHierarchy(scene);
            }
        }

        private void RemoveScene(GameScene scene)
        {
            if(scenes.ContainsKey(scene) == true)
            {
                // Remove the scene
            }
        }

        private void RebuildHierarchy()
        {
            // Process all scenes
            foreach(GameScene scene in editor.GameInstance.Scenes)
            {
                // Rebuild the scene
                RebuildHierarchy(scene);
            }
        }

        private void RebuildHierarchy(GameScene scene)
        {
            // Get the tree
            HierarchyScene hierarchyScene;
            if (scenes.TryGetValue(scene, out hierarchyScene) == false)
                return;

            // Clear tree
            hierarchyScene.Tree.ClearNodes();

            // Get root objects
            foreach (GameObject go in scene.GameObjects)
            {
                RebuildHierarchyObject(go, hierarchyScene.Tree);
            }
        }

        private void RebuildHierarchyObject(GameObject current, EditorTreeView treeView)
        {
            // Create node
            EditorTreeNode currentNode = treeView.AddNode();
            currentNode.Header.AddLabel(current.Name);

            // Build children
            RebuildHierarchyObjectChildren(current, currentNode);
        }

        private void RebuildHierarchyObject(GameObject current, EditorTreeNode treeNode)
        {
            // Create node
            EditorTreeNode currentNode = treeNode.AddNode();
            currentNode.Header.AddLabel(current.Name);

            // Build children
            RebuildHierarchyObjectChildren(current, currentNode);
        }

        private void RebuildHierarchyObjectChildren(GameObject current, EditorTreeNode currentNode)
        {
            // Set expanded state
            currentNode.IsExpanded = current.expanded;

            // Listen for selected
            currentNode.OnSelected += (EditorTreeNode treeNode) => Editor.Selection.Select(current);

            // Listen for expanded changed
            currentNode.OnExpanded += (EditorTreeNode treeNode, bool expanded) => current.expanded = expanded;

            // Create drag handler
            HierarchyGameObjectDragDrop dragDrop = new HierarchyGameObjectDragDrop(current);

            currentNode.DragHandler = dragDrop;
            currentNode.DropHandler = dragDrop;

            // Check for children
            if(current.Transform.HasChildren == true)
            {
                // Process all children
                foreach(Transform child in current.Transform.Children)
                {
                    RebuildHierarchyObject(child.GameObject, currentNode);
                }
            }
        }
    }
}
