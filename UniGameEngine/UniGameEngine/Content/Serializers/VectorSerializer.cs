using Microsoft.Xna.Framework;
using System;

namespace UniGameEngine.Content.Serializers
{
    [CustomSerializer(typeof(Vector2))]
    internal sealed class Vector2Serializer : Serializer<Vector2>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref Vector2 value)
        {
            // Expect object
            reader.ReadObjectStart();
            {
                // X
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.X);
                // Y
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.Y);
            }
            reader.ReadObjectEnd();
        }

        public override void WriteValue(SerializedWriter writer, Vector2 value)
        {
            writer.WriteObjectStart();
            {
                // X
                writer.WritePropertyName("X"); 
                writer.WriteSingle(value.X);
                // Y
                writer.WritePropertyName("Y");
                writer.WriteSingle(value.Y);
            }
            writer.WriteObjectEnd();
        }
    }

    [CustomSerializer(typeof(Vector3))]
    internal sealed class Vector3Serialize : Serializer<Vector3>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref Vector3 value)
        {
            // Expect object
            reader.ReadObjectStart();
            {
                // X
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.X);
                // Y
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.Y);
                // Z
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.Z);
            }
            reader.ReadObjectEnd();
        }

        public override void WriteValue(SerializedWriter writer, Vector3 value)
        {
            writer.WriteObjectStart();
            {
                // X
                writer.WritePropertyName("X");
                writer.WriteSingle(value.X);
                // Y
                writer.WritePropertyName("Y");
                writer.WriteSingle(value.Y);
                // Z
                writer.WritePropertyName("Z");
                writer.WriteSingle(value.Z);
            }
            writer.WriteObjectEnd();
        }
    }

    [CustomSerializer(typeof(Vector4))]
    internal sealed class Vector4Serialize : Serializer<Vector4>
    {
        // Methods
        public override void ReadValue(SerializedReader reader, ref Vector4 value)
        {
            // Expect object
            reader.ReadObjectStart();
            {
                // X
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.X);
                // Y
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.Y);
                // Z
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.Z);
                // W
                reader.ReadPropertyName(out _);
                reader.ReadSingle(out value.W);
            }
            reader.ReadObjectEnd();
        }

        public override void WriteValue(SerializedWriter writer, Vector4 value)
        {
            writer.WriteObjectStart();
            {
                // X
                writer.WritePropertyName("X");
                writer.WriteSingle(value.X);
                // Y
                writer.WritePropertyName("Y");
                writer.WriteSingle(value.Y);
                // Z
                writer.WritePropertyName("Z");
                writer.WriteSingle(value.Z);
                // W
                writer.WritePropertyName("W");
                writer.WriteSingle(value.W);
            }
            writer.WriteObjectEnd();
        }
    }
}
