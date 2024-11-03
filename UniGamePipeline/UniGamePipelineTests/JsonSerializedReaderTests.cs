using Assimp;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UniGameEngine.Content;
using UniGameEngine.Content.Serializers;

namespace UniGamePipeline.Tests
{
    [TestClass()]
    public class JsonSerializedReaderTests
    {
        [DataContract]
        sealed class TestObject
        {
            public string StringVal;
            public string StringValEmpty;
            public string StringValNull;

            public bool BoolFalseVal;
            public bool BoolTrueVal;
            public char CharVal;
            public byte ByteVal;
            public short ShortVal;
            public int IntVal;
            public long LongVal;
            public sbyte SByteVal;
            public ushort UShortVal;
            public uint UIntVal;
            public ulong ULongVal;
            
            public float FloatVal;
            public double DoubleVal;
            public decimal DecimalVal;

            public object ObjectVal;
        }

        [DataContract]
        struct TestValueObject
        {
            public string StringVal;
            public string StringValEmpty;
            public string StringValNull;

            public bool BoolFalseVal;
            public bool BoolTrueVal;
            public char CharVal;
            public byte ByteVal;
            public short ShortVal;
            public int IntVal;
            public long LongVal;
            public sbyte SByteVal;
            public ushort UShortVal;
            public uint UIntVal;
            public ulong ULongVal;

            public float FloatVal;
            public double DoubleVal;
            public decimal DecimalVal;

            public object ObjectVal;
        }

        [DataContract]
        sealed class TestObjectRoot
        {
            [DataContract]
            public sealed class TestObjectChild1
            {
                [DataContract]
                public sealed class TestObjectChild2
                {
                    public string TestValue;
                }
                public TestObjectChild2 Child2;
            }
            public TestObjectChild1 Child1;
        }

        [DataContract]
        sealed class TestObjectElement
        {
            public int Number;
        }

