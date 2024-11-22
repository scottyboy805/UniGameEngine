
using Microsoft.Xna.Framework;
using UniGameEditor.Content;
using UniGameEditor.Property;
using UniGameEditor.UI;
using UniGameEngine;
using UniGameEngine.Content.Contract;

namespace UniGameEditor.Windows
{
    class TestObject
    {
        public Vector3 MyVecProperty = new Vector3(1, 2, 3);
        public int number;
        public string value;
    }


    internal sealed class PropertiesEditorWindow : EditorWindow
    {
        // Private
        private EditorLayoutControl mainControl = null;

        // Constructor
        public PropertiesEditorWindow()
        {
            icon = EditorIcon.FindIcon("Properties");
            title = "Properties";
        }

        // Methods
        protected internal override void OnShow()
        {
            // Add selection listener
            Editor.Selection.OnSelectionChanged += RebuildProperties;

            // Rebuild on show
            RebuildProperties();
        }

        private void RebuildProperties()
        {
            // Remove existing
            if (mainControl != null)
                RootControl.Clear();

            // Create main control
            EditorLayoutControl scroll = RootControl.AddScrollLayout(false);
            mainControl = scroll.AddDirectionalLayout(EditorLayoutDirection.Vertical);

            // Check for any selection
            if (Editor.Selection.HasAnySelection == true)
            {
                // Get selection type
                Type selectionType = Editor.Selection.SelectedType;

                // Get the meta for the content
                ContentMeta meta = null;
                if (Editor.Selection.IsSingleSelection == true)
                {
                    // Get main selected object
                    object mainSelection = Editor.Selection.GetMainSelected();

                    // Get the guid
                    string guid = Editor.ContentDatabase.GetContentGuid(mainSelection);

                    // Get meta if content is imported
                    if (guid != null)
                        meta = Editor.ContentDatabase.GetContentMeta(guid);
                }

                // Create editor
                SerializedContent content = new SerializedContent(selectionType, Editor.Selection.GetSelected().ToArray(), meta);

                // Create the editor
                ContentEditor editor = ContentEditor.ForType(selectionType);

                // Initialize editor
                if(editor != null)
                    editor.CreateContent(mainControl, content);
            }
            else
            {
                mainControl.AddLabel("No Selection");
            }
        }
    }
}
