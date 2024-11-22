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
            SerializedProperty componentsProperty = Content.FindSerializedName("Components");

            // Create name layout
            EditorLayoutControl nameLayout = RootControl.AddDirectionalLayout(EditorLayoutDirection.Horizontal);

            // Enabled
            {
                // Get enabled value
                bool enabled, isMixed;
                enabledProperty.GetValue(out enabled, out isMixed);

                // Add enabled toggle
                EditorToggle enabledToggle = nameLayout.AddToggle(enabled);
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
                EditorToggle staticToggle = nameLayout.AddToggle(isStatic);
                staticToggle.Content.AddLabel("Static");
            }



            // Get transform
            {
                // Create transform foldout
                EditorFoldout transformFoldout = CreateComponentHeader(transformProperty);

                // Create serialized content
                SerializedContent transformContent = transformProperty.CreateContent();

                // Create drawer
                ContentEditor transformEditor = ContentEditor.ForType(transformContent.Contract.ContractType);

                // Initialize drawer
                transformEditor.CreateContent(transformFoldout, transformContent);
            }


            // Components
            foreach(SerializedProperty childProperty in componentsProperty.Children)
            {
                // Create component header
                EditorFoldout componentFoldout = CreateComponentHeader(childProperty);

                // Create the serialized content
                SerializedContent componentContent = childProperty.CreateContent();

                // Create drawer
                ContentEditor componentEditor = ContentEditor.ForType(componentContent.Contract.ContractType);

                // Initialize drawer
                componentEditor.CreateContent(componentFoldout, componentContent);
            }
        }

        private EditorFoldout CreateComponentHeader(SerializedProperty property)
        {
            // Get the name
            string displayName = property.DisplayName;

            // Check for array element
            if(property.IsArrayElement == true)
            {
                // Get the value
                Component componentInstance;
                property.GetValue(out componentInstance, out _);

                // Get the component name
                displayName = componentInstance.Name;
            }

            // Get the component type

            // Create the foldout
            EditorFoldout componentFoldout = RootControl.AddFoldoutLayout();
            componentFoldout.Header.AddLabel(displayName);

            return componentFoldout;
        }
    }
}
