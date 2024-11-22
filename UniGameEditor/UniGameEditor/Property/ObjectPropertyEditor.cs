namespace UniGameEditor.Property
{
    [PropertyEditorFor(typeof(object), true)]
    internal sealed class ObjectPropertyEditor : PropertyEditor
    {
        // Methods
        protected override void OnShow()
        {
            // Get all properties
            foreach(SerializedProperty childProperty in Property.VisibleChildren)
            {
                // Get type
                Type propertyType = childProperty.Property.PropertyType;

                // Get drawer
                PropertyEditor editor = PropertyEditor.ForType(propertyType);

                // Create drawer
                editor.CreateProperty(RootControl, childProperty);
            }
        }
    }
}
