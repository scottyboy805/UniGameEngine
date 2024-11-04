using Microsoft.Xna.Framework.Content.Pipeline;
using System.Collections.Generic;
using UniGameEngine;
using UniGameEngine.Content.Contract;

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

        public string ContentPath
        {
            get { return importedObject.ContentPath; }
        }

        // Constructor
        public GameElementContentItem(T importedElement)
        {
            this.importedObject = importedElement;
            this.Name = importedObject.Name;
        }

        // Methods
        public IEnumerable<string> GetContentDependencies()
        {
            return GetContentDependenciesRecursive(importedObject);
        }

        private IEnumerable<string> GetContentDependenciesRecursive(GameElement element)
        {
            // Create contract
            DataContract contract = DataContract.ForType(element.GetType());

            // Check all properties
            foreach(DataContractProperty property in contract.SerializeProperties)
            {
                // Check for object
                if(property.IsObject == true)
                {
                    // Get assigned value
                    object instanceValue = property.GetInstanceValue(element);

                    // Check for element
                    if(instanceValue is GameElement childElement)
                    {
                        // Check for content path
                        if(string.IsNullOrEmpty(childElement.ContentPath) == false)
                            yield return childElement.ContentPath;

                        // Search deeper
                        foreach(string contentPath in GetContentDependenciesRecursive(childElement))
                            yield return contentPath;
                    }
                }
            }
        }
    }
}
