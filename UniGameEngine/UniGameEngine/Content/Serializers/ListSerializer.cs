using System.Collections.Generic;
using System.IO;

namespace UniGameEngine.Content.Serializers
{
    [CustomSerializer(typeof(List<>))]
    public sealed class ListSerializer<T> : Serializer<List<T>>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref List<T> list)
        {
            // Expect array
            reader.Expect(SerializedType.ArrayStart);

            // Read start of the array
            int length;
            reader.ReadArrayStart(out length);

            // Create list instance
            list = length != -1
                ? new List<T>(length)
                : new List<T>();

            // Store count
            int count = 0;

            // Read until array end
            while(reader.PeekType != SerializedType.ArrayEnd)
            {
                // Check for too many elements
                if (length != -1 && count >= length)
                    throw new InvalidDataException("Too many array elements specified. Expected: " + length);

                // Read the value
                list.Add(Deserialize<T>(reader));

                // Update count
                count++;
            }

            // Read end array
            reader.ReadArrayEnd();
        }

        public override void WriteValue(SerializedWriter writer, List<T> value)
        {
            // Start the array
            writer.WriteArrayStart(value.Count);

            // Write the array elements
            {
                // Process all elements
                for (int i = 0; i < value.Count; i++)
                {
                    // Serialize the element value
                    Serialize(writer, value[i]);
                }
            }

            // End the array
            writer.WriteArrayEnd();
        }
    }
}
