using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using UniGameEngine.Content;
using UniGameEngine.Content.Serializers;

namespace UniGamePipeline
{
    internal sealed class ContentSerializedWriter : SerializedWriter
    {
        // Private
        private ContentWriter contentWriter = null;

        // Constructor
        public ContentSerializedWriter(ContentWriter contentWriter)
        {
            this.contentWriter = contentWriter;
        }

        // Methods
        public override void Dispose()
        {
            contentWriter.DisposeAsync();
            contentWriter = null;
        }

        public override void WriteNull()
        {
            contentWriter.Write((byte)ContentSerializedType.Null);
        }

        public override void WritePropertyName(string name)
        {
            contentWriter.Write((byte)ContentSerializedType.PropertyName);
            contentWriter.Write(name);
        }

        public override void WriteObjectStart(in TypeReference typeReference)
        {
            contentWriter.Write((byte)ContentSerializedType.ObjectStart);
            contentWriter.Write(typeReference.IsRequired);

            // Check for type if
            if(typeReference.IsRequired == true)
                contentWriter.Write(typeReference.TypeName);
        }

        public override void WriteObjectEnd()
        {
            contentWriter.Write((byte)ContentSerializedType.ObjectEnd);
        }

        public override void WriteArrayStart(int length)
        {
            contentWriter.Write((byte)ContentSerializedType.ArrayStart);
            contentWriter.Write(length);
        }

        public override void WriteArrayEnd()
        {
            contentWriter.Write((byte)ContentSerializedType.ArrayEnd);
        }

        public override void WriteBoolean(bool value)
        {
            contentWriter.Write((byte)ContentSerializedType.Bool);
            contentWriter.Write(value);
        }

        public override void WriteChar(char value)
        {
            contentWriter.Write((byte)ContentSerializedType.Char);
            contentWriter.Write(value);
        }

        public override void WriteString(string value)
        {
            contentWriter.Write((byte)ContentSerializedType.String);
            contentWriter.Write(value);
        }

        public override void WriteSByte(sbyte value)
        {
            contentWriter.Write((byte)ContentSerializedType.UInt8);
            contentWriter.Write(value);
        }

        public override void WriteInt16(short value)
        {
            contentWriter.Write((byte)ContentSerializedType.Int16);
            contentWriter.Write(value);
        }

        public override void WriteInt32(int value)
        {
            contentWriter.Write((byte)ContentSerializedType.Int32);
            contentWriter.Write(value);
        }

        public override void WriteInt64(long value)
        {
            contentWriter.Write((byte)ContentSerializedType.Int64);
            contentWriter.Write(value);
        }    

        public override void WriteByte(byte value)
        {
            contentWriter.Write((byte)ContentSerializedType.UInt8);
            contentWriter.Write(value);
        }

        public override void WriteUInt16(ushort value)
        {
            contentWriter.Write((byte)ContentSerializedType.UInt16);
            contentWriter.Write(value);
        }

        public override void WriteUInt32(uint value)
        {
            contentWriter.Write((byte)ContentSerializedType.UInt32);
            contentWriter.Write(value);
        }

        public override void WriteUInt64(ulong value)
        {
            contentWriter.Write((byte)ContentSerializedType.UInt64);
            contentWriter.Write(value);
        }

        public override void WriteSingle(float value)
        {
            contentWriter.Write((byte)ContentSerializedType.Single);
            contentWriter.Write(value);
        }

        public override void WriteDouble(double value)
        {
            contentWriter.Write((byte)ContentSerializedType.Double);
            contentWriter.Write(value);
        }

        public override void WriteDecimal(decimal value)
        {
            contentWriter.Write((byte)ContentSerializedType.Decimal);
            contentWriter.Write(value);
        }
    }
}
