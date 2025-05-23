﻿using Microsoft.Xna.Framework;
using System.Xml.Linq;

namespace UniGameEditor.Build
{
    public sealed class ScriptProject
    {
        // Private
        private const string outputName = "AssemblyName";
        private const string outputPathName = "OutDir";
        private const string baseIntermediateOutputPathName = "BaseIntermediateOutputPath";

        private readonly string projectPath;
        private readonly XDocument projectDoc;
        private readonly XElement projectGroup;
        private readonly XElement propertyGroup;

        // Properties
        public string CSharpProjectPath => projectPath;
        public string CSharpProjectFolder => Directory.GetParent(projectPath).FullName;

        public string OutputName
        {
            get
            {
                // Try to find element
                XElement element = propertyGroup.Element(outputName);

                // Check for found
                if (element != null)
                    return element.Value;

                // Get name from path
                return Path.GetFileNameWithoutExtension(projectPath);
            }
        }

        public string OutputPath
        {
            get
            {
                string outputPath;
                GetProperty(outputPathName, out outputPath);

                return outputPath;
            }
            set { SetProperty(outputPathName, value); }
        }

        public string IntermediatePath
        {
            get
            {
                string intermediatePath;
                GetProperty(baseIntermediateOutputPathName, out intermediatePath);

                return intermediatePath;
            }
            set { SetProperty(baseIntermediateOutputPathName, value); }
        }

        // Constructor
        public ScriptProject(string projectPath)
        {
            // Check for invalid
            if (string.IsNullOrEmpty(projectPath) == true)
                throw new ArgumentException("Project path cannot be null or empty");

            // Get project path
            this.projectPath = projectPath;

            // Load project
            this.projectDoc = XDocument.Load(projectPath);
            this.projectGroup = projectDoc.Root;
            this.propertyGroup = projectDoc.Descendants("PropertyGroup")
                .FirstOrDefault();
            
            // Check for null 
            if (this.propertyGroup == null)
                this.projectDoc.Root.Add(new XElement("PropertyGroup"));
        }

        // Methods
        public string GetOutputAssemblyPath(Project project)
        {
            // Check for null
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            // Combine the output path
            return Path.Combine(project.ScriptBuildFolder, OutputName + ".dll");
        }

        public bool AddReferencePath(string referencePath)
        {
            // Check for invalid
            if (string.IsNullOrEmpty(referencePath) == true)
                throw new ArgumentException("Reference path cannot be null or empty");

            // Check for extension
            if (Path.GetExtension(referencePath) != ".dll")
                throw new ArgumentException("Reference path must be a path to a `.dll` file");

            // Get the reference name
            string referenceName = Path.GetFileNameWithoutExtension(referencePath);

            // Find the node
            XElement itemGroup = projectGroup.Element("ItemGroup");

            // Check for null
            if(itemGroup == null)
            {
                itemGroup = new XElement("ItemGroup");
                projectGroup.Add(itemGroup);
            }

            // Check for already added
            if(itemGroup.Elements("Reference")
                .Any(r => r.Attribute("include")?
                .Value == referenceName))
            {
                // Reference already exists
                return false;
            }

            // Add the new reference
            itemGroup.Add(new XElement("Reference",
                new XAttribute("Include", referenceName),
                new XElement("HintPath", referencePath)));

            // Reference was added
            return true;
        }

        public void Save()
        {
            // Save to path
            projectDoc.Save(projectPath);
        }

        private bool GetProperty(string name, out string value)
        {
            // Try to find element
            XElement element = propertyGroup.Element(name);

            // Check for null
            value = element != null
                ? element.Value 
                : string.Empty;

            // Check for found
            return element != null;
        }

        private void SetProperty(string name, string value)
        {
            // Try to find element
            XElement element = propertyGroup.Element(name);

            // Check for null
            if(element == null)
            {
                element = new XElement(name);
                this.propertyGroup.Add(element);
            }

            // Set value
            element.Value = value;
        }
    }
}
