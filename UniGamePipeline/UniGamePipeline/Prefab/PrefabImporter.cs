using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using System.IO;
using UniGameEngine;
using UniGameEngine.Content.Serializers;

namespace UniGamePipeline.Prefab
{
    [ContentImporter(".jprefab", DisplayName = "Prefab Json Importer - UniGameEngine", DefaultProcessor = nameof(PrefabProcessor))]
    internal sealed class PrefabImporter : ContentImporter<PrefabContentItem>
    {
        // Methods
        public override PrefabContentItem Import(string filename, ContentImporterContext context)
        {
            // Create json reader
            using (JsonReader jsonReader = new JsonTextReader(new StreamReader(filename)))
            {
                // Create serialized reader
                JsonSerializedReader serializedReader = new JsonSerializedReader(jsonReader);

                // Get the game object
                GameObject importedPrefab = Serializer.Deserialize<GameObject>(serializedReader);

                // Get imported content
                return new PrefabContentItem(importedPrefab);
            }
        }
    }
}
