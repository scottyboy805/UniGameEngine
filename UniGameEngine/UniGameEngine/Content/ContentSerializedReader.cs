using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using UniGameEngine.Content.Serializers;

namespace UniGameEngine.Content
{
    internal enum ContentSerializedType : byte
    {
        Null,
        PropertyName,
        ObjectStart,
        ObjectEnd,
        ArrayStart,
        ArrayEnd,
        Bool,
        Char,
        String,
        Int8,
        Int16,
        Int32,
        Int64,
        UInt8,
        UInt16,
        UInt32,
        UInt64,
        Single,
        Double,
        Decimal,

        EOF,
    }

    internal sealed class ContentSerializedReader : SerializedReader
    {
        // Private
        private ContentReader contentReader = null;
        private ContentSerializedType peekSerializedType = 0;

        // Properties
        public override SerializedType PeekType
        {
            get
            {
                return peekSerializedType switch
                { 
                    ContentSerializedType.Null => SerializedType.Null,                    
                    ContentSerializedType.PropertyName => SerializedType.PropertyName,
                    ContentSerializedType.ObjectStart => SerializedType.ObjectStart,
                    ContentSerializedType.ObjectEnd => SerializedType.ObjectEnd,
                    ContentSerializedType.ArrayStart => SerializedType.ArrayStart,
                    ContentSerializedType.ArrayEnd => SerializedType.ArrayEnd,
                    ContentSerializedType.Bool => SerializedType.Boolean,
                    ContentSerializedType.Char => SerializedType.String,
                    ContentSerializedType.String => SerializedType.String,
                    ContentSerializedType.Int8 => SerializedType.Number,
                    ContentSerializedType.Int16 => SerializedType.Number,
                    ContentSerializedType.Int32 => SerializedType.Number,
                    ContentSerializedType.Int64 => SerializedType.Number,
                    ContentSerializedType.UInt8 => SerializedType.Number,
                    ContentSerializedType.UInt16 => SerializedType.Number,
                    ContentSerializedType.UInt32 => SerializedType.Number,
                    ContentSerializedType.UInt64 => SerializedType.Number,
                    ContentSerializedType.Single => SerializedType.Number,
                    ContentSerializedType.Double => SerializedType.Number,
                    ContentSerializedType.Decimal => SerializedType.Number,
                    _ => SerializedType.Invalid,
                };
            }
        }

        // Constructor
        public ContentSerializedReader(ContentReader contentReader)
        {
            this.contentReader = contentReader;

            // Read first type
            ReadSerializedType();
        }

        // Methods
        public override void Dispose()
        {
            contentReader.Dispose();
            contentReader = null;
        }

        public override bool ReadNull()
        {
            RequireSerializedType(ContentSerializedType.Null);
            ReadSerializedType();
            return true;
        }

        public override bool ReadPropertyName(out string name)
        {
            RequireSerializedType(ContentSerializedType.PropertyName);

            // Read value
            name = contentReader.ReadString();

            ReadSerializedType();
            return true;
        }

        public override bool ReadObjectStart(ref TypeReference typeReference)
        {
            RequireSerializedType(ContentSerializedType.ObjectStart);
            bool hasType = contentReader.ReadBoolean();

            // Read type if required
            if (typeReference.IsRequired == true)
            {
                if (hasType == false)
                    throw new InvalidDataException("`$type` specifier is required but was not provided in the serialized data: " + typeReference.TypeName);

                typeReference.TypeName = contentReader.ReadString();                
            }
            ReadSerializedType();
            return true;
        }

        public override bool ReadObjectEnd()
        {
            RequireSerializedType(ContentSerializedType.ObjectEnd);
            ReadSerializedType();
            return true;
        }

        public override bool ReadArrayStart(out int length)
        {
            RequireSerializedType(ContentSerializedType.ArrayStart);

            // Read length
            length = contentReader.ReadInt32();
            ReadSerializedType();
            return true;
        }

        public override bool ReadArrayEnd()
        {
            RequireSerializedType(ContentSerializedType.ArrayEnd);
            ReadSerializedType();
            return true;
        }

        public override bool ReadBoolean(out bool value)
        {
            RequireSerializedType(ContentSerializedType.Bool);

            // Read value
            value = contentReader.ReadBoolean();

            ReadSerializedType();
            return true;
        }

        public override bool ReadChar(out char value)
        {
            RequireSerializedType(ContentSerializedType.Char);

            // Read value
            value = contentReader.ReadChar();

            ReadSerializedType();
            return true;
        }

        public override bool ReadString(out string value)
        {
            RequireSerializedType(ContentSerializedType.String);

            // Read value
            value = contentReader.ReadString();

            ReadSerializedType();
            return true;
        }

        public override bool ReadSByte(out sbyte value)
        {
            RequireSerializedType(ContentSerializedType.Int8);

            // Read value
            value = contentReader.ReadSByte();

            ReadSerializedType();
            return true;
        }

        public override bool ReadInt16(out short value)
        {
            RequireSerializedType(ContentSerializedType.Int16);

            // Read value
            value = contentReader.ReadInt16();

            ReadSerializedType();
            return true;
        }

        public override bool ReadInt32(out int value)
        {
            RequireSerializedType(ContentSerializedType.Int32);

            // Read value
            value = contentReader.ReadInt32();

            ReadSerializedType();
            return true;
        }

        public override bool ReadInt64(out long value)
        {
            RequireSerializedType(ContentSerializedType.Int64);

            // Read value
            value = contentReader.ReadInt64();

            ReadSerializedType();
            return true;
        }

        public override bool ReadByte(out byte value)
        {
            RequireSerializedType(ContentSerializedType.UInt8);

            // Read value
            value = contentReader.ReadByte();

            ReadSerializedType();
            return true;
        }

        public override bool ReadUInt16(out ushort value)
        {
            RequireSerializedType(ContentSerializedType.UInt16);

            // Read value
            value = contentReader.ReadUInt16();

            ReadSerializedType();
            return true;
        }

        public override bool ReadUInt32(out uint value)
        {
            RequireSerializedType(ContentSerializedType.UInt32);

            // Read value
            value = contentReader.ReadUInt32();

            ReadSerializedType();
            return true;
        }

        public override bool ReadUInt64(out ulong value)
        {
            RequireSerializedType(ContentSerializedType.UInt64);

            // Read value
            value = contentReader.ReadUInt64();

            ReadSerializedType();
            return true;
        }

        public override bool ReadSingle(out float value)
        {
            RequireSerializedType(ContentSerializedType.Single);

            // Read value
            value = contentReader.ReadSingle();

            ReadSerializedType();
            return true;
        }

        public override bool ReadDouble(out double value)
        {
            RequireSerializedType(ContentSerializedType.Double);

            // Read value
            value = contentReader.ReadDouble();

            ReadSerializedType();
            return true;
        }

        public override bool ReadDecimal(out decimal value)
        {
            RequireSerializedType(ContentSerializedType.Decimal);

            // Read value
            value = contentReader.ReadDecimal();

            ReadSerializedType();
            return true;
        }

        public override void Skip()
        {

        }

        private void RequireSerializedType(ContentSerializedType serializedType)
        {
            if (peekSerializedType != serializedType)
                throw new InvalidOperationException(string.Format("Attempted to read value of type: {0}, but got: {1}", serializedType, peekSerializedType));
        }

        private void ReadSerializedType()
        {
            try
            {
                peekSerializedType = (ContentSerializedType)contentReader.ReadByte();
            }
            catch(EndOfStreamException)
            {
                peekSerializedType = ContentSerializedType.EOF;
            }
        }
    }
}
