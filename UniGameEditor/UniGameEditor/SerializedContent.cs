using UniGameEngine.Content.Contract;

namespace UniGameEditor
{
    public sealed class SerializedContent
    {
        // Private
        private DataContract contract = null;
        private object[] instances = null;
        private List<SerializedProperty> properties = null;

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

        public bool IsEditingMultiple
        {
            get { return instances.Length > 1; }
        }

        // Constructor
        public SerializedContent(Type type, object[] instances)
        {
            this.contract = DataContract.ForType(type);
            this.instances = instances;

            InitializeProperties();
        }

        // Methods
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
            if (contract.SerializeProperties.Count == 0)
                return;

            // Create collection
            properties = new List<SerializedProperty>(contract.SerializeProperties.Count);

            // Get all properties
            foreach (DataContractProperty childProperty in contract.SerializeProperties)
            {
                // Add the property
                properties.Add(new SerializedProperty(childProperty, instances));
            }
        }
    }
}
