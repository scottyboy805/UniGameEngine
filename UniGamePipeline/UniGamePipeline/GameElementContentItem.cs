using Microsoft.Xna.Framework.Content.Pipeline;
using UniGameEngine;

namespace UniGamePipeline
{
    internal sealed class GameElementContentItem<T> : ContentItem where T : GameElement
    {
        // Private
        private T importedObject = null;

        // Properties
        public T ImportedElement
        {
            get { return importedObject; }
        }

        // Constructor
        public GameElementContentItem(T importedElement)
        {
            this.importedObject = importedElement;
            this.Name = importedObject.Name;
        }
    }
}
