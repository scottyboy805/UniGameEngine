
using System;
using UniGameEngine.Content.Serializers;

namespace UniGameEngine.Content
{
    public abstract class SerializedWriter : IDisposable
    {
        // Methods
        public abstract void Dispose();

        public abstract void WriteNull();
        public abstract void WritePropertyName(string name);

        public void WriteObjectStart() => WriteObjectStart(default);
        public abstract void WriteObjectStart(in TypeReference typeReference);
        public abstract void WriteObjectEnd();
        public abstract void WriteArrayStart(int length);
        public abstract void WriteArrayEnd();

        public abstract void WriteBoolean(bool value);
        public abstract void WriteChar(char value);
        public abstract void WriteString(string value);
        public abstract void WriteSByte(sbyte value);
        public abstract void WriteInt16(short value);
        public abstract void WriteInt32(int value);
        public abstract void WriteInt64(long value);
        public abstract void WriteByte(byte value);
        public abstract void WriteUInt16(ushort value);
        public abstract void WriteUInt32(uint value);
        public abstract void WriteUInt64(ulong value);
        public abstract void WriteDecimal(decimal value);
        public abstract void WriteSingle(float value);
        public abstract void WriteDouble(double value);        
    }
}
