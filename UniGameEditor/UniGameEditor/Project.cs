using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace UniGameEditor
{
    [DataContract]
    public sealed class Project
    {
        // Private        
        private readonly string projectPath;
        private readonly string projectFolder;
        private readonly string contentFolder;
        private readonly string libraryFolder;
        private readonly string scriptFolder;

        [DataMember(Name = "ProjectName")]
        private string projectName;
        [DataMember(Name = "DeveloperName")]
        private string developerName;

        // Public
        public const string FileExtension = ".unigame";

        // Properties
        public string ProjectName => projectName;
        public string ProjectPath => projectPath;
        public string ProjectFolder => projectFolder;
        public string ContentFolder => contentFolder;
        public string LibraryFolder => libraryFolder;
        public string ScriptFolder => scriptFolder;


        public string ContentBuildFolder => Path.Combine(LibraryFolder, "content-build");
        public string ScriptBuildFolder => Path.Combine(LibraryFolder, "script-build");
        public string ScriptIntermediateFolder => Path.Combine(LibraryFolder, "script-obj");

        // Constructor
        internal Project(string projectPath, bool mustExist = true)
        {
            // Check for invalid
            if (string.IsNullOrEmpty(projectPath) == true)
                throw new ArgumentException("Project path cannot be null or empty");

            // Check extension
            if (Path.GetExtension(projectPath) != FileExtension)
                throw new ArgumentException("Project path must have the extension: " + FileExtension);

            // Check for project file
            if (mustExist == true && File.Exists(projectPath) == false)
                throw new ArgumentException("Project path does not exist: " + projectPath);

            this.projectPath = projectPath;
            this.projectFolder = Directory.GetParent(projectPath).FullName;
            this.contentFolder = Path.Combine(projectFolder, "Content");
            this.libraryFolder = Path.Combine(projectFolder, "Library");
            this.scriptFolder = Path.Combine(projectFolder, "Scripts");

            this.projectName = Path.GetFileNameWithoutExtension(projectPath);

            
            // Create content on demand
            if(Directory.Exists(contentFolder) == false)
                Directory.CreateDirectory(contentFolder);

            // Create library folder on demand
            if(Directory.Exists(libraryFolder) == false)
                Directory.CreateDirectory(libraryFolder);

            // Create scripts on demand
            if(Directory.Exists(scriptFolder) == false)
                Directory.CreateDirectory(scriptFolder);
        }

        // Methods
        public void Load()
        {
            // Load the json
            string projectJson = File.ReadAllText(projectPath);

            // Check for valid
            if (string.IsNullOrEmpty(projectJson) == true)
                return;

            // Load the json
            JsonConvert.PopulateObject(projectJson, this);
        }

        public void Save()
        {
            // Serialize the object
            string projectJson = JsonConvert.SerializeObject(this, Formatting.Indented);

            // Save text
            File.WriteAllText(projectPath, projectJson);
        }

        public static Project CreateNew(string createInFolder, string projectName)
        {
            // Check for invalid
            if (string.IsNullOrEmpty(createInFolder) == true)
                throw new ArgumentException("Create in folder cannot be null or empty");

            // Check for project name
            if (string.IsNullOrEmpty(projectName) == true)
                throw new ArgumentException("Project name cannot be null or empty");

            // Get project folder path
            string projectFolderPath = Path.Combine(createInFolder, projectName);

            // Check for folder already exists
            if (Directory.Exists(projectFolderPath) == true)
                throw new InvalidOperationException("Project folder already exists");


            // Create project folder
            Directory.CreateDirectory(projectFolderPath);

            // Get project path
            string projectPath = Path.Combine(projectFolderPath, projectName + FileExtension);

            // Create project file in memory - it does not exist on disk yet
            Project project = new Project(projectPath, false);

            // Setup project
            project.projectName = projectName;

            // Save the project info
            project.Save();

            // Get the project
            return project;
        }
    }
}
