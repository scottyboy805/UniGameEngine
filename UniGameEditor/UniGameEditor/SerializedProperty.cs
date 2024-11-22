using System.Collections;
using UniGameEngine.Content.Contract;

namespace UniGameEditor
{
    public sealed class SerializedProperty
    {
        // Private
        private DataContractProperty property = null;
        private object[] instances = null;
        private List<SerializedProperty> childProperties = null;

        // Properties
        public DataContractProperty Property
        {
            get { return property; }
        }

        public string DisplayName
        {
            get { return property.SerializeName; }
        }

        public bool IsObject
        {
            get { return property.IsObject; }
        }

        public bool IsArray
        {
            get { return property.IsArray; }
        }

        public bool IsArrayElement
        {
            get { return property.IsArrayElement; }
        }

        public IReadOnlyList<SerializedProperty> Children
        {
            get 
            { 
                if(childProperties != null)
                    return childProperties;

                return Array.Empty<SerializedProperty>();
            }
        }

        public bool IsEditingMultiple
        {
            get { return instances.Length > 1; }
        }

        // Constructor
        public SerializedProperty(DataContractProperty property, object[] instances)
        {
            this.property = property;
            this.instances = instances;

            // Initialize
            InitializeProperties();
        }

        // Methods
        public bool GetValue<T>(out T value, out bool isMixed)
        {
            // Check for type
            if (property.PropertyType != typeof(T))
                throw new InvalidOperationException("Cannot get property value as type: " + typeof(T) + ", property is of type: " + property.PropertyType);

            // Check for none
            if (instances == null || instances.Length == 0)
            {
                value = default;
                isMixed = false;
                return false;
            }

            // Check for single instance
            if(instances.Length == 1)
            {
                isMixed = false;
                value = (T)property.GetInstanceValue(instances[0]);
                return true;
            }

            // Store values
            T firstValue = (T)property.GetInstanceValue(instances[0]);
            value = firstValue;
            isMixed = false;

            // Check all
            for(int i = 1; i < instances.Length; i++)
            {
                // Get value
                T otherValue = (T)property.GetInstanceValue(instances[i]);

                // Check for mixed
                if((firstValue != null && firstValue.Equals(otherValue) == false)
                    || (otherValue != null && otherValue.Equals(firstValue) == false))
                {
                    isMixed = true;
                    break;
                }
            }
            return true;
        }

        public SerializedProperty FindPropertyName(string name)
        {
            return childProperties.FirstOrDefault(n => n.Property.PropertyName == name);
        }

        public SerializedProperty FindSerializedName(string name)
        {
            return childProperties.FirstOrDefault(n => n.Property.SerializeName == name);
        }

        internal SerializedContent CreateContent()
        {
            object[] newInstances = new object[instances.Length];

            // Get instances
            for(int i = 0; i < instances.Length; i++)
                newInstances[i] = property.GetInstanceValue(instances[i]);

            // Create contract
            return new SerializedContent(property.PropertyType, newInstances);
        }

        private void InitializeProperties()
        {
            // Check for object
            if(property.IsObject == true)
            {
                // Create object contract
                DataContract objectContract = DataContract.ForType(property.PropertyType);

                // Create properties
                if(objectContract.SerializeProperties.Count == 0)
                    return;

                // Create collection
                childProperties = new List<SerializedProperty>(objectContract.SerializeProperties.Count);

                // Get all properties
                foreach(DataContractProperty childProperty in objectContract.SerializeProperties)
                {
                    object[] instanceValues = new object[instances.Length];

                    // Get all values
                    for(int i = 0; i < instances.Length; i++)
                        instanceValues[i] = property.GetInstanceValue(instances[i]);

                    // Add the property
                    childProperties.Add(new SerializedProperty(childProperty, instanceValues));
                }
            }
            // Check for array
            else if(property.IsArray == true)
            {
                // Create array objects
                IList[] arrayInstances = new IList[instances.Length];
                int maxSize = -1;

                // Fetch all array instances
                for (int i = 0; i < arrayInstances.Length; i++)
                {
                    // Get the array
                    arrayInstances[i] = (IList)property.GetInstanceValue(instances[i]);

                    // Assign the max size
                    if(maxSize == -1 || arrayInstances[i].Count > maxSize)
                        maxSize = arrayInstances[i].Count;
                }

                // Create collection
                childProperties = new List<SerializedProperty>(maxSize);

                // Create all elements
                for (int i = 0; i < maxSize; i++)
                {
                    // Add the property
                    childProperties.Add(new SerializedProperty(
                        new DataContractElement(property.ElementType, i), arrayInstances));
                }
            }
        }
    }
}
