using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class GameEditorWindow : EditorWindow
    {
        // Constructor
        public GameEditorWindow()
        {
            icon = EditorIcon.FindIcon("Game");
            title = "Game";
        }
    }
}
