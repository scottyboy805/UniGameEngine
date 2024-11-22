using System.Reflection;
using UniGameEditor.Property;
using UniGameEditor.UI;
using UniGameEngine;

namespace UniGameEditor.Content
{
    public abstract class ContentEditor
    {
        // Private
        private static readonly Dictionary<Type, ContentEditor> specificContentEditors = new Dictionary<Type, ContentEditor>();
        private static readonly List<(Type, ContentEditor)> derivedContentEditors = new List<(Type, ContentEditor)>();

        // Internal
        internal UniEditor editor = null;
        internal EditorLayoutControl rootControl = null;
        internal SerializedContent content = null;

        // Properties
        public UniEditor Editor
        {
            get { return editor; }
        }

        public SerializedContent Content
        {
            get { return content; }
        }

        public EditorLayoutControl RootControl
        {
            get { return rootControl; }
        }

        public IReadOnlyList<SerializedProperty> Properties
        {
            get
            {
                if (content.Properties != null)
                    return content.Properties;

                return Array.Empty<SerializedProperty>();
            }
        }

        // Methods
        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        public void CreateContent(EditorLayoutControl rootControl, SerializedContent content)
        {
            this.rootControl = rootControl;
            this.content = content;

            // Show the property
            try
            {
                OnShow();
            }
            catch (Exception e)
            {
                Debug.LogException(e);

#if DEBUG
                throw;
#endif
            }
        }

        public static ContentEditor ForType<T>()
        {
            return ForType(typeof(T));
        }

        public static ContentEditor ForType(Type type)
        {
            // Check for null
            if (type == null)
                return null;

            ContentEditor contentEditor = null;

            // Check for specified
            if (specificContentEditors.TryGetValue(type, out contentEditor) == false)
            {
                // Try to get derived
                foreach ((Type, ContentEditor) derivedContentEditor in derivedContentEditors)
                {
                    // Check for found
                    if (derivedContentEditor.Item1.IsAssignableFrom(type) == true)
                    {
                        contentEditor = derivedContentEditor.Item2;
                        break;
                    }
                }
            }

            // Get property editor
            return contentEditor;
        }

        internal static void InitializePropertyEditors(UniEditor editor)
        {
            // Get this assembly name
            Assembly thisAsm = typeof(UniEditor).Assembly;
            AssemblyName thisName = thisAsm.GetName();

            // Process all assemblies
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Check if we should scan
                bool shouldCheckAssembly = thisAsm == asm;

                // Check for referenced
                if (shouldCheckAssembly == false)
                {
                    // Get references
                    AssemblyName[] referenceNames = asm.GetReferencedAssemblies();

                    // Check for assembly referenced
                    foreach (AssemblyName referenceName in referenceNames)
                    {
                        if (referenceName.FullName == thisName.FullName)
                        {
                            shouldCheckAssembly = true;
                            break;
                        }
                    }
                }

                // Check for skip
                if (shouldCheckAssembly == false)
                    continue;

                Type[] checkTypes = null;

                try
                {
                    // Try to load all types
                    checkTypes = asm.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    Debug.LogException(e);

                    // Get all types that could be loaded
                    checkTypes = e.Types.Where(t => t != null)
                        .ToArray();
                }

                // Check all types
                foreach (Type type in checkTypes)
                {
                    // Check for attribute
                    if (type.IsDefined(typeof(ContentEditorForAttribute)) == true)
                    {
                        // Get the attribute
                        IEnumerable<ContentEditorForAttribute> attributes = type.GetCustomAttributes<ContentEditorForAttribute>();

                        // Check for type
                        if (typeof(ContentEditor).IsAssignableFrom(type) == false)
                        {
                            Debug.LogErrorF("Content editor '{0}' must derive from '{1}'", type, typeof(ContentEditor));
                            break;
                        }

                        // Process all
                        foreach (ContentEditorForAttribute attrib in attributes)
                        {
                            // Create instance of editor
                            ContentEditor contentEditor = (ContentEditor)Activator.CreateInstance(type);
                            contentEditor.editor = editor;

                            // Check for specific
                            if (attrib.ForDerivedTypes == false)
                            {
                                // Check for already added
                                if (specificContentEditors.ContainsKey(attrib.ForType) == true)
                                {
                                    Debug.LogError("A content editor already exists for type: " + attrib.ForType);
                                    continue;
                                }

                                specificContentEditors[attrib.ForType] = contentEditor;
                            }
                            // Add derived
                            else
                            {
                                // Add to derived
                                derivedContentEditors.Add((attrib.ForType, contentEditor));
                            }
                        }
                    }
                }
            }
        }
    }
}
