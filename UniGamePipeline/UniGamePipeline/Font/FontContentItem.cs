using FontStashSharp;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace UniGamePipeline.Font
{
    internal sealed class FontContentItem : ContentItem
    {
        // Private
        private byte[] importedBytes = null;
        private FontSystem importedFont = null;      
        
        // Properties
        public byte[] ImportedBytes
        {
            get { return importedBytes; }
        }

        public FontSystem ImportedFont
        {
            get { return importedFont; }
        }

        // Constructor
        public FontContentItem(byte[] importedBytes)
        {
            this.importedBytes = importedBytes;
        }

        // Methods
        public void InitializeFont()
        {
            importedFont = new FontSystem();
            importedFont.AddFont(importedBytes);
        }
    }
}
