using FontStashSharp;
using Microsoft.Xna.Framework.Content;

namespace UniGameEngine.Content.Reader
{
    internal sealed class FontContentReader : ContentTypeReader<FontSystem>
    {
        // Methods
        protected override FontSystem Read(ContentReader input, FontSystem existingInstance)
        {
            // Create font system
            FontSystem fontSystem = new FontSystem();

            // Read size
            int size = input.ReadInt32();

            // Read font
            fontSystem.AddFont(input.ReadBytes(size));

            // Get the font system
            return fontSystem;
        }
    }
}
