using Microsoft.Win32;
using UniGameEditor.UI;
using UniGameEngine;
using UniGameEngine.Scene;
using UniGameEngine.UI.Events;
using UniGameEngine.UI;
using UniGameEngine.Graphics;
using System.Windows;

namespace UniGameEditor
{
    public sealed class UniEditor
    {
        // Events
        public event Action<GameScene> OnSceneLoaded;
        public event Action<GameScene> OnSceneUnloaded;

        // Private
        private Selection selection = new Selection();
        private Undo undo = new Undo();

        private bool isProjectOpen = false;
        private string projectPath = null;
        private string projectDirectory = null;
        private string contentDirectory = null;
        private string libraryDirectory = null;

        private UniEditorGameInstance gameInstance = null;

        // Internal
        internal EditorMenu menu = null;
        internal EditorMenu fileMenu = null;
        internal EditorMenu editMenu = null;
        internal EditorMenu contentMenu = null;
        internal EditorMenu gameObjectMenu = null;
        internal EditorMenu componentMenu = null;
        internal EditorMenu windowMenu = null;

        // Properties
        public Selection Selection
        {
            get { return selection; }
        }

        public Undo Undo
        {
            get { return undo; }
        }

        public bool IsProjectOpen
        {
            get { return isProjectOpen; }
        }

        public string ProjectPath
        {
            get { return projectPath; }
        }

        public string ProjectDirectory
        {
            get { return projectDirectory; }
        }

        public string ContentDirectory
        {
            get { return contentDirectory; }
        }

        public GameScene ActiveScene
        {
            get { return gameInstance.Scenes.Count > 0 ? gameInstance.Scenes[0] : null; }
        }

        internal UniEditorGameInstance GameInstance
        {
            get { return gameInstance; }
        }

        public EditorMenu Menu => menu;
        public EditorMenu FileMenu => fileMenu;
        public EditorMenu EditMenu => editMenu;
        public EditorMenu ContentMenu => contentMenu;
        public EditorMenu GameObjectMenu => gameObjectMenu;
        public EditorMenu ComponentMenu => componentMenu;
        public EditorMenu WindowMenu => windowMenu;

        // Constructor
        public UniEditor()
        {
            gameInstance = new UniEditorGameInstance();
        }

        // Methods
        public void Initialize()
        {
            // Initialize menus
            InitializeFileMenu();
            InitializeEditMenu();
            InitializeContentMenu();
            InitializeGameObjectMenu();
            InitializeComponentMenu();            
        }

        public GameScene NewScene(string sceneName)
        {
            // Create the scene
            GameScene scene = new GameScene(sceneName);

            // Activate the scene
            scene.Activate();

            // Trigger event
            if(OnSceneLoaded != null)
                OnSceneLoaded(scene);

            return scene;
        }

        public void OpenProject(string projectPath)
        {
            this.isProjectOpen = true;
            this.projectPath = projectPath;
            this.projectDirectory = Directory.GetParent(projectPath).FullName;
            this.contentDirectory = Path.Combine(projectDirectory, "Content");
            this.libraryDirectory = Path.Combine(projectDirectory, "Library");
            
            // Update content directory
            gameInstance.Content.RootDirectory = libraryDirectory;

            // Load base content
            gameInstance.LoadEditorContent();


            GameScene scene = NewScene("My Scene");

            scene.CreateObject<Camera>("Camera");
            UICanvas canvas = scene.CreateObject<UICanvas>("Canvas", typeof(UIEventDispatcher));
            //Image.Create(canvas.GameObject);

            //PipelineProject
        }

        #region IODialog
        public bool ShowSaveFileDialog(ref string fileName, string title, string filter, string directory = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();            
            saveFileDialog.FileName = fileName;
            saveFileDialog.Title = title;
            saveFileDialog.Filter = filter;            

            if (directory != null)
                saveFileDialog.DefaultDirectory = directory;

            // Check for success
            if(saveFileDialog.ShowDialog() == true)
            {
                fileName = saveFileDialog.FileName;
                return true;
            }
            fileName = null;
            return false;
        }

        public bool ShowOpenFileDialog(ref string fileName, string title, string filter, string directory = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = fileName;
            openFileDialog.Title = title;
            openFileDialog.Filter = filter;

            if(directory != null)
                openFileDialog.DefaultDirectory = directory;

            // Check for success
            if(openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                return true;
            }
            fileName = null;
            return false;
        }

        public bool ShowOpenFolderDialog(ref string folderName, string title, string directory = null)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.FolderName = folderName;
            openFolderDialog.Title = title;

            if(directory != null)
                openFolderDialog.DefaultDirectory = directory;

            // Check for success
            if(openFolderDialog.ShowDialog() == true)
            {
                folderName = openFolderDialog.FolderName;
                return true;
            }
            folderName = null;
            return false;
        }
        #endregion

