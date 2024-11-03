using Microsoft.Xna.Framework.Content.Pipeline;

namespace UniGamePipeline.Prefab
{
    [ContentProcessor(DisplayName = "Prefab Json Processor - UniGameEngine")]
    internal sealed class PrefabProcessor : ContentProcessor<PrefabContentItem, PrefabContentItem>
    {
        public override PrefabContentItem Process(PrefabContentItem input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
