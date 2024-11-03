using System;

namespace UniGameEngine.Content
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CustomSerializerAttribute : Attribute
    {
        // Private
        private Type forType = null;

        // Properties
        public Type ForType
        {
            get { return forType; }
        }

        // Constructor
        public CustomSerializerAttribute(Type forType) 
        { 
            this.forType = forType;
        }
    }
}
