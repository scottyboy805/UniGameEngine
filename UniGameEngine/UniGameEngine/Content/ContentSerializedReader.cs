
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
    }

    internal sealed class ContentSerializedReader
    {
    }
}
