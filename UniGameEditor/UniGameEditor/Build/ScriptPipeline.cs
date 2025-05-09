using System.Diagnostics;
using UniGameEngine;

namespace UniGameEditor.Build
{
    public enum BuildConfiguration
    {
        Debug,
        Release,
    }

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
            
            // Set output directory
            scriptProject.OutputPath = relativeOutputPath;
            scriptProject.Save();


            // Refresh the solution
            RefreshCSharpSolution(project);

            // Add default references - make sure to run after solution gen because that step will copy reference assemblies to the library
            AddDefaultProjectReferences(project, scriptProject);


            return scriptProject;
        }

        public static void BuildCSharpSolution(Project project, BuildConfiguration configuration = BuildConfiguration.Release, bool rebuild = false)
        {
            // Check for null
            if(project == null)
                throw new ArgumentNullException(nameof(project));

            // Get solution path by refreshing before build
            string solutionPath = RefreshCSharpSolution(project);

            // Check for rebuild
            if(rebuild == true)
            {
                // Create clean command
                string cleanCommand = $"clean {solutionPath}";

                // Run clear
                RunDotnetCommand(cleanCommand);
            }

            // Select configuration
            string configSymbol = configuration.ToString();

            // Build command
            string buildCommand = $@"build {solutionPath} -c {configSymbol} -o {project.ScriptBuildFolder}";

            // Build the solution
            RunDotnetCommand(buildCommand);
        }

        public static string RefreshCSharpSolution(Project project)
        {
            // Check for null
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            // Get path
            string solutionPath = Path.Combine(project.ProjectFolder, project.Name + CSharpSolutionExtension);

            // Delete existing
            if(File.Exists(solutionPath) == true)
                File.Delete(solutionPath);


            // Create the new solution
            string createCommand = $@"new sln -n {project.Name} -o {project.ProjectFolder}";

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

            // Generate build dir props file
            GenerateBuildDirectoryProperties(project);

            // Ensure dependant assemblies are copied to the library
            RestoreReferenceAssemblies(project);

            return solutionPath;
        }

        public static void RestoreReferenceAssemblies(Project project)
        {
            // Check for null
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            // Get reference assembly paths
            string[] referenceAssemblies =
            {
                typeof(UniGame).Assembly.Location,
            };

            // Copy all references
            foreach (string referenceAssembly in referenceAssemblies)
            {
                // Get copy path
                string targetPath = Path.Combine(project.ScriptBuildFolder, Path.GetFileName(referenceAssembly));

                // Get target directory
                string targetFolder = Directory.GetParent(targetPath).FullName;

                // Create if required
                if(Directory.Exists(targetFolder) == false)
                    Directory.CreateDirectory(targetFolder);

                // Copy the file with overwrite
                File.Copy(referenceAssembly, targetPath, true);
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

        private static void GenerateBuildDirectoryProperties(Project project)
        {
            // Get target file
            string dirPropFile = Path.Combine(project.ScriptFolder, DirectoryPropertiesFile);

            // Check for exists
            if (File.Exists(dirPropFile) == false)
            {
                // Xml string
                string xml = $@"<?xml version=""1.0""?>
<Project>
    <PropertyGroup>
        <BaseIntermediateOutputPath>$(SolutionDir)\{Path.GetRelativePath(project.ProjectFolder, project.ScriptIntermediateFolder)}</BaseIntermediateOutputPath>
        <IntermediateOutputPath>$(BaseIntermediateOutputPath)\$(Configuration)</IntermediateOutputPath>
    </PropertyGroup>
</Project>";

                // Write to file
                File.WriteAllText(dirPropFile, xml);
            }
        }

        private static void AddDefaultProjectReferences(Project project, ScriptProject scriptProject)
        {
            // Add UniGame
            AddPathProjectReference(project, scriptProject, typeof(UniGame).Assembly.Location, false);
        }

        public static void AddPathProjectReference(Project project, ScriptProject scriptProject, string referencePath, bool copySource)
        {
            // Check for project
            if(project == null)
                throw new ArgumentNullException(nameof(project));

            // Check for script project
            if(scriptProject == null)
                throw new ArgumentNullException(nameof(scriptProject));

            // Check for invalid
            if (string.IsNullOrEmpty(referencePath) == true)
                throw new ArgumentException("Reference path cannot be null or empty");

            // Check for file exists
            if (File.Exists(referencePath) == false)
                throw new ArgumentException("Could not locate reference path: " + referencePath);

            // Get the output path
            string targetPath = Path.Combine(project.ScriptBuildFolder, Path.GetFileName(referencePath));

            // Check for copy
            if (copySource == true)
            {
                // Copy if it does not already exist
                if(File.Exists(targetPath) == false)
                    File.Copy(referencePath, targetPath);
            }

            // Get path relative to project
            string relativeTargetPath = Path.GetRelativePath(scriptProject.CSharpProjectFolder, targetPath);

            // Add the reference
            scriptProject.AddReferencePath(relativeTargetPath);
            scriptProject.Save();
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

            // Check for exit code
            if (process.ExitCode != 0)
                throw new ApplicationException("Error executing dotnet command");
        }
    }
}