        #region FileMenu
        private void InitializeFileMenu()
        {
            fileMenu.AddItem("New Project").OnClicked += () =>
            {
                string fileName = "My Project";
                if(ShowSaveFileDialog(ref fileName, "New Project", "UniGame Project (*.unigame)|*.unigame") == true)
                {

                }
            };
            fileMenu.AddItem("Open Project").OnClicked += () =>
            {
                string fileName = "";
                if(ShowOpenFileDialog(ref fileName, "Open Project", "UniGame Project (*.unigame)|*.unigame") == true)
                {

                }
            };
            fileMenu.AddSeparator();
            fileMenu.AddItem("New Scene");
            fileMenu.AddItem("Open Scene");
            fileMenu.AddItem("Open Recent Scene");
            fileMenu.AddSeparator();
            fileMenu.AddItem("Save");
            fileMenu.AddItem("Save As");
            fileMenu.AddSeparator();
            fileMenu.AddItem("Exit").OnClicked += () => Application.Current.Shutdown();
        }
        #endregion
        #region EditMenu
        private void InitializeEditMenu()
        {
            EditorMenuItem undoItem = editMenu.AddItem("Undo");
            {
                undoItem.OnShown += () => undoItem.IsEnabled = undo.CanUndo;
                undoItem.OnClicked += () => undo.PerformUndo();
            }
            EditorMenuItem redoItem = editMenu.AddItem("Redo");
            {
                redoItem.OnShown += () => redoItem.IsEnabled = undo.CanRedo;
                redoItem.OnClicked += () => undo.PerformRedo();
            }
            editMenu.AddSeparator();
            editMenu.AddItem("Cut");
            editMenu.AddItem("Copy");
            editMenu.AddItem("Paste");
            editMenu.AddSeparator();
            editMenu.AddItem("Duplicate");
            editMenu.AddItem("Rename");
            EditorMenuItem deleteItem = editMenu.AddItem("Delete");
            {
                deleteItem.OnShown += () => deleteItem.IsEnabled = selection.HasAnySelection;
                deleteItem.OnClicked += () =>
                {
                    // Delete selection
                    foreach (GameElement selectedElement in selection.GetSelected<GameElement>())
                        GameElement.Destroy(selectedElement);

                    // Clear selection
                    selection.Clear();
                };
            }
        }
        #endregion
        #region ContentMenu
        private void InitializeContentMenu()
        {
            contentMenu.AddItem("Import").OnClicked += () =>
            {

            };
            contentMenu.AddItem("Edit").OnClicked += () =>
            {

            };
            contentMenu.AddSeparator();
            contentMenu.AddItem("Build");
            contentMenu.AddItem("Rebuild");
            contentMenu.AddItem("Clean");
        }
        #endregion
        #region GameObjectMenu
        private void InitializeGameObjectMenu()
        {
            gameObjectMenu.AddItem("Create Empty").OnClicked += () =>
            {
                undo.RecordElementCreated(ActiveScene.CreateEmptyObject("New Object"));
            };
            gameObjectMenu.AddSeparator();
            gameObjectMenu.AddItem("Camera").OnClicked += () =>
            {
                undo.RecordElementCreated(ActiveScene.CreateObject<Camera>("New Camera"));
            };

            // 2D objects
            EditorMenuItem gameObject2DMenu = gameObjectMenu.AddItem("2D");
            gameObject2DMenu.AddItem("Sprite").OnClicked += () =>
            {
                undo.RecordElementCreated(ActiveScene.CreateObject<SpriteRenderer>("New Sprite"));
            };

            // 3D objects
            EditorMenuItem gameObject3DMenu = gameObjectMenu.AddItem("3D");

            // Audio
            EditorMenuItem gameObjectAudioMenu = gameObjectMenu.AddItem("Audio");

            // UI
            EditorMenuItem gameObjectUIMenu = gameObjectMenu.AddItem("UI");
            gameObjectUIMenu.AddItem("Canvas").OnClicked += () =>
            {
                undo.RecordElementCreated(ActiveScene.CreateObject<UICanvas>("New Canvas", typeof(UIEventDispatcher)));
            };
            gameObjectUIMenu.AddSeparator();
            gameObjectUIMenu.AddItem("Image").OnClicked += () =>
            {
                undo.RecordElementCreated(Image.Create(ActiveScene));
            };
            gameObjectUIMenu.AddItem("Label").OnClicked += () =>
            {
                //undo.RecordElementCreated(Label.cre)
            };
            gameObjectUIMenu.AddItem("Button").OnClicked += () =>
            {
                undo.RecordElementCreated(Button.Create(ActiveScene));
            };
            gameObjectUIMenu.AddItem("Toggle").OnClicked += () =>
            {
                undo.RecordElementCreated(Toggle.Create(ActiveScene));
            };
        }
        #endregion
        #region ComponentMenu
        private void InitializeComponentMenu()
        {

        }
        #endregion
    }
}
