
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

        // Methods
        protected void OnInitialized()
        {
            ContextMenu = CreatePropertyContextMenu();
        }

        private EditorMenu CreatePropertyContextMenu()
        {
            // Create the menu
            EditorMenu menu = EditorMenu.Create();

            // Reset option
            menu.AddItem("Reset").OnClicked += () =>
            {

            };
            // Copy option
            EditorMenuItem copyMenu = menu.AddItem("Copy");
            copyMenu.OnShown += () =>
            {
                // Only available if property is not mixed
                copyMenu.IsEnabled = property.IsMixed() == false;
            };
            copyMenu.OnClicked += () =>
            {

            };

            return menu;
        }
    }
}
