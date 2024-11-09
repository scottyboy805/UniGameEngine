using Microsoft.Xna.Framework;
using UniGameEditor.UI;

namespace UniGameEditor.Property
{
    [PropertyEditorFor(typeof(Vector3))]
    internal sealed class Vector3PropertyEditor : PropertyEditor
    {
        // Private
        private SerializedProperty xProperty = null;
        private SerializedProperty yProperty = null;
        private SerializedProperty zProperty = null;

        // Methods
        protected internal override void OnShow()
        {
            // Find the component properties
            xProperty = Property.FindPropertyName(nameof(Vector3.X));
            yProperty = Property.FindPropertyName(nameof(Vector3.Y));
            zProperty = Property.FindPropertyName(nameof(Vector3.Z));

            // Add layout
            EditorLayoutControl layout = RootControl.AddHorizontalLayout();

            // Add property label
            EditorPropertyLabel propertyLabel = layout.AddPropertyLabel(Property);
            propertyLabel.Width = 120;

            // Get remaining width
            float elementWidth = (RootControl.Width - propertyLabel.Width) / 3f;

            
            // X
            {
                // Draw X element
                EditorLayoutControl xLayout = layout.AddHorizontalLayout();
                //xLayout.Width = elementWidth;

                // X property
                EditorPropertyLabel xPropertyLabel = xLayout.AddPropertyLabel(xProperty);

                // X value
                float xValue;
                bool xMixed;
                xProperty.GetValue(out xValue, out xMixed);

                // X input
                EditorNumberInput xPropertyInput = xLayout.AddNumberInput(xValue);
            }

            // Y
            {
                // Draw y element
                EditorLayoutControl yLayout = layout.AddHorizontalLayout();
                //yLayout.Width = elementWidth;

                // Y property
                EditorPropertyLabel yPropertyLabel = yLayout.AddPropertyLabel(yProperty);

                // Y value
                float yValue;
                bool yMixed;
                yProperty.GetValue(out yValue, out yMixed);

                // Y input
                EditorNumberInput yPropertyInput = yLayout.AddNumberInput(yValue);
            }

            // Z
            {
                // Draw z element
                EditorLayoutControl zLayout = layout.AddHorizontalLayout();
                //zLayout.Width = elementWidth;

                // Z property
                EditorPropertyLabel zPropertyLabel = zLayout.AddPropertyLabel(zProperty);

                // Z value
                float zValue;
                bool zMixed;
                zProperty.GetValue(out zValue, out zMixed);

                // Z input
                EditorNumberInput zPropertyInput = zLayout.AddNumberInput(zValue);
            }
        }
    }
}
