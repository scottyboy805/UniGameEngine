using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace UniGameEngine.Content.Contract
{
    public sealed class DataContract
    {
        // Private
        private static HashSet<Type> alwaysSerializeTypes = new HashSet<Type>();
        private static readonly Dictionary<Type, DataContract> typeContracts = new Dictionary<Type, DataContract>();

        private Type contractType = null;
        private bool isTypeSerializable = false;
        private List<DataContractProperty> serializeProperties = new List<DataContractProperty>();

        // Properties
        public Type ContractType
        {
            get { return contractType; }
        }

        public bool IsTypeSerializable
        {
            get { return isTypeSerializable; }
        }

        public IReadOnlyList<DataContractProperty> SerializeProperties
        {
            get { return serializeProperties; }
        }

        public bool HasSerializeProperties
        {
            get { return serializeProperties.Count > 0; }
        }

        public DataContractProperty this[string name]
        {
            get { return serializeProperties.Find(m => m.SerializeName == name); }
        }

        // Constructor
        private DataContract(Type fromType)
        {
            // Check for array
            if (fromType.IsArray == true) throw new ArgumentException("Cannot create a data contract for an array type");

            this.contractType = fromType;
            this.isTypeSerializable = CheckTypeSerializable(fromType);

            // Setup members
            InitializeMembers(contractType);
        }

        // Methods
        public override string ToString()
        {
            return string.Format("Data Contract ({0}): {1}", contractType.Name, contractType);
        }

        private void InitializeMembers(Type type)
        {
            // Search base type first
            if(type.BaseType != null && type.BaseType != typeof(object) && type.BaseType != typeof(ValueType))
            {
                // Process all members
                InitializeMembers(type.BaseType);
            }

            // Check for always serialize
            bool alwaysSerialize = alwaysSerializeTypes.Contains(type);

            // Process fields
            foreach(FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                // Check for serializable
                if (DataContractProperty.CheckMemberSerializable(field) == true || alwaysSerialize == true)
                {
                    serializeProperties.Add(new DataContractFieldMember(field));
                }
            }

            // Process properties
            foreach(PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                // Check for serializable
                if (DataContractProperty.CheckMemberSerializable(property) == true || alwaysSerialize == true)
                {
                    serializeProperties.Add(new DataContractPropertyMember(property));
                }
            }
        }

        public static bool CheckTypeSerializable(Type checkType)
        {
            // Check for always serialize
            if (alwaysSerializeTypes.Contains(checkType) == true)
                return true;

            // Check for serializable
            if(typeof(GameElement).IsAssignableFrom(checkType) == true ||
                checkType.GetCustomAttribute<DataContractAttribute>() != null)
            {
                return true;
            }
            return false;
        }

        public static DataContract ForType(Type type)
        {
            // Check for null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            DataContract result;

            // Check for cached value
            if (typeContracts.TryGetValue(type, out result) == true)
                return result;

            // Create new contract
            lock (typeContracts)
            {
                result = typeContracts[type] = new DataContract(type);
            }

            return result;
        }            
    }
}
