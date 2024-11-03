using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UniGameEngine.Content.Serializers
{
    public enum SerializedType : byte
    {
        Invalid = 0,

        Null,
        String,
        Number,
        Boolean,

        Reference,
        PropertyName,

        ObjectStart,
        ObjectEnd,
        ArrayStart,
        ArrayEnd,

        EndStream,
    }

    public abstract class Serializer
    {
        // Private
        private static readonly TypeManager typeManager = new TypeManager();

        // Primitive serializers
        private static readonly BooleanSerializer booleanSerializer = new BooleanSerializer();        
        private static readonly CharSerializer charSerializer = new CharSerializer();
        private static readonly StringSerializer stringSerializer = new StringSerializer();
        private static readonly SByteSerializer sbyteSerializer = new SByteSerializer();
        private static readonly Int16Serializer int16Serializer = new Int16Serializer();
        private static readonly Int32Serializer int32Serializer = new Int32Serializer();
        private static readonly Int64Serializer int64Serializer = new Int64Serializer();
        private static readonly IntPtrSerializer intPtrSerializer = new IntPtrSerializer();
        private static readonly ByteSerializer byteSerializer = new ByteSerializer();
        private static readonly UInt16Serializer uint16Serializer = new UInt16Serializer();
        private static readonly UInt32Serializer uint32Serializer = new UInt32Serializer();
        private static readonly UInt64Serializer uint64Serializer = new UInt64Serializer();
        private static readonly UIntPtrSerializer uintPtrSerializer = new UIntPtrSerializer();
        private static readonly DecimalSerializer decimalSerializer = new DecimalSerializer();
        private static readonly DoubleSerializer doubleSerializer = new DoubleSerializer();
        private static readonly SingleSerializer singleSerializer = new SingleSerializer();

        private static readonly Dictionary<Type, Type> customSerializers = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, Serializer> cachedSerializers = new Dictionary<Type, Serializer>
        {
            { typeof(bool), booleanSerializer },
            { typeof(char), charSerializer },
            { typeof(string), stringSerializer },
            { typeof(sbyte), sbyteSerializer },
            { typeof(short), int16Serializer },
            { typeof(int), int32Serializer },
            { typeof(long), int64Serializer },
            { typeof(IntPtr), intPtrSerializer },
            { typeof(byte), byteSerializer },
            { typeof(ushort), uint16Serializer },
            { typeof(uint), uint32Serializer },
            { typeof(ulong), uint64Serializer },
            { typeof(UIntPtr), uintPtrSerializer },
            { typeof(decimal), decimalSerializer },
            { typeof(double), doubleSerializer },
            { typeof(float), singleSerializer },
        };

        // Properties
        public static TypeManager TypeManager
        {
            get { return typeManager; }
        }

        // Constructor
        static Serializer()
        {
            InitializeSerializers();
        }

        // Methods
        public abstract void WriteValueObject(SerializedWriter writer, object value);
        public abstract void ReadValueObject(SerializedReader reader, Type type, ref object value);

        public static void Serialize<T>(SerializedWriter writer, T value)
        {
            // Check for null
            if(writer == null)
                throw new ArgumentNullException(nameof(writer));

            // Get serializer
            Serializer<T> serializer = Get<T>();

            // Check for no serializer
            if (serializer == null)
                throw new NotSupportedException("No serializer found for type: " + typeof(T));

            // Serialize value
            serializer.WriteValue(writer, value);
        }

        public static void Serialize(SerializedWriter writer, object value)
        {
            // Check for null
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            // Check for null
            if(value == null)
            {
                writer.WriteNull();
                return;
            }

            // Get type
            Type type = value.GetType();

            // Get serializer
            Serializer serializer = Get(type);

            // Check for no serializer
            if (serializer == null)
                throw new NotSupportedException("No serializer found for type: " + type);

            // Serialize value
            serializer.WriteValueObject(writer, value);
        }

        public static T Deserialize<T>(SerializedReader reader)
        {
            // Deserialize
            T value = default;
            Deserialize(reader, ref value);

            // Get instance
            return value;
        }

        public static void Deserialize<T>(SerializedReader reader, ref T value)
        {
            // Check for null
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            // Check for null
            if(reader.PeekType == SerializedType.Null)
            {
                reader.ReadNull();
                value = default;
                return;
            }

            // Get serializer
            Serializer<T> serializer = Get<T>();

            // Check for no serializer
            if (serializer == null)
                throw new NotSupportedException("No serializer found for type: " + typeof(T));

            // Deserialize value
            serializer.ReadValue(reader, ref value);
        }

        public static object Deserialize(SerializedReader reader, Type type)
        {
            // Deserialize
            object value = default;
            Deserialize(reader, type, ref value);

            // Get instance
            return value;
        }

        public static void Deserialize(SerializedReader reader, Type type, ref object value)
        {
            // Check for null
            if(reader == null)
                throw new ArgumentNullException(nameof(reader));

            if(type == null) 
                throw new ArgumentNullException(nameof(type));

            // Check for null
            if (reader.PeekType == SerializedType.Null)
            {
                reader.ReadNull();
                value = default;
                return;
            }

            // Get serializer
            Serializer serializer = Get(type);

            // Check for no serializer
            if (serializer == null)
                throw new NotSupportedException("No serializer found for type: " + type);

            // Deserialize value
            serializer.ReadValueObject(reader, type, ref value);
        }

        public static Serializer<T> Get<T>()
        {
            // Get base
            Serializer serializer = Get(typeof(T));

            // Check for base
            if (serializer is Serializer<T> result)
                return result;

            // Not supported
            throw new NotSupportedException("No serializer found for type: " + typeof(T));
        }

        public static Serializer Get(Type type)
        {
            // Check for cached
            Serializer result;
            if (cachedSerializers.TryGetValue(type, out result) == true)
                return result;

            // Check for primitive
            if(type.IsPrimitive == true)
            {
                // Not supported unless already cached
                return null;
            }

            Type serializerType = null;

            // Check for enum
            if(type.IsEnum == true)
            {
                // Create enum serializer
                serializerType = typeof(EnumSerializer<>).MakeGenericType(type);
            }
            // Check for array
            else if(type.IsArray == true)
            {
                // Check rank - multidimensional arrays are not supported
                if (type.GetArrayRank() != 1)
                    return null;

                // Create array serializer
                serializerType = typeof(ArraySerializer<>).MakeGenericType(type.GetElementType());
            }
            

            // Check for type found
            if(serializerType != null)
            {
                // Create the instance
                Serializer serializer = (Serializer)Activator.CreateInstance(serializerType);

                // Cache the serializer
                cachedSerializers[type] = serializer;
                return serializer;
            }
            // Check for custom serializer
            else
            {
                Type lookupType = type;
                bool isGeneric = type.IsGenericType == true && type.IsGenericTypeDefinition == false;

                // Check for generic
                if (isGeneric == true)
                    lookupType = type.GetGenericTypeDefinition();

                // Try to resolve type
                if(customSerializers.TryGetValue(lookupType, out serializerType) == true)
                {
                    // Make generic with provided args
                    Type finalGenericSerializerType = serializerType.MakeGenericType(type.GenericTypeArguments);

                    // Create the instance
                    Serializer serializer = (Serializer)Activator.CreateInstance(finalGenericSerializerType);

                    // Cache the serializer
                    cachedSerializers[type] = serializer;
                    return serializer;
                }
            }

            // Fallback to object serializer
            if (type.IsClass == true || type.IsValueType == true)
            {
                // get serializer type
                Type objectSerializerType = typeof(ObjectSerializer<>).MakeGenericType(type);

                // Create the instance
                Serializer serializer = (Serializer)Activator.CreateInstance(objectSerializerType);

                // Cache the serializer
                cachedSerializers[type] = serializer;
                return serializer;
            }

            // No serializer
            return null;
        }

        private static void InitializeSerializers()
        {
            // Get this assembly name
            Assembly thisAsm = typeof(Serializer).Assembly;
            AssemblyName thisName = thisAsm.GetName();

            // Process all assemblies
            foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Check if we should scan
                bool shouldCheckAssembly = thisAsm == asm;

                // Check for referenced
                if (shouldCheckAssembly == false)
                {
                    // Get references
                    AssemblyName[] referenceNames = asm.GetReferencedAssemblies();

                    // Check for assembly referenced
                    foreach (AssemblyName referenceName in referenceNames)
                    {
                        if (referenceName.FullName == thisName.FullName)
                        {
                            shouldCheckAssembly = true;
                            break;
                        }
                    }
                }

                // Check for skip
                if (shouldCheckAssembly == false)
                    continue;

                try
                {
                    // Check all types
                    foreach (Type type in asm.GetTypes())
                    {
                        // Check for attribute
                        if (type.IsDefined(typeof(CustomSerializerAttribute)) == true)
                        {
                            // Get the attribute
                            CustomSerializerAttribute attrib = type.GetCustomAttribute<CustomSerializerAttribute>();

                            // Check generics
                            if (attrib.ForType.IsGenericType == true)
                            {
                                if (attrib.ForType.GenericTypeArguments.Length != type.GenericTypeArguments.Length)
                                {
                                    Debug.LogErrorF("Custom serializer could not be registered because the generic type arguments are incompatible. Expected '{0}' generic arguments", attrib.ForType.GenericTypeArguments.Length);
                                    continue;
                                }
                            }

                            // Register serializer
                            customSerializers[attrib.ForType] = type;
                        }
                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

    public abstract class Serializer<T> : Serializer
    {
        // Methods
        public abstract void WriteValue(SerializedWriter writer, T value);
        public abstract void ReadValue(SerializedReader reader, ref T value);


        public override void WriteValueObject(SerializedWriter writer, object value)
        {
            WriteValue(writer, (T)value);
        }

        public override void ReadValueObject(SerializedReader reader, Type type, ref object value)
        {
            if (value == null)
            {
                // Deserialize into temp
                T impl = default;
                ReadValue(reader, ref impl);

                // Update value
                value = impl;
            }
            else
            {
                // Check type
                if ((value is T) == false)
                    throw new ArgumentException(string.Format("Cannot deserialize object of type: {0}, as type: {1}", value, typeof(T)));

                // Deserialize into ref
                T impl = (T)value;
                ReadValue(reader, ref impl);

                // Need to copy back the reference for value type otherwise deserialization fails
                if (typeof(T).IsValueType == true)
                    value = impl;
            }
        }
    }
}


