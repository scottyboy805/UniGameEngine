﻿using Microsoft.Xna.Framework;
using UniGameEditor.UI;

namespace UniGameEditor.Property
{
    [PropertyEditorFor(typeof(Vector2))]
    internal sealed class Vector2PropertyEditor : PropertyEditor
    {
        // Private
        private SerializedProperty xProperty = null;
        private SerializedProperty yProperty = null;

        // Methods
        protected internal override void OnShow()
        {
            // Find the component properties
            xProperty = Property.FindPropertyName(nameof(Vector2.X));
            yProperty = Property.FindPropertyName(nameof(Vector2.Y));

            // Add layout
            EditorLayoutControl layout = RootControl.AddHorizontalLayout();

            // Add property label
            EditorPropertyLabel propertyLabel = layout.AddPropertyLabel(Property);
            propertyLabel.Width = EditorControl.PropertiesLabelWidth;

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
        }
    }

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
            propertyLabel.Width = EditorControl.PropertiesLabelWidth;

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

    [PropertyEditorFor(typeof(Vector4))]
    internal sealed class Vector4PropertyEditor : PropertyEditor
    {
        // Private
        private SerializedProperty xProperty = null;
        private SerializedProperty yProperty = null;
        private SerializedProperty zProperty = null;
        private SerializedProperty wProperty = null;

        // Methods
        protected internal override void OnShow()
        {
            // Find the component properties
            xProperty = Property.FindPropertyName(nameof(Vector4.X));
            yProperty = Property.FindPropertyName(nameof(Vector4.Y));
            zProperty = Property.FindPropertyName(nameof(Vector4.Z));
            wProperty = Property.FindPropertyName(nameof(Vector4.W));

            // Add layout
            EditorLayoutControl layout = RootControl.AddHorizontalLayout();

            // Add property label
            EditorPropertyLabel propertyLabel = layout.AddPropertyLabel(Property);
            propertyLabel.Width = EditorControl.PropertiesLabelWidth;

            // Get remaining width
            float elementWidth = (RootControl.Width - propertyLabel.Width) / 4f;


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

            // W
            {
                // Draw z element
                EditorLayoutControl wLayout = layout.AddHorizontalLayout();
                //zLayout.Width = elementWidth;

                // Z property
                EditorPropertyLabel wPropertyLabel = wLayout.AddPropertyLabel(wProperty);

                // Z value
                float wValue;
                bool wMixed;
                wProperty.GetValue(out wValue, out wMixed);

                // Z input
                EditorNumberInput wPropertyInput = wLayout.AddNumberInput(wValue);
            }
        }
    }
}
