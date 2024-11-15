using Microsoft.Xna.Framework.Content.Pipeline;

namespace UniGamePipeline.Font
{
    [ContentProcessor(DisplayName = "Font Processor - UniGameEngine")]
    internal sealed class FontProcessor : ContentProcessor<FontContentItem, FontContentItem>
    {
        // Methods
        public override FontContentItem Process(FontContentItem input, ContentProcessorContext context)
        {
            // Try to initialize the font
            input.InitializeFont();

            // Get input bytes
            return input;
        }
    }
}
