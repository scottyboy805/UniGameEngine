using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Newtonsoft.Json;
using System.IO;
using UniGameEngine;
using UniGameEngine.Content;
using UniGameEngine.Content.Reader;
using UniGameEngine.Content.Serializers;

namespace UniGamePipeline.Prefab
{
    [ContentTypeWriter]
    internal sealed class PrefabWriter : GameElementContentWriter<GameObject, PrefabContentReader> { }

    [ContentImporter(".prefab", DisplayName = "Prefab Json Importer - UniGameEngine", DefaultProcessor = nameof(PrefabProcessor))]
    internal sealed class PrefabImporter : ContentImporter<GameElementContentItem<GameObject>>
    {
        // Methods
        public override GameElementContentItem<GameObject> Import(string filename, ContentImporterContext context)
        {
            // Create json reader
            JsonReader jsonReader = new JsonTextReader(new StreamReader(filename));

            // Create serialized reader
            using (JsonSerializedReader serializedReader = new JsonSerializedReader(jsonReader))
            {
                // Get the game object
                GameObject importedPrefab = Serializer.Deserialize<GameObject>(serializedReader);

                // Get imported content
                return new GameElementContentItem<GameObject>(importedPrefab);
            }
        }
    }
}
