using UniGameEngine;
using UniGameEngine.Scene;

namespace UniGameEditor
{
    public sealed class UniEditor
    {
        // Private
        private Selection selection = new Selection();

        private bool isProjectOpen = false;
        private string projectPath = null;
        private string projectDirectory = null;
        private string contentDirectory = null;
        private string libraryDirectory = null;

        private UniEditorGameInstance gameInstance = null;

        // Properties
        public Selection Selection
        {
            get { return selection; }
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

        internal UniEditorGameInstance GameInstance
        {
            get { return gameInstance; }
        }

        // Constructor
        public UniEditor()
        {
            gameInstance = new UniEditorGameInstance();
        }

        // Methods
        public void OpenProject(string projectPath)
        {
            this.isProjectOpen = true;
            this.projectPath = projectPath;
            this.projectDirectory = Directory.GetParent(projectPath).FullName;
            this.contentDirectory = Path.Combine(projectDirectory, "Content");
            this.libraryDirectory = Path.Combine(projectDirectory, "Library");

            // Update content directory
            gameInstance.Content.RootDirectory = libraryDirectory;
        }

        //internal void InitializeEditor()
        //{
        //    base.Initialize();
        //}

        //internal void UpdateFrame()
        //{
        //    base.
        //}

        //internal void RenderFrame()
        //{

        //}
    }
}
