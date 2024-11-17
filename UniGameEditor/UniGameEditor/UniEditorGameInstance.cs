using UniGameEngine;
using UniGameEngine.Graphics;
using UniGameEngine.Scene;
using UniGameEngine.UI;
using UniGameEngine.UI.Events;

namespace UniGameEditor
{
    internal sealed class UniEditorGameInstance : UniGame
    {
        // Events
        public event Action OnInitialized;

        // Private
        private bool isPlaying = false;

        // Properties
        public override bool IsEditor => true;
        public override bool IsPlaying => isPlaying;

        // Constructor
        public UniEditorGameInstance()
        {
            current = this;
        }

        // Methods
        protected override void Initialize()
        {
            // Call base
            base.Initialize();

            // Trigger event
            if (OnInitialized != null)
                OnInitialized();
        }

        protected override void LoadContent()
        {
        }

        public void LoadEditorContent()
        {
            base.LoadContent();
        }
    }
}
