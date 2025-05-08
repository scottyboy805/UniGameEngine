using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Framework.Content.Pipeline.Builder;
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;
using UniGameEngine.Content.Serializers;
using UniGamePipeline;
using Debug = UniGameEngine.Debug;

namespace UniGameEditor.Content
{
    public sealed class ContentDatabase : ContentManager
    {
        // Events
        public event Action OnContentModified;
        public event Action<string> OnContentImported;              // Path
        public event Action<string> OnContentDeleted;               // Path
        public event Action<string, string> OnContentMoved;         // Old Path, New Path
        public event Action<string> OnContentFolderCreated;         // Path
        public event Action<string> OnContentFolderDeleted;         // Path

        // Private
        private PipelineManager pipelineManager = null;
        private FileSystemWatcher contentWatcher = null;
        private Dictionary<string, ContentMeta> contentMetas = new Dictionary<string, ContentMeta>();       // Guid, meta
        private Dictionary<string, string> contentPaths = new Dictionary<string, string>();                 // Path, guid
        private Dictionary<object, string> contentObjects = new Dictionary<object, string>();               // Content, guid
        private string projectDirectory = null;
        private string contentDirectory = null;
        private string intermediateDirectory = null;
        private string outputDirectory = null;

        public const string ContentMetaExtension = ".content";

        // Properties
        public ContentDatabase(string projectDirectory, string contentDirectory, string buildDirectory, IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            this.projectDirectory = projectDirectory;
            this.contentDirectory = contentDirectory;

            // Create directories
            intermediateDirectory = Path.Combine(buildDirectory, "obj");
            outputDirectory = Path.Combine(buildDirectory, "bin");

            // Create pipeline manager
            pipelineManager = new PipelineManager(contentDirectory, outputDirectory, intermediateDirectory);

            // Load custom pipeline
            pipelineManager.AddAssembly(typeof(UniPipeline).Assembly.Location);

            // Create watcher
            contentWatcher = new FileSystemWatcher(contentDirectory, "*");
            contentWatcher.EnableRaisingEvents = true;
            contentWatcher.IncludeSubdirectories = true;
            contentWatcher.Created += OnContentWatcherCreated;
            contentWatcher.Deleted += OnContentWatcherDeleted;

            // Set root directory
            RootDirectory = outputDirectory;
        }

        // Methods
        public string GetContentPath(string guid)
        {
            // Try to find meta
            ContentMeta meta;
            if(contentMetas.TryGetValue(guid, out meta) == true)
            {
                // Get the path
                return meta.ContentPath;
            }
            // Not found
            return null;
        }

        public string GetContentPath(object loadedContent)
        {
            // Check for null
            if (loadedContent == null)
                throw new ArgumentNullException(nameof(loadedContent));

            // Try to get guid
            string guid;
            string path;
            if(contentObjects.TryGetValue(loadedContent, out guid) == true
                && contentPaths.TryGetValue(guid, out path) == true)
            {
                // Get the path
                return path;
            }
            return null;
        }

        public string GetContentRelativePath(string contentFullPath)
        {
            // Get the new path
            string newPath = Path.GetRelativePath(contentDirectory, contentFullPath);

            // Convert separator
            newPath = newPath.Replace('\\', '/');
            return newPath;
        }

        public string GetContentGuid(string path)
        {
            // Make sure path is valid
            CheckContentPath(path);

            // Check for path registered
            string guid;
            if (contentPaths.TryGetValue(path, out guid) == true)
                return guid;

            // Not found
            return null;
        }

        public string GetContentGuid(object loadedContent)
        {
            // Check for null
            if (loadedContent == null)
                throw new ArgumentNullException(nameof(loadedContent));

            // Try to get guid
            string guid;
            if (contentObjects.TryGetValue(loadedContent, out guid) == true)
            {
                // Get the path
                return guid;
            }
            return null;
        }

        public ContentMeta GetContentMeta(string guid)
        {
            ContentMeta meta;
            contentMetas.TryGetValue(guid, out meta);

            return meta;
        }

