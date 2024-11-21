
using UniGameEditor.UI;

namespace UniGameEditor.Content
{
    [ContentEditorFor(typeof(FolderObject))]
    internal sealed class FolderContentEditor : ContentEditor
    {
        // Methods
        protected internal override void OnShow()
        {
            // Get folder icon
            EditorIcon folderIcon = EditorIcon.FindIcon("FolderNormal");

            // Find the property
            SerializedProperty property = Content.FindPropertyName(nameof(FolderObject.projectRelativePath));

            // Get the folder name
            string folderName;
            property.GetValue(out folderName, out _);

            // Add main label
            EditorLabel mainLabel = RootControl.AddLabel(folderName);
            mainLabel.Icon = folderIcon;
        }
    }
}
