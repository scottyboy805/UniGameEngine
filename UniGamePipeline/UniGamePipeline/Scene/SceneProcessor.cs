using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using UniGameEngine.Scene;

namespace UniGamePipeline.Scene
{
    [ContentProcessor(DisplayName = "Scene Json Processor - UniGameEngine")]
    internal sealed class SceneProcessor : ContentProcessor<GameElementContentItem<GameScene>, GameElementContentItem<GameScene>>
    {
        // Methods
        public override GameElementContentItem<GameScene> Process(GameElementContentItem<GameScene> input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
