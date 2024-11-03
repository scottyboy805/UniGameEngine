using System;

namespace UniGameEngine.Content.Serializers
{
    public sealed class EnumSerializer<T> : Serializer<T> where T : Enum
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref T value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read ulong
            ulong valueTemp;
            reader.ReadUInt64(out valueTemp);

            // Convert to enum
            value = (T)Enum.ToObject(typeof(T), value);
        }

        public override void WriteValue(SerializedWriter writer, T value)
        {
            ulong val;
            try
            {
                val = Convert.ToUInt64(value);
            }
            catch (OverflowException)
            {
                unchecked
                {
                    val = (ulong)Convert.ToInt64(value);
                }
            }

            // Write as 64 bit
            writer.WriteUInt64(val);
        }
    }
}
