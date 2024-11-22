using System;
using System.Collections;

namespace UniGameEngine.Content.Contract
{
    internal sealed class DataContractElement : DataContractProperty
    {
        // Private
        private int index = 0;
        private bool readOnly = false;

        // Properties
        public override bool IsReadOnly => readOnly;

        // Constructor
        public DataContractElement(Type elementType, int index, bool readOnly = false)
            : base(index.ToString(), elementType)
        {
            this.index = index;
            this.readOnly = readOnly;
            this.dataType = DataType.ArrayElement;
        }

        // Methods
        protected override object GetInstanceValueImpl(object instance)
        {
            // Check for array
            if(instance is IList list)
                return list[index];

            throw new InvalidOperationException("Instance must be an array or collection");
        }

        protected override object SetInstanceValueImpl(object instance, object value)
        {
            // Check for array
            if (instance is IList list)
                list[index] = value;

            throw new InvalidOperationException("Instance must be an array or collection");
        }

        protected override T GetAttributeImpl<T>()
        {
            return null;
        }

        public override string ToString()
        {
            return string.Format("Data Element ({0}): {1}", index, PropertyType);
        }
    }
}
