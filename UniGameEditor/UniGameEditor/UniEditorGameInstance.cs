using UniGameEngine;

namespace UniGameEditor
{
    internal sealed class UniEditorGameInstance : UniGame
    {
        // Private
        private bool isPlaying = false;

        // Properties
        public override bool IsEditor => true;
        public override bool IsPlaying => isPlaying;
    }
}
