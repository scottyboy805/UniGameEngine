using UniGameEngine;
using UniGameEngine.Scene;
using UniGameEngine.UI;
using UniGameEngine.UI.Events;

namespace UniGameEditor
{
    internal sealed class UniEditorGameInstance : UniGame
    {
        // Private
        private bool isPlaying = false;

        // Properties
        public override bool IsEditor => true;
        public override bool IsPlaying => isPlaying;

        // Methods
        protected override void Initialize()
        {
            // Call base
            base.Initialize();

            GameScene scene = new GameScene("MyScene");
            UICanvas canvas = scene.CreateObject<UICanvas>("Canvas", typeof(UIEventDispatcher));
            Image.Create(canvas.GameObject);

            scene.Activate();
        }
    }
}
