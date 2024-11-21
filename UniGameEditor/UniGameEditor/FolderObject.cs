using System.Runtime.Serialization;
using UniGameEditor.Content;

namespace UniGameEditor
{
    [DataContract]
    public sealed class FolderObject
    {
        // Internal
        [DataMember]
        internal string projectRelativePath = null;
        internal string contentRelativePath = null;

        // Properties
        public string ProjectRelativePath
        {
            get { return projectRelativePath; }
        }

        public string ContentRelativePath
        {
            get { return contentRelativePath; }
        }

        // Constructor
        public FolderObject(string contentRelativePath) 
        { 
            this.contentRelativePath = contentRelativePath;
            this.projectRelativePath = (string.IsNullOrEmpty(contentRelativePath) == true 
                || contentRelativePath == ".")
                    ? "Content"
                    : "Content/" + contentRelativePath;

            // Check valid path
            ContentDatabase.CheckContentPathValid(projectRelativePath);
        }
    }
}
