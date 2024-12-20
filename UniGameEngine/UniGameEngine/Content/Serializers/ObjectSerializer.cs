﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniGameEngine.Content.Contract;

namespace UniGameEngine.Content.Serializers
{    
    public sealed class ObjectSerializer<T> : Serializer<T>
    {
        // Private
        TypeReference typeReference = new TypeReference(TypeManager, typeof(T));

        // Methods
        public override void ReadValue(SerializedReader reader, ref T instance)
        {
            // Expect object
            reader.Expect(SerializedType.ObjectStart);

            // Read object start            
            reader.ReadObjectStart(ref typeReference);

            // Get object type
            Type objectType = typeof(T);

            // Create instance
            if (instance == null)
            {
                // Create type instance
                objectType = typeReference.Resolve(TypeManager, typeof(T));

                // Create instance
                instance = (T)TypeManager.CreateTypeInstance(objectType);
            }

            // Create contract
            DataContract contract = DataContract.ForType(objectType);

            // Get serialize properties
            IReadOnlyList<DataContractProperty> properties = contract.SerializeProperties;

            // Read properties
            {
                // Read until object end
                while (reader.PeekType != SerializedType.ObjectEnd)
                {
                    // Expect property name
                    reader.Expect(SerializedType.PropertyName);

                    // Read the property name
                    string propertyName;
                    reader.ReadPropertyName(out propertyName);

                    // Check for property
                    DataContractProperty matchedProperty = properties.FirstOrDefault(p => p.SerializeName == propertyName);

                    // Check for found
                    if (matchedProperty != null)
                    {
                        // Deserialize the property
                        object propertyValue = null;
                        Deserialize(reader, matchedProperty.PropertyType, ref propertyValue);

                        // Update the property
                        matchedProperty.SetInstanceValue(ref instance, propertyValue);
                    }
                    else
                    {
                        // Skip the value
                        reader.Skip();
                    }
                }
            }

            // Read object end
            reader.ReadObjectEnd();
        }

        public override void WriteValue(SerializedWriter writer, T value)
        {
            // Check for null
            if(value.Equals(default) == true)
            {
                writer.WriteNull();
                return;
            }

            // Write object start
            writer.WriteObjectStart(new TypeReference(TypeManager, typeof(T), SerializeAsType));

            // Create contract
            DataContract contract = DataContract.ForType(typeof(T));

            // Get serialize properties
            IReadOnlyList<DataContractProperty> properties = contract.SerializeProperties;

            // Write properties
            {
                // Process all properties
                foreach (DataContractProperty property in properties)
                {
                    // Write property name
                    writer.WritePropertyName(property.SerializeName);

                    // Get the value
                    object propertyValue = property.GetInstanceValue(value);

                    // Serialize value
                    Serialize(writer, propertyValue);
                }
            }

            // Write object end
            writer.WriteObjectEnd();
        }
    }
}
