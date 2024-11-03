using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using UniGameEngine;
using UniGameEngine.Content;
using UniGameEngine.Content.Serializers;

namespace UniGamePipeline.Prefab
{
    [ContentTypeWriter]
    internal sealed class PrefabContentWriter : ContentTypeWriter<PrefabContentItem>
    {
        // Methods
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(PrefabContentReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, PrefabContentItem value)
        {
            // Create write
            ContentSerializedWriter serializedWriter = new ContentSerializedWriter(output);

            // Write the prefab
            Serializer.Serialize(serializedWriter, value.ImportedObject);
        }
    }
}
