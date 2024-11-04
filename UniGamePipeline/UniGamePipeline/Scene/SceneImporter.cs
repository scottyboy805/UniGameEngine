using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Newtonsoft.Json;
using System.IO;
using UniGameEngine.Content;
using UniGameEngine.Content.Reader;
using UniGameEngine.Content.Serializers;
using UniGameEngine.Scene;

namespace UniGamePipeline.Scene
{
    [ContentTypeWriter]
    internal sealed class SceneWriter : GameElementContentWriter<GameScene, SceneContentReader> { }

    [ContentImporter(".scene", DisplayName = "Scene Json Importer - UniGameEngine", DefaultProcessor = nameof(SceneProcessor))]
    internal sealed class SceneImporter : ContentImporter<GameElementContentItem<GameScene>>
    {
        // Methods
        public override GameElementContentItem<GameScene> Import(string filename, ContentImporterContext context)
        {
            // Create json reader
            JsonReader jsonReader = new JsonTextReader(new StreamReader(filename));

            // Create serialized reader
            using (JsonSerializedReader serializedReader = new JsonSerializedReader(jsonReader))
            {
                // Get the game object
                GameScene importedScene = Serializer.Deserialize<GameScene>(serializedReader);

                // Get imported content
                return new GameElementContentItem<GameScene>(importedScene);
            }
        }
    }
}
