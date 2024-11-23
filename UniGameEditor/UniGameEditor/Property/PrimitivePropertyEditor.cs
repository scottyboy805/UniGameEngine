using UniGameEditor.UI;

namespace UniGameEditor.Property
{
    [PropertyEditorFor(typeof(bool))]
    internal sealed class PrimitiveBoolPropertyEditor : PropertyEditor
    {
        // Methods
        protected override void OnShow()
        {
            // Add layout
            EditorLayoutControl layout = RootControl.AddDirectionalLayout(EditorLayoutDirection.Horizontal);

            // Add property label
            EditorPropertyLabel propertyLabel = layout.AddPropertyLabel(Property);
            propertyLabel.Width = PropertyLabelWidth;

            // Get the current value
            bool value;
            bool isMixed;
            Property.GetValue(out value, out isMixed);

            // Create toggle field
            EditorToggle toggle = layout.AddToggle(value);

            // Check for read only
            toggle.IsReadOnly = Property.IsReadOnly;
        }
    }

    [PropertyEditorFor(typeof(string))]
    internal sealed class PrimitiveStringPropertyEditor : PropertyEditor
    {
        // Methods
        protected override void OnShow()
        {
            // Add layout
            EditorLayoutControl layout = RootControl.AddDirectionalLayout(EditorLayoutDirection.Horizontal);

            // Add property label
            EditorPropertyLabel propertyLabel = layout.AddPropertyLabel(Property);
            propertyLabel.Width = PropertyLabelWidth;

            // Get the current value
            string value;
            bool isMixed;
            Property.GetValue(out value, out isMixed);

            // Create toggle field
            EditorInput input = layout.AddInput(value);

            // Check for read only
            input.IsReadOnly = Property.IsReadOnly;
        }
    }

    #region PrimitiveNumber
    [PropertyEditorFor(typeof(char))]
    internal sealed class PrimitiveCharPropertyEditor : PrimitiveNumberPropertyEditor<char>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, char value, bool isMixed)
        {
            return layout.AddNumberInput(value, char.MinValue, char.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(sbyte))]
    internal sealed class PrimitiveSBytePropertyEditor : PrimitiveNumberPropertyEditor<sbyte>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, sbyte value, bool isMixed)
        {
            return layout.AddNumberInput(value, sbyte.MinValue, sbyte.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(short))]
    internal sealed class PrimitiveInt16PropertyEditor : PrimitiveNumberPropertyEditor<short>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, short value, bool isMixed)
        {
            return layout.AddNumberInput(value, short.MinValue, short.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(int))]
    internal sealed class PrimitiveInt32PropertyEditor : PrimitiveNumberPropertyEditor<int>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, int value, bool isMixed)
        {
            return layout.AddNumberInput(value, int.MinValue, int.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(long))]
    internal sealed class PrimitiveInt64PropertyEditor : PrimitiveNumberPropertyEditor<long>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, long value, bool isMixed)
        {
            return layout.AddNumberInput(value, long.MinValue, long.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(byte))]
    internal sealed class PrimitiveBytePropertyEditor : PrimitiveNumberPropertyEditor<byte>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, byte value, bool isMixed)
        {
            return layout.AddNumberInput(value, byte.MinValue, byte.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(ushort))]
    internal sealed class PrimitiveUInt16PropertyEditor : PrimitiveNumberPropertyEditor<ushort>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, ushort value, bool isMixed)
        {
            return layout.AddNumberInput(value, ushort.MinValue, ushort.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(uint))]
    internal sealed class PrimitiveUInt32PropertyEditor : PrimitiveNumberPropertyEditor<uint>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, uint value, bool isMixed)
        {
            return layout.AddNumberInput(value, uint.MinValue, uint.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(ulong))]
    internal sealed class PrimitiveUInt64PropertyEditor : PrimitiveNumberPropertyEditor<ulong>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, ulong value, bool isMixed)
        {
            return layout.AddNumberInput(value, ulong.MinValue, ulong.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(float))]
    internal sealed class PrimitiveSinglePropertyEditor : PrimitiveNumberPropertyEditor<float>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, float value, bool isMixed)
        {
            return layout.AddNumberInput(value, float.MinValue, float.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(double))]
    internal sealed class PrimitiveDoublePropertyEditor : PrimitiveNumberPropertyEditor<double>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, double value, bool isMixed)
        {
            return layout.AddNumberInput(value, double.MinValue, double.MaxValue);
        }
    }

    [PropertyEditorFor(typeof(decimal))]
    internal sealed class PrimitiveDecimalPropertyEditor : PrimitiveNumberPropertyEditor<decimal>
    {
        // Methods
        protected override EditorNumberInput CreateNumberInput(EditorLayoutControl layout, decimal value, bool isMixed)
        {
            return layout.AddNumberInput((double)value, (double)decimal.MinValue, (double)decimal.MaxValue);
        }
    }

    internal abstract class PrimitiveNumberPropertyEditor<T> : PropertyEditor
    {
        // Methods
        protected abstract EditorNumberInput CreateNumberInput(EditorLayoutControl layout, T value, bool isMixed);

        protected override void OnShow()
        {
            // Add layout
            EditorLayoutControl layout = RootControl.AddDirectionalLayout(EditorLayoutDirection.Horizontal);

            // Add property label
            EditorPropertyLabel propertyLabel = layout.AddPropertyLabel(Property);
            propertyLabel.Width = PropertyLabelWidth;

            // Get the current value
            T value;
            bool isMixed;
            Property.GetValue(out value, out isMixed);

            // Create number field
            EditorNumberInput numberInput = CreateNumberInput(layout, value, isMixed);

            // Check for read only
            numberInput.IsReadOnly = Property.IsReadOnly;
        }
    }
    #endregion
}
