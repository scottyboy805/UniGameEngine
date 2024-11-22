using System;

namespace UniGameEngine
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DataMemberHideInEditor : Attribute
    {
        // Empty class
    }
}
