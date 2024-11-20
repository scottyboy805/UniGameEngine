using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Framework.Content.Pipeline.Builder;
using System.Runtime.Serialization;

namespace UniGameEditor.Content
{
    public enum BuildAction
    {
        Build,
        Copy,
    }

    [DataContract]
    public sealed class ContentMeta
    {
        // Private
        private string contentPath = "";

        [DataMember(Name = "Guid")]
        private string guid = "";
        [DataMember(Name = "Action")]
        private BuildAction action = BuildAction.Build;
        [DataMember(Name = "Importer")]
        private string importer = "";
        [DataMember(Name = "Processor")]
        private string processor = "";
        //[DataMember(Name = "ProcessorParameters")]
        private Dictionary<string, object> processorParameters;

        private Type contentType = null;

        // Properties
        public string ContentPath
        {
            get { return contentPath; }
        }

        public string Guid
        {
            get { return guid; }
        }

        public BuildAction Action
        {
            get { return action; }
        }

        public string Importer
        {
            get { return importer; }
        }

        public string Processor
        {
            get { return processor; }
        }

        public Dictionary<string, object> ProcessorParameters
        {
            get { return processorParameters; }
        }

        // Methods
        internal void EnsureGuid()
        {
            if(string.IsNullOrEmpty(guid) == true)
                guid = System.Guid.NewGuid().ToString();
        }

        internal Type GetContentType(PipelineManager pipeline)
        {
            // Try to get processor type
            if (contentType == null)
                contentType = pipeline.GetProcessorType(processor);

            return contentType;
        }

        public static ContentMeta CreateForFile(PipelineManager pipeline, string contentPath)
        {
            // Get importer and processor
            string importer = null;
            string processor = null;
            pipeline.ResolveImporterAndProcessor(contentPath, ref importer, ref processor);

            // Get default values for processor
            OpaqueDataDictionary processorParams = pipeline.GetProcessorDefaultValues(processor);

            // Create meta
            return new ContentMeta
            {
                contentPath = contentPath,
                guid = System.Guid.NewGuid().ToString(),
                importer = importer,
                processor = processor,
                processorParameters = new Dictionary<string, object>(processorParams),
            };
        }
    }
}
