
namespace UniGameEditor.Windows
{
    internal sealed class SceneEditorWindow : EditorWindow
    {

        // Constructor
        public SceneEditorWindow()
        {
            title = "Scene";
        }

        // Methods
        protected internal override void OnOpenWindow()
        {
            RootControl.AddRenderView(null);
        }
    }
}
