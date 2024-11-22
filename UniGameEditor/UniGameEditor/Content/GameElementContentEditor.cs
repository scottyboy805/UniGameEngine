using UniGameEditor.Property;
using UniGameEditor.UI;
using UniGameEngine;

namespace UniGameEditor.Content
{
    [ContentEditorFor(typeof(GameElement), true)]
    internal sealed class GameElementContentEditor : ContentEditor
    {
        // Methods
        protected internal override void OnShow()
        {
            // Add element type
            EditorLayoutControl layout = RootControl.AddDirectionalLayout(EditorLayoutDirection.Horizontal);

            // Add type field
            layout.AddLabel(Content.DisplayName);


            // Get all properties
            foreach (SerializedProperty property in Content.Properties)
            {
                // Get type
                Type propertyType = property.Property.PropertyType;

                // Get drawer
                PropertyEditor editor = PropertyEditor.ForType(propertyType);

                // Create drawer
                editor.CreateProperty(RootControl, property);
            }
        }
    }
}