        public Type GetContentType(string guid)
        {
            ContentMeta meta;
            if (contentMetas.TryGetValue(guid, out meta) == true)
            {
                // Load the asset to determine the type
                object loadedObj = Load<object>(meta.ContentPath);

                // Get type if loaded
                if (loadedObj != null)
                    return loadedObj.GetType();
            }

            return null;
        }

        public bool IsContentSupported(string path)
        {
            // Make sure path is valid
            CheckContentPath(path);

            // Try to get importer and processor
            try
            {
                // Try to find
                string importer = null;
                string processor = null;
                pipelineManager.ResolveImporterAndProcessor(path, ref importer, ref processor);

                // Check for valid
                return string.IsNullOrEmpty(importer) == false && string.IsNullOrEmpty(processor) == false;
            }
            catch { }
            return false;
        }

        public void RefreshContent()
        {
            // Process all content files
            foreach(string file in Directory.EnumerateFiles(contentDirectory, "*.*", SearchOption.AllDirectories)
                .Where(f => Path.GetExtension(f) != ContentMetaExtension))
            {
                // Get the relative path
                string contentPath = GetContentRelativePath(file);

                // Import the content
                ImportContent(contentPath);
            }

            // Process no longer existing content - create clone here so we can modify collections
            foreach(KeyValuePair<string, string> contentPath in contentPaths.ToDictionary())
            {
                // Get the content path
                string fullPath = Path.Combine(contentDirectory, contentPath.Key);

                // Check for content no longer available
                if(File.Exists(fullPath) == false)
                {
                    // Check for content file
                    if(File.Exists(fullPath + ContentMetaExtension) == true)
                        File.Decrypt(fullPath + ContentMetaExtension);

                    // Remove from cache
                    contentPaths.Remove(contentPath.Key);
                    contentMetas.Remove(contentPath.Value);

                    // Trigger event
                    UniEditor.DoEvent(OnContentModified);
                    UniEditor.DoEvent(OnContentDeleted, contentPath.Key);
                }
            }
        }

        public IEnumerable<string> SearchContent(string searchFolder = null, string search = null, SearchOption searchOption = SearchOption.TopDirectoryOnly, Type type = null)
        {
            // Check for empty folder
            if (string.IsNullOrEmpty(searchFolder) == true)
            {
                searchFolder = contentDirectory;
            }
            else
            {
                // Check directory
                CheckContentPath(searchFolder, false);
                searchFolder = Path.Combine(contentDirectory, searchFolder);
            }

            // Check for empty search
            if (string.IsNullOrEmpty(search) == true)
            {
                search = "*.*";
            }
            else if (Path.HasExtension(search) == false)
            {
                search += ".*";
            }

            // Process all content files
            foreach (string file in Directory.EnumerateFiles(searchFolder, search, searchOption)
                .Where(f => Path.GetExtension(f) != ContentMetaExtension))
            {
                // Get the relative path
                string contentPath = GetContentRelativePath(file);

                // Try to get guid
                string guid;
                if (contentPaths.TryGetValue(contentPath, out guid) == true)
                {
                    // Check for type
                    if(type != null)
                    {
                        // Get the meta
                        ContentMeta meta;
                        contentMetas.TryGetValue(guid, out meta);
                        
                        // Check for matching type
                        if (meta == null || type.IsAssignableFrom(GetContentType(guid)) == false)
                            continue;
                    }

                    // The content is a match
                    yield return guid;
                }
            }
        }

        public IEnumerable<string> SearchFolders(string searchFolder = null, string search = null, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            // Check for empty folder
            if (string.IsNullOrEmpty(searchFolder) == true)
            {
                searchFolder = contentDirectory;
            }
            else
            {
                // Check directory
                CheckContentPath(searchFolder, false);
                searchFolder = Path.Combine(contentDirectory, searchFolder);
            }

            // Check for empty search
            if (string.IsNullOrEmpty(search) == true)
            {
                search = "*";
            }

            // Get directories
            return Directory.EnumerateDirectories(searchFolder, search, searchOption);
        }

