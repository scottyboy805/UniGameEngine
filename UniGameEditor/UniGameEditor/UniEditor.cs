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
        private string contentDirectory = null;

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

        public string ContentDirectory
        {
            get { return contentDirectory; }
        }

        public override bool IsEditor => true;

        public override bool IsPlaying => isPlaying;

        // Methods
        public void OpenProject(string projectPath)
        {
            this.isProjectOpen = true;
            this.projectPath = projectPath;
            this.contentDirectory = Path.Combine(Directory.GetParent(projectPath).FullName, "Content");
        }
    }
}
