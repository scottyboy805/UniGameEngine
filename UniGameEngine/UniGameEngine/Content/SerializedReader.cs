using System;
using UniGameEngine.Content.Serializers;

namespace UniGameEngine.Content
{
    public abstract class SerializedReader : IDisposable
    {
        // Properties
        public abstract SerializedType PeekType { get; }

        // Methods
        public void Expect(SerializedType type)
        {
            if (PeekType != type)
                throw new FormatException("Expected type: " + type);
        }

        public abstract void Dispose();

        public abstract void Skip();
        public abstract bool ReadNull();
        public abstract bool ReadPropertyName(out string name);

        public abstract bool ReadObjectStart(out string typeId);
        public abstract bool ReadObjectEnd();
        public abstract bool ReadArrayStart(out int length);
        public abstract bool ReadArrayEnd();

        public abstract bool ReadBoolean(out bool value);
        public abstract bool ReadChar(out char value);
        public abstract bool ReadString(out string value);
        public abstract bool ReadSByte(out sbyte value);
        public abstract bool ReadInt16(out short value);
        public abstract bool ReadInt32(out int value);
        public abstract bool ReadInt64(out long value);
        public abstract bool ReadByte(out byte value);
        public abstract bool ReadUInt16(out ushort value);
        public abstract bool ReadUInt32(out uint value);
        public abstract bool ReadUInt64(out ulong value);
        public abstract bool ReadDecimal(out decimal value);        
        public abstract bool ReadDouble(out double value);
        public abstract bool ReadSingle(out float value);
    }
}
