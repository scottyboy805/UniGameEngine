using Microsoft.Xna.Framework.Content;
using UniGameEngine.Content.Serializers;
using UniGameEngine.Scene;

namespace UniGameEngine.Content.Reader
{
    internal sealed class SceneContentReader : ContentTypeReader<GameScene>
    {
        // Methods
        protected override GameScene Read(ContentReader input, GameScene existingInstance)
        {
            // Create scene reader
            using (ContentSerializedReader serializedReader = new ContentSerializedReader(input))
            {
                // Deserialize
                GameScene scene = Serializer.Deserialize<GameScene>(serializedReader);

                // Update content path
                scene.ContentPath = input.AssetName;

                return scene;
            }
        }
    }
}
