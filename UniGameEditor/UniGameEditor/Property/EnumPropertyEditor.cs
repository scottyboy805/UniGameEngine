using UniGameEditor.UI;

namespace UniGameEditor.Property
{
    [PropertyEditorFor(typeof(Enum), true)]
    internal sealed class EnumPropertyEditor : PropertyEditor
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
            Enum value;
            bool isMixed;
            Property.GetValue(out value, out isMixed);


            // Get property type
            Type enumType = Property.Property.PropertyType;

            // Check for flags attribute
            if (enumType.IsDefined(typeof(FlagsAttribute), false) == false)
            {
                // Show option selection
                OnShowEnumDropdown(layout, value, isMixed);
            }
            else
            {
                // Show combination options
                OnShowEnumCombinationDropdown();
            }
        }

        private void OnShowEnumDropdown(EditorLayoutControl layout, Enum value, bool isMixed)
        {
            // Get the enum type and options
            Type enumType = value.GetType();
            Array enumValues = Enum.GetValues(enumType);


            // Create dropdown
            EditorDropdown dropdown = layout.AddDropdown();

            // Add all options
            foreach(Enum enumOption in enumValues)
            {
                // Add the option
                EditorOption option = dropdown.AddOption();

                // Add option
                option.Content.AddLabel(enumOption.ToString());

                // Update selected
                if(enumOption.Equals(value) == true)
                    dropdown.SelectedOption = option;
            }

            // Check for readonly
            dropdown.IsReadOnly = Property.IsReadOnly;

            // Listen for changed
            dropdown.OnSelectedIndexChanged += (EditorDropdown dropdown, int index) =>
            {
                // Get new enum value
                Enum newValue = (Enum)enumValues.GetValue(index);

                // Update property with undo
                Editor.Undo.RecordPropertyModified(Property, newValue);
            };
        }

        private void OnShowEnumCombinationDropdown()
        {

        }
    }
}
