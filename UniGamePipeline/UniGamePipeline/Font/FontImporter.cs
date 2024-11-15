using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

namespace UniGamePipeline.Font
{
    [ContentImporter(".ttf", DisplayName = "Font Importer - UniGameEngine", DefaultProcessor = nameof(FontProcessor))]
    internal sealed class FontImporter : ContentImporter<FontContentItem>
    {
        // Methods
        public override FontContentItem Import(string filename, ContentImporterContext context)
        {
            // Read all bytes
            byte[] fontBytes = File.ReadAllBytes(filename);

            // Create content
            return new FontContentItem(fontBytes);
        }
    }
}
