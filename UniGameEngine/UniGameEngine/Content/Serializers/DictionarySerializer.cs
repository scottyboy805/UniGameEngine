using System.Collections.Generic;
using System.IO;

namespace UniGameEngine.Content.Serializers
{
    [CustomSerializer(typeof(Dictionary<,>))]
    public sealed class DictionarySerializer<TKey, TValue> : Serializer<Dictionary<TKey, TValue>>
    {
        // Private
        private const string keyName = "Key";
        private const string valueName = "Value";

        private Serializer<TKey> keySerializer = Get<TKey>();
        private Serializer<TValue> valueSerializer = Get<TValue>();

        // Methods
        public override void ReadValue(SerializedReader reader, ref Dictionary<TKey, TValue> dictionary)
        {
            // Expect array
            reader.Expect(SerializedType.ArrayStart);

            // Read start of the array
            int length;
            reader.ReadArrayStart(out length);

            // Create dictionary instance
             dictionary = length != -1
                ? new Dictionary<TKey, TValue>(length)
                : new Dictionary<TKey, TValue>();

            // Store count
            int count = 0;

            // Read until array end
            while(reader.PeekType != SerializedType.ArrayEnd)
            {
                // Check for too many elements
                if (length != -1 && count >= length)
                    throw new InvalidDataException("Too many array elements specified. Expected: " + length);

                // Expect object
                reader.Expect(SerializedType.ObjectStart);

                // Read start of object
                reader.ReadObjectStart();
                {
                    // Read property name key
                    string keyName;
                    reader.ReadPropertyName(out keyName);

                    // Read key
                    TKey key = default;
                    keySerializer.ReadValue(reader, ref key);

                    // Read property value key
                    string valueName;
                    reader.ReadPropertyName(out valueName);

                    // Read value
                    TValue value = default;
                    valueSerializer.ReadValue(reader, ref value);

                    // Insert into dictionary
                    dictionary[key] = value;
                }
                // Read end of object
                reader.ReadObjectEnd();
            }

            // Read end of the array
            reader.ReadArrayEnd();
        }

        public override void WriteValue(SerializedWriter writer, Dictionary<TKey, TValue> value)
        {
            // Start the array
            writer.WriteArrayStart(value.Count);

            // Write the array elements
            {
                // Process all elements
                foreach(KeyValuePair<TKey, TValue> element in value)
                {
                    // Write start of object
                    writer.WriteObjectStart();
                    {
                        // Write the key
                        writer.WritePropertyName(keyName);
                        keySerializer.WriteValue(writer, element.Key);

                        // Write value
                        writer.WritePropertyName(valueName);
                        valueSerializer.WriteValue(writer, element.Value);
                    }
                    // Write end of object
                    writer.WriteObjectEnd();
                }
            }

            // End the array
            writer.WriteArrayEnd();
        }
    }
}
