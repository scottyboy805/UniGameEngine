using System;

namespace UniGameEngine
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DataMemberReadOnly : Attribute
    {
        // Empty class
    }
}
