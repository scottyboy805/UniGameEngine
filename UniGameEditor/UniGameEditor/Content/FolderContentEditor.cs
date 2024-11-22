
using UniGameEditor.UI;

namespace UniGameEditor.Content
{
    [ContentEditorFor(typeof(FolderObject))]
    internal sealed class FolderContentEditor : ContentEditor
    {
        // Methods
        protected override void OnShow()
        {
            // Get folder icon
            EditorIcon folderIcon = EditorIcon.FindIcon("FolderNormal");

            // Find the property
            SerializedProperty property = Content.FindPropertyName(nameof(FolderObject.projectRelativePath));

            // Get the folder name
            string folderName;
            property.GetValue(out folderName, out _);

            EditorLayoutControl layout = RootControl.AddDirectionalLayout(EditorLayoutDirection.Horizontal);

            // Add icon
            layout.AddImage(folderIcon);

            // Add main label
            EditorLabel mainLabel = layout.AddLabel(folderName);
        }
    }
}