        public void ImportExternalContent(string externalPath, string importPath)
        {
            // Check for invalid
            if (string.IsNullOrEmpty(externalPath) == true)
                throw new ArgumentException(nameof(externalPath) + " cannot be null or empty");

            // Make sure path is valid
            CheckContentPath(importPath, false);

            // Check for directory
            if ((File.GetAttributes(externalPath) & FileAttributes.Directory) != 0)
            {
                // Check for exists
                if(Directory.Exists(externalPath) == false)
                    throw new DirectoryNotFoundException(externalPath);

                // Get the file name
                string directoryImportPath = Path.Combine(contentDirectory, importPath);

                // Create directory
                if (Directory.Exists(directoryImportPath) == false)
                {
                    // create the directory
                    Directory.CreateDirectory(directoryImportPath);

                    // Trigger event
                    UniEditor.DoEvent(OnContentModified);
                    UniEditor.DoEvent(OnContentFolderCreated, importPath);
                }

                // Process files
                foreach(string file in Directory.EnumerateFiles(externalPath, "*.*", SearchOption.TopDirectoryOnly))
                {
                    // Get import path
                    string fileImportPath = JoinContentPath(importPath, Path.GetFileName(file));

                    // Copy the file
                    File.Copy(file, Path.Combine(contentDirectory, fileImportPath), false);

                    // Import the content
                    ImportContent(fileImportPath);
                }

                // Process directories
                foreach(string directory in Directory.EnumerateDirectories(externalPath, "*", SearchOption.TopDirectoryOnly))
                {
                    // Get the file name
                    string childDirectoryImportPath = Path.Combine(directoryImportPath, new DirectoryInfo(directory).Name);

                    // Import the directory
                    ImportExternalContent(directory, childDirectoryImportPath);
                }
            }
            else
            {
                // Check for exists
                if (File.Exists(externalPath) == false)
                    throw new FileNotFoundException(externalPath);

                // Copy the file
                File.Copy(externalPath, Path.Combine(contentDirectory, importPath), false);

                // Import the content
                ImportContent(importPath);
            }
        }

        public void ImportContent(string path)
        {
            // Make sure path is valid
            CheckContentPath(path, false);

            // Check for directory
            if ((File.GetAttributes(Path.Combine(contentDirectory, path)) & FileAttributes.Directory) != 0)
            {
                // List the files inside
                foreach (string file in Directory.EnumerateFiles(Path.Combine(contentDirectory, path), "*.*", SearchOption.TopDirectoryOnly)
                    .Where(f => Path.GetExtension(f) != ContentMetaExtension))
                {
                    // Get the relative path
                    string relativePath = GetContentRelativePath(file);

                    // Delete the content
                    ImportContent(relativePath);
                }

                // Process directories
                foreach (string directory in Directory.EnumerateDirectories(Path.Combine(contentDirectory, path), "*", SearchOption.TopDirectoryOnly))
                {
                    // Get the relative path
                    string relativePath = GetContentRelativePath(directory);

                    // Delete the content
                    ImportContent(relativePath);
                }

                // Trigger event
                UniEditor.DoEvent(OnContentModified);
                UniEditor.DoEvent(OnContentFolderCreated, path);
            }
            // Must be a file
            else
            {
                // Check for supported
                if (IsContentSupported(path) == false)
                {
                    Debug.LogWarning("Content is not supported: " + path);
                    return;
                }

                // Get the content meta
                ContentMeta meta = CreateOrLoadContentMeta(path);

                // Update cache
                contentMetas[meta.Guid] = meta;
                contentPaths[path] = meta.Guid;

                // Unload existing content - so it can be loaded again later
                UnloadAsset(path);

                // Build the content
                BuildContent(path);

                // Trigger event
                UniEditor.DoEvent(OnContentModified);
                UniEditor.DoEvent(OnContentImported, path);
            }
        }

