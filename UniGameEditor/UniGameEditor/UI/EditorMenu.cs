
namespace UniGameEditor.UI
{
    public abstract class EditorMenuItem
    {
        // Events
        public event Action OnClicked;
        public event Action OnShown;

        // Properties
        public abstract string Text { get; set; }
        public abstract bool IsEnabled { get; set; }
        public abstract bool IsChecked { get; set; }

        // Methods
        public abstract EditorMenuItem AddItem(string text);
        public abstract void RemoveItem(EditorMenuItem item);
        public abstract void AddSeparator();

        protected void InvokeOnClicked()
        {
            if (OnClicked != null)
                OnClicked();
        }

        protected void InvokeOnShown()
        {
            if(OnShown != null)
                OnShown();
        }
    }

    public abstract class EditorMenu
    {
        // Protected
        protected static Func<EditorMenu> MenuProvider = null;

        // Constructor
        protected EditorMenu() { }

        // Methods
        public abstract EditorMenuItem AddItem(string text);
        public abstract void RemoveItem(EditorMenuItem item);
        public abstract void AddSeparator();

        public static EditorMenu Create()
        {
            // Check for provider
            if (MenuProvider == null)
                throw new InvalidOperationException("Menu provider has not been setup for the host application");

            // Create new menu
            return MenuProvider();
        }
    }
}
