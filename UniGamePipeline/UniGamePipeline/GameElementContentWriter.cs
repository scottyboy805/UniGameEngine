using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using UniGameEngine;
using UniGameEngine.Content.Serializers;

namespace UniGamePipeline
{
    internal abstract class GameElementContentWriter<TAsset, TReader> : ContentTypeWriter<GameElementContentItem<TAsset>> where TAsset : GameElement where TReader : ContentTypeReader
    {
        // Methods
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, GameElementContentItem<TAsset> value)
        {
            // Create write
            ContentSerializedWriter serializedWriter = new ContentSerializedWriter(output);

            // Write the prefab
            Serializer.Serialize(serializedWriter, value.ImportedElement);
        }
    }
}
