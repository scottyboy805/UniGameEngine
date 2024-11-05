using UniGameEditor.UI;
using UniGameEngine;

namespace UniGameEditor.Content
{
    [ContentEditorFor(typeof(GameObject))]
    internal sealed class GameObjectContentEditor : ContentEditor
    {
        // Methods
        protected internal override void OnShow()
        {
            // Get properties
            SerializedProperty enabledProperty = Content.FindSerializedName(nameof(GameObject.Enabled));
            SerializedProperty nameProperty = Content.FindSerializedName(nameof(GameObject.Name));
            SerializedProperty staticProperty = Content.FindSerializedName(nameof(GameObject.IsStatic));

            SerializedProperty transformProperty = Content.FindSerializedName(nameof(GameObject.Transform));


            // Create name layout
            EditorLayoutControl nameLayout = RootControl.AddHorizontalLayout();

            // Enabled
            {
                // Get enabled value
                bool enabled, isMixed;
                enabledProperty.GetValue(out enabled, out isMixed);

                // Add enabled toggle
                EditorToggle enabledToggle = nameLayout.AddToggle(null, enabled);
            }

            // Name
            {
                // Get name value
                string name;
                bool isMixed;
                nameProperty.GetValue(out name, out isMixed);

                // Add name option
                EditorInput nameInput = nameLayout.AddInput(name);
            }

            // Static
            {
                // Get static value
                bool isStatic, isMixed;
                staticProperty.GetValue(out isStatic, out isMixed);

                // Add static toggle
                EditorToggle staticToggle = nameLayout.AddToggle("Static", isStatic);
            }



            // Get transform
            {
                // Create transform foldout
                EditorFoldout transformFoldout = CreateComponentHeader(transformProperty);

                // Create serialized content
                SerializedContent transformContent = transformProperty.CreateContent();

                // Create drawer
                ContentEditor transformEditor = ContentEditor.ForType(transformContent.Contract.ContractType);

                // Create drawer
                transformEditor.CreateContent(transformFoldout, transformContent);
            }
        }

        private EditorFoldout CreateComponentHeader(SerializedProperty property)
        {
            // Get the component type

            // Create the foldout
            EditorFoldout componentFoldout = RootControl.AddFoldoutLayout(property.DisplayName);

            return componentFoldout;
        }
    }
}
