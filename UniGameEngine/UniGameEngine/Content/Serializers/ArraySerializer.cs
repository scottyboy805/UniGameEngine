using System;
using System.Collections.Generic;

namespace UniGameEngine.Content.Serializers
{
    public sealed class ArraySerializer<T> : Serializer<T[]>
    {
        // Private
        private List<T> pooledList = new List<T>();

        // Methods
        public override void ReadValue(SerializedReader reader, ref T[] array)
        {
            // Expect array
            reader.Expect(SerializedType.ArrayStart);

            // Read start of the array
            int length;
            reader.ReadArrayStart(out length);

            // Check for length
            if(length != -1)
            {
                // Create array instance
                array = new T[length];

                // Process all elements
                for (int i = 0; i < length; i++)
                {
                    // Read the value
                    array[i] = Deserialize<T>(reader);
                }
            }
            else
            {
                // Use pooled list
                pooledList.Clear();

                // Read until array end
                while(reader.PeekType != SerializedType.ArrayEnd)
                {
                    // Read the value
                    pooledList.Add(Deserialize<T>(reader));
                }

                // Build final array
                array = pooledList.ToArray();
            }

            // Read end array
            reader.ReadArrayEnd();
        }

        public override void WriteValue(SerializedWriter writer, T[] value)
        {
            // Start the array
            writer.WriteArrayStart(value.Length);

            // Write the array elements
            {
                // Process all elements
                for (int i = 0; i < value.Length; i++)
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