        #region Object
        [TestMethod()]
        public void ObjectTest()
        {
            string source = @"
{
    ""StringVal"": ""Test String"",
    ""StringValEmpty"": """",
    ""StringValNull"": null,

    ""BoolFalseVal"": false,
    ""BoolTrueVal"": true,
    ""CharVal"": ""a"",
    ""ByteVal"": 123,
    ""ShortVal"": 123,
    ""IntVal"": 123,
    ""LongVal"": 123,
    ""SByteVal"": 123,
    ""UShortVal"": 123,
    ""UIntVal"": 123,
    ""ULongVal"": 123,

    ""FloatVal"": 123.456,
    ""DoubleVal"": 123.456,
    ""DecimalVal"": 123.456,

    ""ObjectVal"": null
}";

            using(JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize generic
                TestResults(Serializer.Deserialize<TestObject>(reader));
            }

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize generic existing
                TestObject obj = new TestObject();
                Serializer.Deserialize(reader, ref obj);
                TestResults(obj);
            }

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize non-generic                
                TestResults((TestObject)Serializer.Deserialize(reader, typeof(TestObject)));
            }

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize non-generic existing
                object obj = new TestObject();
                Serializer.Deserialize(reader, typeof(TestObject), ref obj);
                TestResults((TestObject)obj);
            }

            void TestResults(TestObject result)
            {
                Assert.IsNotNull(result);

                Assert.AreEqual("Test String", result.StringVal);
                Assert.AreEqual("", result.StringValEmpty);
                Assert.AreEqual(null, result.StringValNull);

                Assert.AreEqual(false, result.BoolFalseVal);
                Assert.AreEqual(true, result.BoolTrueVal);
                Assert.AreEqual('a', result.CharVal);
                Assert.AreEqual((byte)123, result.ByteVal);
                Assert.AreEqual((short)123, result.ShortVal);
                Assert.AreEqual((int)123, result.IntVal);
                Assert.AreEqual((long)123, result.LongVal);
                Assert.AreEqual((sbyte)123, result.SByteVal);
                Assert.AreEqual((ushort)123, result.UShortVal);
                Assert.AreEqual((uint)123, result.UIntVal);
                Assert.AreEqual((ulong)123, result.ULongVal);

                Assert.AreEqual(123.456f, result.FloatVal);
                Assert.AreEqual(123.456, result.DoubleVal);
                Assert.AreEqual((decimal)123.456, result.DecimalVal);

                Assert.AreEqual(null, result.ObjectVal);
            }
        }

        [TestMethod()]
        public void ValueObjectTest()
        {
            string source = @"
{
    ""StringVal"": ""Test String"",
    ""StringValEmpty"": """",
    ""StringValNull"": null,

    ""BoolFalseVal"": false,
    ""BoolTrueVal"": true,
    ""CharVal"": ""a"",
    ""ByteVal"": 123,
    ""ShortVal"": 123,
    ""IntVal"": 123,
    ""LongVal"": 123,
    ""SByteVal"": 123,
    ""UShortVal"": 123,
    ""UIntVal"": 123,
    ""ULongVal"": 123,

    ""FloatVal"": 123.456,
    ""DoubleVal"": 123.456,
    ""DecimalVal"": 123.456,

    ""ObjectVal"": null
}";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize generic
                TestResults(Serializer.Deserialize<TestValueObject>(reader));
            }

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize generic existing
                TestValueObject obj = new TestValueObject();
                Serializer.Deserialize(reader, ref obj);
                TestResults(obj);
            }

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize non-generic                
                TestResults((TestValueObject)Serializer.Deserialize(reader, typeof(TestValueObject)));
            }

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize non-generic existing
                object obj = new TestValueObject();
                Serializer.Deserialize(reader, typeof(TestValueObject), ref obj);
                TestResults((TestValueObject)obj);
            }

            void TestResults(in TestValueObject result)
            {
                Assert.IsNotNull(result);

                Assert.AreEqual("Test String", result.StringVal);
                Assert.AreEqual("", result.StringValEmpty);
                Assert.AreEqual(null, result.StringValNull);

                Assert.AreEqual(false, result.BoolFalseVal);
                Assert.AreEqual(true, result.BoolTrueVal);
                Assert.AreEqual('a', result.CharVal);
                Assert.AreEqual((byte)123, result.ByteVal);
                Assert.AreEqual((short)123, result.ShortVal);
                Assert.AreEqual((int)123, result.IntVal);
                Assert.AreEqual((long)123, result.LongVal);
                Assert.AreEqual((sbyte)123, result.SByteVal);
                Assert.AreEqual((ushort)123, result.UShortVal);
                Assert.AreEqual((uint)123, result.UIntVal);
                Assert.AreEqual((ulong)123, result.ULongVal);

                Assert.AreEqual(123.456f, result.FloatVal);
                Assert.AreEqual(123.456, result.DoubleVal);
                Assert.AreEqual((decimal)123.456, result.DecimalVal);

                Assert.AreEqual(null, result.ObjectVal);
            }
        }

        [TestMethod]
        public void NestedObjectTest()
        {
            string source = @"
{
    ""TestObjectRoot"": {
        ""Child1"": {
            ""Child2"": {
                ""TestValue"": ""Test String""
            }
        }
    }
}";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                TestObjectRoot result = Serializer.Deserialize<TestObjectRoot>(reader);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Child1);
                Assert.IsNotNull(result.Child1.Child2);
                Assert.AreEqual("Test String", result.Child1.Child2.TestValue);
            }
        }
        #endregion

        #region Array
        [TestMethod]
        public void ArrayStringTest()
        {
            string source = @"
[
    ""String1"",
    ""String2"",
    ""String3""
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                string[] result = Serializer.Deserialize<string[]>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Length);
                Assert.AreEqual("String1", result[0]);
                Assert.AreEqual("String2", result[1]);
                Assert.AreEqual("String3", result[2]);
            }
        }

        [TestMethod]
        public void ArrayNumberTest()
        {
            string source = @"
[
    10,
    20,
    30
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                int[] result = Serializer.Deserialize<int[]>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Length);
                Assert.AreEqual(10, result[0]);
                Assert.AreEqual(20, result[1]);
                Assert.AreEqual(30, result[2]);
            }
        }

        [TestMethod]
        public void ArrayBoolTest()
        {
            string source = @"
[
    true,
    false,
    true
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                bool[] result = Serializer.Deserialize<bool[]>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Length);
                Assert.AreEqual(true, result[0]);
                Assert.AreEqual(false, result[1]);
                Assert.AreEqual(true, result[2]);
            }
        }

        [TestMethod]
        public void ArrayObjectTest()
        {
            string source = @"
[
    { ""Number"": 10 },
    { ""Number"": 20 },
    { ""Number"": 30 }
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                TestObjectElement[] result = Serializer.Deserialize<TestObjectElement[]>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Length);
                Assert.AreEqual(10, result[0].Number);
                Assert.AreEqual(20, result[1].Number);
                Assert.AreEqual(30, result[2].Number);
            }
        }
        #endregion

        #region List
        [TestMethod]
        public void ListStringTest()
        {
            string source = @"
[
    ""String1"",
    ""String2"",
    ""String3""
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                List<string> result = Serializer.Deserialize<List<string>>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual("String1", result[0]);
                Assert.AreEqual("String2", result[1]);
                Assert.AreEqual("String3", result[2]);
            }
        }

        [TestMethod]
        public void ListNumberTest()
        {
            string source = @"
[
    10,
    20,
    30
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                List<int> result = Serializer.Deserialize<List<int>>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual(10, result[0]);
                Assert.AreEqual(20, result[1]);
                Assert.AreEqual(30, result[2]);
            }
        }

        [TestMethod]
        public void ListBoolTest()
        {
            string source = @"
[
    true,
    false,
    true
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
               List<bool> result = Serializer.Deserialize<List<bool>>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual(true, result[0]);
                Assert.AreEqual(false, result[1]);
                Assert.AreEqual(true, result[2]);
            }
        }

        [TestMethod]
        public void ListObjectTest()
        {
            string source = @"
[
    { ""Number"": 10 },
    { ""Number"": 20 },
    { ""Number"": 30 }
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                List<TestObjectElement> result = Serializer.Deserialize<List<TestObjectElement>>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual(10, result[0].Number);
                Assert.AreEqual(20, result[1].Number);
                Assert.AreEqual(30, result[2].Number);
            }
        }
        #endregion

        #region Dictionary
        [TestMethod]
        public void DictionaryTest()
        {
            string source = @"
[
    {
        ""Key"": 10,
        ""Value"": ""Value1""
    },
    {
        ""Key"": 20,
        ""Value"": ""Value2""
    },
    {
        ""Key"": 30,
        ""Value"": ""Value3""
    }
]
";

            using (JsonSerializedReader reader = CreateReader(source))
            {
                // Deserialize
                Dictionary<int, string> result = Serializer.Deserialize<Dictionary<int, string>>(reader);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual("Value1", result[10]);
                Assert.AreEqual("Value2", result[20]);
                Assert.AreEqual("Value3", result[30]);
            }
        }
        #endregion

        private static JsonSerializedReader CreateReader(string source)
        {
            // Create reader
            JsonTextReader reader = new JsonTextReader(new StringReader(source));

            // Create serializer reader
            return new JsonSerializedReader(reader);
        }
    }
}