using System.Diagnostics;

namespace UniGameEditor.Build
{
    public static class ScriptPipeline
    {
        // Public
        public const string CSharpScriptExtension = ".cs";
        public const string CSharpProjectExtension = ".csproj";
        public const string CSharpSolutionExtension = ".sln";
        public const string DirectoryPropertiesFile = "Directory.Build.props";

        // Methods
        public static ScriptProject CreateNewCSharpProject(Project project, string relativePath)
        {
            // Check for null
            if(project == null)
                throw new ArgumentNullException(nameof(project));

            // Check for invalid
            if (string.IsNullOrEmpty(relativePath) == true)
                throw new ArgumentException("Relative path cannot be null or empty");

            // Check invalid extension
            if (Path.HasExtension(relativePath) == true)
                throw new InvalidOperationException("Relative path must be a folder path for the project");

            // Get project name
            string projectName = Path.GetFileNameWithoutExtension(relativePath);

            // Get project name
            string projectFolderPath = Path.Combine(project.ScriptFolder, projectName);

            // Get the full path
            string projectFullPath = Path.Combine(projectFolderPath, projectName + CSharpProjectExtension);

            // Check for already exists
            if(File.Exists(projectFullPath) == true)
                throw new InvalidOperationException("Project path already exists: " + projectFullPath);

            // Create if required
            if(Directory.Exists(projectFolderPath) == false)
                Directory.CreateDirectory(projectFolderPath);

            // Get temp the output directory
            string tempProjectDirectory = Path.Combine(project.LibraryFolder, "TempProject");
            string tempDirectory = Path.Combine(tempProjectDirectory, projectName);

            // Build the command
            string command = @$"new classlib --language ""C#"" --framework net8.0 --name ""{projectName}"" --output ""{tempDirectory}""";

            // Run the command
            RunDotnetCommand(command);

            // Copy project file to intended output
            File.Copy(Path.Combine(tempDirectory, projectName + CSharpProjectExtension), projectFullPath);

            // Delete temp
            Directory.Delete(tempProjectDirectory, true);

            // Open the project for editing
            ScriptProject scriptProject = new ScriptProject(projectFullPath);

            // Get relative output path and intermediate paths
            string relativeOutputPath = Path.GetRelativePath(projectFolderPath, project.ScriptBuildFolder);
            //string relativeIntermediatePath = Path.GetRelativePath(projectFolderPath, project.ScriptIntermediateFolder);

            // Set output directory
            scriptProject.OutputPath = relativeOutputPath;

            // Set intermediate directory
            //scriptProject.IntermediatePath = relativeIntermediatePath;
            scriptProject.Save();


            


            // Refresh the solution
            RefreshCSharpSolution(project);


            // Generate build dir props file
            GenerateBuildDirectoryProperties(project, projectFolderPath);


            return scriptProject;
        }

        public static void RefreshCSharpSolution(Project project)
        {
            // Check for null
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            // Get path
            string solutionPath = Path.Combine(project.ProjectFolder, project.ProjectName + CSharpSolutionExtension);

            // Delete existing
            if(File.Exists(solutionPath) == true)
                File.Delete(solutionPath);


            // Create the new solution
            string createCommand = $@"new sln -n {project.ProjectName} -o {project.ProjectFolder}";

            // Run create command
            RunDotnetCommand(createCommand);


            // Scan all C# projects found
            foreach(ScriptProject scriptProject in GetCSharpProjects(project))
            {
                // Add to solution
                string addCommand = $@"sln {solutionPath} add {scriptProject.CSharpProjectPath} --in-root";

                // Run create command
                RunDotnetCommand(addCommand);
            }
        }

        public static IEnumerable<ScriptProject> GetCSharpProjects(Project project)
        {
            // Check for null
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            // Scan for script projects in the scripts folder
            foreach(string cSharpProject in Directory.EnumerateFiles(project.ScriptFolder, "*" + CSharpProjectExtension, SearchOption.AllDirectories))
            {
                // Load the project
                ScriptProject scriptProject = null;

                try
                {
                    // Try to load
                    scriptProject = new ScriptProject(cSharpProject);
                }
                catch { }

                // Check for loadable
                if (scriptProject != null)
                    yield return scriptProject;
            }
        }

        public static void GenerateBuildDirectoryProperties(Project project, string projectFolder)
        {
            // Check for null
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            // Get relative path
            string relativeIntermediatePath = Path.GetRelativePath(projectFolder, project.ScriptIntermediateFolder);

            // Get target file
            string dirPropFile = Path.Combine(projectFolder, DirectoryPropertiesFile);

            // Check for exists
            if (File.Exists(dirPropFile) == false)
            {
                // Xml string
                string xml = $@"<?xml version=""1.0""?>
<Project>
    <PropertyGroup>
        <BaseIntermediateOutputPath>{relativeIntermediatePath}</BaseIntermediateOutputPath>
        <IntermediateOutputPath>$(BaseIntermediateOutputPath)\$(Configuration)</IntermediateOutputPath>
    </PropertyGroup>
</Project>";

                // Write to file
                File.WriteAllText(dirPropFile, xml);
            }
        }

        private static void RunDotnetCommand(string arguments)
        {
            // Create process
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "dotnet.exe",
                Arguments = arguments,
            };

            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo = startInfo;
            bool started = process.Start();

            // Dotnet maybe not installed
            if (started == false)
                throw new ApplicationException("Could not execute dotnet command. Please make sure the dotnet SDK is installed!");

            // Wait for it to finish
            process.WaitForExit();
        }
    }
}