        public void DeleteContent(string path)
        {
            // Make sure path is valid
            CheckContentPath(path, false);

            // Check for directory
            if ((File.GetAttributes(Path.Combine(contentDirectory, path)) & FileAttributes.Directory) != 0)
            {
                // List the files inside
                foreach(string file in Directory.EnumerateFiles(Path.Combine(contentDirectory, path), "*.*", SearchOption.TopDirectoryOnly)
                    .Where(f => Path.GetExtension(f) != ContentMetaExtension))
                {
                    // Get the relative path
                    string relativePath = GetContentRelativePath(file);

                    // Delete the content
                    DeleteContent(relativePath);
                }

                // Process directories
                foreach(string directory in Directory.EnumerateDirectories(Path.Combine(contentDirectory, path), "*", SearchOption.TopDirectoryOnly))
                {
                    // Get the relative path
                    string relativePath = GetContentRelativePath(directory);

                    // Delete the content
                    DeleteContent(relativePath);
                }

                // Finally delete the directory
                Directory.Delete(Path.Combine(contentDirectory, path));

                // Trigger event
                UniEditor.DoEvent(OnContentModified);
                UniEditor.DoEvent(OnContentFolderDeleted, path);
            }
            // Must be a file
            else
            {
                // Unload the asset
                UnloadAsset(path);

                // Check for registered
                if (contentPaths.ContainsKey(path) == true)
                {
                    string contentFullPath = Path.Combine(contentDirectory, path);

                    // Clean intermediate and output content
                    pipelineManager.CleanContent(contentFullPath);

                    // Delete files
                    File.Delete(contentFullPath);
                    File.Delete(contentFullPath + ".content");
                }

                // Get the guid
                string guid;
                contentPaths.TryGetValue(path, out guid);

                // Update cache
                contentMetas.Remove(guid);
                contentPaths.Remove(path);

                // Trigger event
                UniEditor.DoEvent(OnContentModified);
                UniEditor.DoEvent(OnContentDeleted, path);
            }
        }

        public void MoveContent(string currentPath, string newPath)
        {
            // Make sure paths are valid
            CheckContentPath(currentPath, true, nameof(currentPath));
            CheckContentPath(newPath, false, nameof(newPath));


            // Trigger event
            UniEditor.DoEvent(OnContentModified);
            UniEditor.DoEvent(OnContentMoved, currentPath, newPath);            
        }

        public void OpenContent(string path)
        {
            // Make sure path is valid
            CheckContentPath(path, false);

            // Open process
            using (Process.Start("explorer", "\"" + Path.Combine(contentDirectory, path.Replace('/', '\\')) + "\"")) ;
        }

        public void BuildAllContent()
        {
            // Process call contents
            foreach(KeyValuePair<string, string> contentPath in contentPaths)
            {
                BuildContent(contentPath.Key);
            }
        }

        public void RebuildAllContent()
        {
            CleanAllContent();
            BuildAllContent();
        }

        public void CleanAllContent()
        {
            // Process call contents
            foreach (KeyValuePair<string, string> contentPath in contentPaths)
            {
                CleanContent(contentPath.Key);
            }

            // Delete directories
            Directory.Delete(intermediateDirectory, true);
            Directory.Delete(outputDirectory, true);
        }

        public void BuildContent(string path)
        {
            // Make sure path is valid
            CheckContentPath(path);

            try
            {
                // Build the content
                pipelineManager.BuildContent(Path.Combine(contentDirectory, path));
            }
            catch(InvalidContentException e)
            {
                Debug.LogException(e);
            }
        }

        public void RebuildContent(string path)
        {
            // Make sure path is valid
            CheckContentPath(path);

            try
            {
                // Clean and build the content
                pipelineManager.CleanContent(Path.Combine(contentDirectory, path));
                pipelineManager.BuildContent(Path.Combine(contentDirectory, path));
            }
            catch (InvalidContentException e)
            {
                Debug.LogException(e);
            }
        }

        public void CleanContent(string path)
        {
            // Make sure path is valid
            CheckContentPath(path);

            try
            {
                // Build the content
                pipelineManager.CleanContent(Path.Combine(contentDirectory, path));
            }
            catch (InvalidContentException e)
            {
                Debug.LogException(e);
            }
        }

