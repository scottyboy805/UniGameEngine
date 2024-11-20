using System.Runtime.Serialization;
using UniGameEditor.Content;

namespace UniGameEditor
{
    [DataContract]
    public sealed class FolderObject
    {
        // Internal
        [DataMember]
        internal string contentFolder = null;

        // Properties
        public string ContentFolder
        {
            get { return contentFolder; }
        }

        // Constructor
        public FolderObject(string contentFolder) 
        { 
            this.contentFolder = contentFolder;

            // Check valid path
            ContentDatabase.CheckContentPathValid(contentFolder);
        }
    }
}
