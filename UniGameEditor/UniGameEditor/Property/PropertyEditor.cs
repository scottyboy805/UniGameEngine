using System.Reflection;
using UniGameEditor.UI;
using UniGameEngine;

namespace UniGameEditor.Property
{
    public abstract class PropertyEditor
    {
        // Private
        private static readonly Dictionary<Type, PropertyEditor> specificPropertyEditors = new Dictionary<Type, PropertyEditor>();
        private static readonly List<(Type, PropertyEditor)> derivedPropertyEditors = new List<(Type, PropertyEditor)>();

        // Internal
        internal UniEditor editor = null;
        internal EditorLayoutControl rootControl = null;
        internal SerializedProperty property = null;        

        // Properties
        public UniEditor Editor
        {
            get { return editor; }
        }

        public SerializedProperty Property
        {
            get { return property; }
        }

        public EditorLayoutControl RootControl
        {
            get { return rootControl; }
        }

        // Methods
        protected internal virtual void OnShow() { }

        protected internal virtual void OnHide() { }

        public void CreateProperty(EditorLayoutControl rootControl, SerializedProperty property)
        {
            this.rootControl = rootControl;
            this.property = property;

            // Show the property
            try
            {
                OnShow();
            }
            catch(Exception e)
            {
                Debug.LogException(e);

#if DEBUG
                throw;
#endif
            }
        }

        public static PropertyEditor ForType<T>()
        {
            return ForType(typeof(T));
        }

        public static PropertyEditor ForType(Type type)
        {
            // Check for null
            if (type == null)
                return null;

            PropertyEditor propertyEditor = null;

            // Check for specified
            if(specificPropertyEditors.TryGetValue(type, out propertyEditor) == false)
            {
                // Try to get derived
                foreach((Type, PropertyEditor) derivedPropertyEditor in derivedPropertyEditors)
                {
                    // Check for found
                    if(derivedPropertyEditor.Item1.IsAssignableFrom(type) == true)
                    {
                        propertyEditor = derivedPropertyEditor.Item2;
                        break;
                    }
                }
            }

            // Get property editor
            return propertyEditor;
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
                    if (type.IsDefined(typeof(PropertyEditorForAttribute)) == true)
                    {
                        // Get the attribute
                        PropertyEditorForAttribute attrib = type.GetCustomAttribute<PropertyEditorForAttribute>();

                        // Check for type
                        if(typeof(PropertyEditor).IsAssignableFrom(type) == false)
                        {
                            Debug.LogErrorF("Property editor '{0}' must derive from '{1}'", type, typeof(PropertyEditor));
                            break;
                        }

                        // Create instance of editor
                        PropertyEditor propertyEditor = (PropertyEditor)Activator.CreateInstance(type);
                        propertyEditor.editor = editor;

                        // Check for specific
                        if(attrib.ForDerivedTypes == false)
                        {
                            // Check for already added
                            if(specificPropertyEditors.ContainsKey(attrib.ForType) == true)
                            {
                                Debug.LogError("A property editor already exists for type: " + attrib.ForType);
                                continue;
                            }

                            specificPropertyEditors[attrib.ForType] = propertyEditor;
                        }
                        // Add derived
                        else
                        {
                            // Add to derived
                            derivedPropertyEditors.Add((attrib.ForType, propertyEditor));
                        }
                    }
                }
            }
        }
    }
}
