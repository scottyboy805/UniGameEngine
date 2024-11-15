using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class SceneEditorWindow : EditorWindow
    {

        // Constructor
        public SceneEditorWindow()
        {
            icon = EditorIcon.FindIcon("Scene");
            title = "Scene";
        }

        // Methods
        protected internal override void OnShow()
        {
            RootControl.AddRenderView(editor.GameInstance).Height = Height;
        }

        private void RenderScene()
        {
            //editor.GameInstance.RunOneFrame();
        }
    }
}
