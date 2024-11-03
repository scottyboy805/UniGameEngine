using Microsoft.Xna.Framework.Content.Pipeline;
using UniGameEngine;

namespace UniGamePipeline.Prefab
{
    [ContentProcessor(DisplayName = "Prefab Json Processor - UniGameEngine")]
    internal sealed class PrefabProcessor : ContentProcessor<GameElementContentItem<GameObject>, GameElementContentItem<GameObject>>
    {
        public override GameElementContentItem<GameObject> Process(GameElementContentItem<GameObject> input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
