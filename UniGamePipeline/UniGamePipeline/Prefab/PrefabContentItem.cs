using Microsoft.Xna.Framework.Content.Pipeline;
using UniGameEngine;

namespace UniGamePipeline.Prefab
{
    internal sealed class PrefabContentItem : ContentItem
    {
        // Private
        private GameObject importedObject = null;

        // Properties
        public GameObject ImportedObject
        {
            get { return importedObject; }
        }

        // Constructor
        public PrefabContentItem(GameObject importedObject)
        {
            this.importedObject = importedObject;
        }
    }
}
