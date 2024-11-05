
namespace UniGameEditor.UI
{
    public abstract class EditorPropertyLabel : EditorLabel
    {
        // Private
        private SerializedProperty property = null;

        // Properties
        public SerializedProperty Property => property;

        // Constructor
        protected EditorPropertyLabel(SerializedProperty property)
        {
            this.property = property;
        }
    }
}
