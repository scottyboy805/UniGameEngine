using Newtonsoft.Json;
using System.IO;
using UniGameEngine.Content.Serializers;

namespace UniGameEngine.Content
{
    internal sealed class JsonSerializedReader : SerializedReader
    {
        // Private
        private JsonReader reader = null;

        // Properties
        public override SerializedType PeekType
        {
            get
            {
                return reader.TokenType switch
                { 
                    JsonToken.Null => SerializedType.Null,
                    JsonToken.Float => SerializedType.Number,
                    JsonToken.Integer => SerializedType.Number,
                    JsonToken.String => SerializedType.String,
                    JsonToken.Boolean => SerializedType.Boolean,
                    JsonToken.StartArray => SerializedType.ArrayStart,
                    JsonToken.EndArray => SerializedType.ArrayEnd,
                    JsonToken.StartObject => SerializedType.ObjectStart,
                    JsonToken.EndObject => SerializedType.ObjectEnd,
                    JsonToken.PropertyName => SerializedType.PropertyName,
                    _ => SerializedType.Invalid,
                };
            }
        }

        // Constructor
        public JsonSerializedReader(JsonReader reader)
        {
            this.reader = reader;

            // Ignore invalid tokens
            SkipInvalid();
        }

        static JsonSerializedReader()
        {
            Serializer.TypeManager.RegisterAssembly(typeof(UniGame).Assembly);
        }

        // Methods
        public override void Dispose()
        {
            reader.Close();
            reader = null;
        }

        public override void Skip()
        {
            reader.Read();

            // Ignore invalid tokens
            SkipInvalid();
        }

        public override bool ReadNull()
        {
            // Check for null
            if (reader.TokenType == JsonToken.Null)
            {
                bool result = reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return result;
            }
            return false;
        }

        public override bool ReadPropertyName(out string name)
        {
            name = default;

            // Check for property
            if (reader.TokenType == JsonToken.PropertyName)
            {
                name = (string)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadObjectStart(ref TypeReference typeReference)
        {
            // Check for object start
            if (reader.TokenType == JsonToken.StartObject)
            {
                bool result = reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
            }

            // Require type if
            if (typeReference.IsRequired == true)
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    // Get the property name
                    string propertyName = (string)reader.Value;
                    reader.Read();

                    // Check for type
                    if (propertyName != TypeReference.TypeSpecifier)
                        throw new InvalidDataException("`" + TypeReference.TypeSpecifier + "` specifier must be provided");

                    // Ignore invalid tokens
                    SkipInvalid();

                    // Read type id
                    typeReference.TypeName = (string)reader.Value;
                    reader.Read();

                    // Ignore invalid tokens
                    SkipInvalid();
                    return true;
                }
            }
            else
                return true;
            return false;
        }

        public override bool ReadObjectEnd()
        {
            // Check for object end
            if (reader.TokenType == JsonToken.EndObject)
            {
                bool result = reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadArrayStart(out int length)
        {
            length = -1;

            // Check for array start
            if (reader.TokenType == JsonToken.StartArray)
            {
                bool result = reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return result;
            }
            return false;
        }

        public override bool ReadArrayEnd()
        {
            // Check for array end
            if (reader.TokenType == JsonToken.EndArray)
            {
                bool result = reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return result;
            }
            return false;
        }

        public override bool ReadSerializedReference(ref SerializedReference serializedReference)
        {
            // Check for string
            if(reader.TokenType == JsonToken.String)
            {
                // Get value
                string value = (string)reader.Value;
                bool result = reader.Read();

                // Parse reference
                serializedReference = SerializedReference.Parse(value);

                // Ignore invalid tokens
                SkipInvalid();
                return result;
            }
            return false;
        }

        public override bool ReadBoolean(out bool value)
        {
            value = default;

            // Check for bool
            if (reader.TokenType == JsonToken.Boolean)
            {
                value = (bool)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadChar(out char value)
        {
            value = default;

            // Check for string
            if (reader.TokenType == JsonToken.String)
            {
                string literal = (string)reader.Value;

                // Try to parse character
                value = string.IsNullOrEmpty(literal) == false
                    ? literal[0]
                    : default;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadString(out string value)
        {
            value = default;

            // Check for string
            if (reader.TokenType == JsonToken.String)
            {
                value = (string)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadByte(out byte value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Integer)
            {
                value = (byte)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }        

        public override bool ReadInt16(out short value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Integer)
            {
                value = (short)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadInt32(out int value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Integer)
            {
                value = (int)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadInt64(out long value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Integer)
            {
                value = (long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadSByte(out sbyte value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Integer)
            {
                value = (sbyte)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadUInt16(out ushort value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Integer)
            {
                value = (ushort)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadUInt32(out uint value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Integer)
            {
                value = (uint)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadUInt64(out ulong value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Integer)
            {
                value = (ulong)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadDecimal(out decimal value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Float)
            {
                value = (decimal)(double)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            // Check for non-decimal number
            else if (reader.TokenType == JsonToken.Integer)
            {
                value = (decimal)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadDouble(out double value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Float)
            {
                value = (double)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            // Check for non-decimal number
            else if (reader.TokenType == JsonToken.Integer)
            {
                value = (double)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        public override bool ReadSingle(out float value)
        {
            value = default;

            // Check for number
            if (reader.TokenType == JsonToken.Float)
            {
                value = (float)(double)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            // Check for non-decimal number
            else if(reader.TokenType == JsonToken.Integer)
            {
                value = (float)(long)reader.Value;
                reader.Read();

                // Ignore invalid tokens
                SkipInvalid();
                return true;
            }
            return false;
        }

        private void SkipInvalid()
        {
            while (reader.TokenType == JsonToken.None || reader.TokenType == JsonToken.Comment)
            {
                // Consume token
                if (reader.Read() == false)
                    break;
            }
        }
    }
}
