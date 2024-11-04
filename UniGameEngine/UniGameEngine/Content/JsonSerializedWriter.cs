using Newtonsoft.Json;
using UniGameEngine.Content.Serializers;

namespace UniGameEngine.Content
{
    internal sealed class JsonSerializedWriter : SerializedWriter
    {
        // Private
        private JsonWriter writer = null;

        // Constructor
        public JsonSerializedWriter(JsonWriter writer, bool format = true) 
        { 
            this.writer = writer;
            this.writer.Formatting = format == true
                ? Formatting.Indented
                : Formatting.None;
        }

        // Methods
        public override void Dispose()
        {
            writer.Close();
            writer = null;
        }

        public override void WriteNull()
        {
            writer.WriteNull();
        }

        public override void WritePropertyName(string name)
        {
            writer.WritePropertyName(name);
        }

        public override void WriteObjectStart(in TypeReference typeReference)
        {
            writer.WriteStartObject();

            // Check for type
            if(typeReference.IsRequired == true)
            {
                writer.WritePropertyName(TypeReference.TypeSpecifier);
                writer.WriteValue(typeReference.TypeName);
            }
        }

        public override void WriteObjectEnd()
        {
            writer.WriteEndObject();
        }

        public override void WriteArrayStart(int length)
        {
            writer.WriteStartArray();
        }

        public override void WriteArrayEnd()
        {
            writer.WriteEndArray();
        }

        public override void WriteBoolean(bool value)
        {
            writer.WriteValue(value);
        }

        public override void WriteChar(char value)
        {
            writer.WriteValue(value);
        }

        public override void WriteString(string value)
        {
            writer.WriteValue(value);
        }

        public override void WriteSByte(sbyte value)
        {
            writer.WriteValue(value);
        }

        public override void WriteInt16(short value)
        {
            writer.WriteValue(value);
        }

        public override void WriteInt32(int value)
        {
            writer.WriteValue(value);
        }

        public override void WriteInt64(long value)
        {
            writer.WriteValue(value);
        }

        public override void WriteByte(byte value)
        {
            writer.WriteValue(value);
        }

        public override void WriteUInt16(ushort value)
        {
            writer.WriteValue(value);
        }

        public override void WriteUInt32(uint value)
        {
            writer.WriteValue(value);
        }

        public override void WriteUInt64(ulong value)
        {
            writer.WriteValue(value);
        }

        public override void WriteSingle(float value)
        {
            writer.WriteValue(value);
        }

        public override void WriteDouble(double value)
        {
            writer.WriteValue(value);
        }

        public override void WriteDecimal(decimal value)
        {
            writer.WriteValue(value);
        }
    }
}
