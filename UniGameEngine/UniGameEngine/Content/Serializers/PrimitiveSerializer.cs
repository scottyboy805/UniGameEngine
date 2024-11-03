
using System;

namespace UniGameEngine.Content.Serializers
{
    /// <summary>
    /// Serializer for the <see cref="bool"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Boolean}" />
    public sealed class BooleanSerializer : Serializer<bool>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref bool value)
        {
            // Expect bool
            reader.Expect(SerializedType.Boolean);

            // Try to read bool
            reader.ReadBoolean(out value);
        }

        public override void WriteValue(SerializedWriter writer, bool value)
        {
            writer.WriteBoolean(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="byte"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Byte}" />
    public sealed class ByteSerializer : Serializer<byte>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref byte value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read byte
            reader.ReadByte(out value);
        }

        public override void WriteValue(SerializedWriter writer, byte value)
        {
            writer.WriteByte(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="char"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Char}" />
    public sealed class CharSerializer : Serializer<char>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref char value)
        {
            // Expect string
            reader.Expect(SerializedType.String);

            // Try to read char
            reader.ReadChar(out value);
        }

        public override void WriteValue(SerializedWriter writer, char value)
        {
            writer.WriteChar(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="decimal"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Decimal}" />
    public sealed class DecimalSerializer : Serializer<decimal>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref decimal value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read decimal
            reader.ReadDecimal(out value);
        }

        public override void WriteValue(SerializedWriter writer, decimal value)
        {
            writer.WriteDecimal(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="double"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Double}" />
    public sealed class DoubleSerializer : Serializer<double>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref double value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read double
            reader.ReadDouble(out value);
        }

        public override void WriteValue(SerializedWriter writer, double value)
        {
            writer.WriteDouble(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="short"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Int16}" />
    public sealed class Int16Serializer : Serializer<short>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref short value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read short
            reader.ReadInt16(out value);
        }

        public override void WriteValue(SerializedWriter writer, short value)
        {
            writer.WriteInt16(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="int"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Int32}" />
    public sealed class Int32Serializer : Serializer<int>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref int value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read int
            reader.ReadInt32(out value);
        }

        public override void WriteValue(SerializedWriter writer, int value)
        {
            writer.WriteInt32(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="long"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Int64}" />
    public sealed class Int64Serializer : Serializer<long>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref long value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read long
            reader.ReadInt64(out value);
        }

        public override void WriteValue(SerializedWriter writer, long value)
        {
            writer.WriteInt64(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="IntPtr"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.IntPtr}" />
    public sealed class IntPtrSerializer : Serializer<IntPtr>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref IntPtr value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read long
            long valueTemp;
            reader.ReadInt64(out valueTemp);

            // Get as IntPtr
            value = new IntPtr(valueTemp);
        }

        public override void WriteValue(SerializedWriter writer, IntPtr value)
        {
            writer.WriteInt64(value.ToInt64());
        }
    }

    /// <summary>
    /// Serializer for the <see cref="sbyte"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.SByte}" />
    public sealed class SByteSerializer : Serializer<sbyte>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref sbyte value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read sbyte
            reader.ReadSByte(out value);
        }

        public override void WriteValue(SerializedWriter writer, sbyte value)
        {
            writer.WriteSByte(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="float"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.Single}" />
    public sealed class SingleSerializer : Serializer<float>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref float value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read single
            reader.ReadSingle(out value);
        }

        public override void WriteValue(SerializedWriter writer, float value)
        {
            writer.WriteSingle(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="string"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.String}" />
    public sealed class StringSerializer : Serializer<string>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref string value)
        {
            // Check for null
            if(reader.PeekType == SerializedType.Null)
            {
                reader.ReadNull();
                value = null;
                return;
            }

            // Expect string
            reader.Expect(SerializedType.String);

            // Try to read string
            reader.ReadString(out value);
        }

        public override void WriteValue(SerializedWriter writer, string value)
        {
            // Check for null
            if(value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteString(value);
            }
        }
    }

    /// <summary>
    /// Serializer for the <see cref="ushort"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.UInt16}" />
    public sealed class UInt16Serializer : Serializer<ushort>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref ushort value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read ushort
            reader.ReadUInt16(out value);
        }

        public override void WriteValue(SerializedWriter writer, ushort value)
        {
            writer.WriteUInt16(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="uint"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.UInt32}" />
    public sealed class UInt32Serializer : Serializer<uint>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref uint value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read int
            reader.ReadUInt32(out value);
        }

        public override void WriteValue(SerializedWriter writer, uint value)
        {
            writer.WriteUInt32(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="ulong"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.UInt64}" />
    public sealed class UInt64Serializer : Serializer<ulong>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref ulong value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read ulong
            reader.ReadUInt64(out value);
        }

        public override void WriteValue(SerializedWriter writer, ulong value)
        {
            writer.WriteUInt64(value);
        }
    }

    /// <summary>
    /// Serializer for the <see cref="UIntPtr"/> type.
    /// </summary>
    /// <seealso cref="Serializer{System.UIntPtr}" />
    public sealed class UIntPtrSerializer : Serializer<UIntPtr>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref UIntPtr value)
        {
            // Expect number
            reader.Expect(SerializedType.Number);

            // Try to read ulong
            ulong valueTemp;
            reader.ReadUInt64(out valueTemp);

            // Get as uintptr
            value = new UIntPtr(valueTemp);
        }

        public override void WriteValue(SerializedWriter writer, UIntPtr value)
        {
            writer.WriteUInt64(value.ToUInt64());
        }
    }
}
