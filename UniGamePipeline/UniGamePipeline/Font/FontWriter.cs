using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using UniGameEngine.Content.Reader;

namespace UniGamePipeline.Font
{
    [ContentTypeWriter]
    internal sealed class FontWriter : ContentTypeWriter<FontContentItem>
    {
        // Methods
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(FontContentReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, FontContentItem value)
        {
            // Write size
            output.Write(value.ImportedBytes.Length);

            // Write bytes
            output.Write(value.ImportedBytes);
        }
    }
}