        public override T Load<T>(string assetName)
        {
            // Get the full asset name
            string fullAssetName = assetName;

            // Strip extension
            if (Path.HasExtension(assetName) == true)
                assetName = Path.ChangeExtension(assetName, null);

            // Load the asset normally
            T result = base.Load<T>(assetName);

            // Check for loaded
            if (result != null)
            {
                // Get guid
                string guid;
                contentPaths.TryGetValue(fullAssetName, out guid);

                // Add to loaded
                contentObjects[result] = guid;
            }
            return result;
        }

        public override T LoadLocalized<T>(string assetName)
        {
            // Get the full asset name
            string fullAssetName = assetName;

            // Strip extension
            if (Path.HasExtension(assetName) == true)
                assetName = Path.ChangeExtension(assetName, null);

            // Load the asset normally
            T result = base.LoadLocalized<T>(assetName);

            // Check for loaded
            if (result != null)
            {
                // Get guid
                string guid;
                contentPaths.TryGetValue(fullAssetName, out guid);

                // Add to loaded
                contentObjects[result] = guid;
            }
            return result;
        }

        private async void OnContentWatcherCreated(object sender, FileSystemEventArgs e)
        {
            // Get the relative path
            string relativePath = GetContentRelativePath(e.FullPath);

            // Wait for time for IO to update
            await Task.Delay(25);

            // Import the content
            ImportContent(relativePath);
        }

        private void OnContentWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            // Do a full refresh to scan for deleted content
            RefreshContent();
        }

        private ContentMeta CreateOrLoadContentMeta(string path)
        {
            // Check the path
            CheckContentPath(path);

            // Check for exists
            bool exists = File.Exists(Path.Combine(contentDirectory, path + ContentMetaExtension));

            // Get guid
            string guid;
            ContentMeta meta = null;
            if (contentPaths.TryGetValue(path, out guid) == false 
                || contentMetas.TryGetValue(guid, out meta) == false
                || exists == false)
            {
                if (exists == false)
                {
                    // Create new meta
                    meta = ContentMeta.CreateForFile(pipelineManager, path);

                    // Save meta to disk
                    File.WriteAllText(Path.Combine(contentDirectory, path + ContentMetaExtension), 
                        Serializer.SerializeJson(meta));
                }
            }

            // Load existing meta
            if(meta == null)
                meta = Serializer.DeserializeJson<ContentMeta>(File.ReadAllText(Path.Combine(contentDirectory, path + ContentMetaExtension)));

            // Update path
            meta.UpdateContentPath(path);

            // Ensure guid
            meta.EnsureGuid();
            return meta;
        }

        private void CheckContentPath(string path, bool checkExists = true, string hintName = null)
        {
            if (hintName == null)
                hintName = nameof(path);

            // Make sure path is valid
            CheckContentPathValid(path, hintName);

            if (checkExists == true)
            {
                // Check for exists
                string fullPath = Path.Combine(contentDirectory, path);
                if (File.Exists(fullPath) == false)
                    throw new FileNotFoundException(hintName + " could not be found: " + fullPath);
            }
        }

        internal static void CheckContentPathValid(string path, string hintName = null)
        {
            if (hintName == null)
                hintName = nameof(path);

            // Check for null or empty
            if (string.IsNullOrEmpty(path) == true)
                throw new ArgumentException(hintName + " cannot be null or empty");

            // Check for invalid backslash
            if (path.Contains('\\') == true)
                throw new FormatException(hintName + " should not use '\\' separator: Use '/' instead");

            // Check for rooted
            if (Path.IsPathRooted(path) == true)
                throw new ArgumentException(hintName + " must be relative to the content folder");
        }

        internal static string JoinContentPath(string a, string b)
        {
            return Path.Combine(a, b).Replace('\\', '/');
        }

        internal static string JoinContentPath(string a, string b, string c)
        {
            return Path.Combine(a, b, c).Replace('\\', '/');
        }

        internal static string JoinContentPath(string a, string b, string c, string d)
        {
            return Path.Combine(a, b, c, d).Replace('\\', '/');
        }
    }
}
