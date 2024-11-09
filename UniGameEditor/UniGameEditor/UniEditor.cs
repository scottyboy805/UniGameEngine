using UniGameEngine;
using UniGameEngine.Scene;

namespace UniGameEditor
{
    public sealed class UniEditor : UniGame
    {
        // Private
        private Selection selection = new Selection();

        private bool isProjectOpen = false;
        private string projectPath = null;
        private string projectDirectory = null;
        private string contentDirectory = null;
        private string libraryDirectory = null;

        private bool isPlaying = false;

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

        public override bool IsEditor => true;

        public override bool IsPlaying => isPlaying;

        // Constructor
        public UniEditor()
        {

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
            Content.RootDirectory = libraryDirectory;
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
