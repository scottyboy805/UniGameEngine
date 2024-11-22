using System.Runtime.Serialization;
using UniGameEditor.Content;
using UniGameEngine;
using UniGameEngine.Content.Contract;

namespace UniGameEditor
{
    [DataContract]
    public sealed class SerializedContent
    {
        // Private
        private DataContract contract = null;
        private object[] instances = null;
        private List<SerializedProperty> properties = null;
        private List<SerializedProperty> visibleProperties = null;

        [DataMember(Name = "$meta")]
        private ContentMeta meta = null;

        // Properties
        public DataContract Contract
        {
            get { return contract; }
        }

        public string DisplayName
        {
            get { return contract.ContractType.Name; }
        }

        public IReadOnlyList<SerializedProperty> Properties
        {
            get 
            { 
                if(properties != null)
                    return properties;

                return Array.Empty<SerializedProperty>();
            }
        }

        public IReadOnlyList<SerializedProperty> VisibleProperties
        {
            get
            {
                // Create properties
                if (visibleProperties == null && properties != null)
                    visibleProperties = new List<SerializedProperty>(properties.Where(p => p.IsVisible == true));

                // Get result
                return visibleProperties != null
                    ? visibleProperties
                    : Array.Empty<SerializedProperty>();
            }
        }

        public bool IsEditingMultiple
        {
            get { return instances.Length > 1; }
        }

        // Constructor
        public SerializedContent(Type type, object[] instances, ContentMeta meta = null)
        {
            this.contract = DataContract.ForType(type);
            this.instances = instances;
            this.meta = meta;

            InitializeProperties();
        }

        // Methods
        public override string ToString()
        {
            return string.Format("Serialized Content ({0}): {1}", DisplayName, contract.ContractType);
        }

        public SerializedProperty FindPropertyName(string name)
        {
            return properties.FirstOrDefault(n => n.Property.PropertyName == name);
        }

        public SerializedProperty FindSerializedName(string name)
        {
            return properties.FirstOrDefault(n => n.Property.SerializeName == name);
        }

        private void InitializeProperties()
        {
            // Create properties
            if (contract.SerializeProperties.Count == 0 && meta == null)
                return;

            // Create collection
            properties = new List<SerializedProperty>(contract.SerializeProperties.Count);

            // Get all properties
            foreach (DataContractProperty childProperty in contract.SerializeProperties)
            {
                // Add the property
                properties.Add(new SerializedProperty(childProperty, instances));
            }

            // Check for meta
            if (meta != null)
            {
                // Get the contract for this
                DataContract thisContract = DataContract.ForType(typeof(SerializedContent));

                // Add the property with the meta
                properties.Add(new SerializedProperty(thisContract.SerializeProperties[0], new[] { this }));
            }
        }
    }
}
