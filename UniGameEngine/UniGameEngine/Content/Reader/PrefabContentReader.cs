using Microsoft.Xna.Framework.Content;
using UniGameEngine.Content.Serializers;

namespace UniGameEngine.Content.Reader
{
    internal sealed class PrefabContentReader : ContentTypeReader<GameObject>
    {
        // Methods
        protected override GameObject Read(ContentReader input, GameObject existingInstance)
        {
            // Create prefab reader
            using (ContentSerializedReader serializedReader = new ContentSerializedReader(input))
            {
                // Deserialize
                GameObject prefab = Serializer.Deserialize<GameObject>(serializedReader);

                // Update content path
                prefab.ContentPath = input.AssetName;

                return prefab;
            }
        }
    }
}
