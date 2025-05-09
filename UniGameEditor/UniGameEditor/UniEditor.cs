using Microsoft.Win32;
using UniGameEditor.UI;
using UniGameEngine;
using UniGameEngine.Scene;
using UniGameEngine.UI.Events;
using UniGameEngine.UI;
using UniGameEngine.Graphics;
using System.Windows;
using UniGameEditor.Content;
using System.Windows.Threading;
using UniGameEngine.Physics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using UniGameEditor.Build;
using UniGamePipeline;

namespace UniGameEditor
{
    public enum DialogOptions
    {
        Ok,
        OkCancel,
        YesNo,
    }

    public sealed class UniEditor
    {
        // Events
        public event Action OnProjectLoaded;
        public event Action<GameScene> OnSceneLoaded;
        public event Action<GameScene> OnSceneUnloaded;

        // Private
        private static Dispatcher mainThreadDispatcher;

        private string editorPath;
        private string editorFolder;
        private Selection selection = new Selection();
        private Undo undo = new Undo();

        private Project project = null;
        private ContentDatabase contentDatabase = null;
        private UniEditorGameInstance gameInstance = null;

        // Public
        public static readonly Version EditorVersion = typeof(UniEditor).Assembly.GetName().Version;

        // Internal
        internal EditorMenu menu = null;
        internal EditorMenu fileMenu = null;
        internal EditorMenu editMenu = null;
        internal EditorMenu contentMenu = null;
        internal EditorMenu gameObjectMenu = null;
        internal EditorMenu componentMenu = null;
        internal EditorMenu windowMenu = null;

        // Properties
        public string EditorPath => editorPath;
        public string EditorFolder => editorFolder;
        public Selection Selection => selection;
        public Undo Undo => undo;
        public bool IsProjectOpen => project != null;
        public Project Project => project;
        public GameScene ActiveScene => gameInstance.Scenes.Count > 0 ? gameInstance.Scenes[0] : null;
        public ContentDatabase ContentDatabase => contentDatabase;
        internal UniEditorGameInstance GameInstance => gameInstance;
        internal Dispatcher MainThreadDispatcher => mainThreadDispatcher;

        public EditorMenu Menu => menu;
        public EditorMenu FileMenu => fileMenu;
        public EditorMenu EditMenu => editMenu;
        public EditorMenu ContentMenu => contentMenu;
        public EditorMenu GameObjectMenu => gameObjectMenu;
        public EditorMenu ComponentMenu => componentMenu;
        public EditorMenu WindowMenu => windowMenu;

        // Constructor
        internal UniEditor()
        {
            gameInstance = new UniEditorGameInstance();
            mainThreadDispatcher = Dispatcher.CurrentDispatcher;

            editorPath = Environment.ProcessPath;
            editorFolder = Directory.GetParent(editorPath).FullName;
        }

        // Methods
        internal void Initialize()
        {
            // Initialize menus
            InitializeFileMenu();
            InitializeEditMenu();
            InitializeContentMenu();
            InitializeGameObjectMenu();
            InitializeComponentMenu();            
        }

        internal void Shutdown()
        {
            // Check for project open
            if(IsProjectOpen == true)
            {
                // Save changes to project
                project.Save();
                project = null;
            }
        }

        public GameScene NewScene(string sceneName)
        {
            // Create the scene
            GameScene scene = new GameScene(sceneName);

            // Create default cube
            ModelRenderer cube = scene.CreateObject<ModelRenderer>("Cube", typeof(BoxCollider));
            cube.Transform.WorldPosition = new Vector3(0f, 1f, 0f);
            cube.Transform.LocalScale *= 0.01f;
            cube.Model = ContentDatabase.Load<Model>("DefaultCube");
            cube.GameObject.CreateComponent<TestScript>();

            // Create ground cube
            ModelRenderer ground = scene.CreateObject<ModelRenderer>("Ground", typeof(BoxCollider));
            ground.Transform.LocalScale = new Vector3(5f, 0.01f, 5f);
            ground.Transform.LocalScale *= 0.01f;
            ground.Model = cube.Model;

            // Create light
            Light light = scene.CreateObject<Light>("Light");
            light.Transform.LocalEulerAngles = new Vector3(-60, -90, 0);

            // Create camera
            Camera camera = scene.CreateObject<Camera>("Camera");
            camera.Transform.WorldPosition = new Vector3(-7, 1, -4);
            //camera.Transform.WorldPosition = new Vector3(5, -1, -5);
            camera.Transform.WorldRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(45));

            // Activate the scene
            scene.Activate();

            // Trigger event
            if(OnSceneLoaded != null)
                OnSceneLoaded(scene);

            return scene;
        }

        public void NewProject(string createInFolder, string projectName)
        {
            // Create project
            project = Project.CreateNew(createInFolder, projectName);

            // Open the newly created project
            OpenProject(project.ProjectPath);
        }

        public void OpenProject(string projectPath)
        {
            // Create project
            project = new Project(projectPath);

            // Create content
            contentDatabase = new ContentDatabase(project.ProjectFolder, project.ContentFolder, project.ContentBuildFolder, gameInstance.Services);

            // Update content directory
            gameInstance.Content = contentDatabase;

            // Refresh content database
            contentDatabase.RefreshContent();

            // Load base content
            gameInstance.LoadEditorContent();


            // Trigger event
            if (OnProjectLoaded != null)
                OnProjectLoaded();


            //contentDatabase.ImportContent("WGPU-Logo.png");


            //var result = ScriptPipeline.CreateNewCSharpProject(project, "TestProject");
            //string outputPath = result.GetOutputAssemblyPath(project);


            // Build the solution
            ScriptPipeline.BuildCSharpSolution(project);

            GameScene scene = NewScene("My Scene");

            //scene.CreateObject<Camera>("Camera");
            //UICanvas canvas = scene.CreateObject<UICanvas>("Canvas", typeof(UIEventDispatcher));
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

        public bool ShowDialog(string title, string message, DialogOptions buttonOptions = DialogOptions.YesNo)
        {
            // Select option
            MessageBoxButton buttons = buttonOptions switch
            {
                DialogOptions.OkCancel => MessageBoxButton.OKCancel,
                DialogOptions.YesNo => MessageBoxButton.YesNo,
                _ => MessageBoxButton.OK,
            };

            // Show the message box
            MessageBoxResult result = MessageBox.Show(message, title, buttons);

            // Check for success
            return result == MessageBoxResult.Yes
                || result == MessageBoxResult.OK;
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
            contentMenu.AddItem("Build").OnClicked += () =>
            {
                ContentDatabase.BuildAllContent();
            };
            contentMenu.AddItem("Rebuild").OnClicked += () => 
            { 
                ContentDatabase.RebuildAllContent(); 
            };
            contentMenu.AddItem("Clean").OnClicked += () => 
            { 
                ContentDatabase.CleanAllContent(); 
            };
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

        internal static void DoEvent(Action action)
        {
            try
            {
                if (action != null)
                    mainThreadDispatcher.Invoke(action);
            }
            catch (Exception e) { Debug.LogException(e); }
        }

        internal static void DoEvent<T>(Action<T> action, T arg0)
        {
            try
            {
                if (action != null)
                    mainThreadDispatcher.Invoke(action, arg0);
            }
            catch (Exception e) { Debug.LogException(e); }
        }

        internal static void DoEvent<T0, T1>(Action<T0, T1> action, T0 arg0, T1 arg1)
        {
            try
            {
                if (action != null)
                    mainThreadDispatcher.Invoke(action, arg0, arg1);
            }
            catch (Exception e) { Debug.LogException(e); }
        }
    }
}
