using System.Runtime.CompilerServices;
using UniGameEditor.UI;

[assembly: InternalsVisibleTo("WindowsEditor")]

namespace UniGameEditor.Windows
{
    public enum EditorWindowLocation
    {
        Center,
        Right,
        Left,
        Bottom,
    }

    public abstract class EditorWindow
    {
        // Events
        internal static event Action<EditorWindow, EditorWindowLocation> OnRequestOpenWindow;

        // Internal
        internal UniEditor editor = null;
        internal EditorLayoutControl rootControl = null;
        internal EditorIcon icon = null;
        internal string title = null;
        internal bool isOpen = false;

        // Private
        private float width = 0f;
        private float height = 0f;

        // Properties
        public UniEditor Editor
        {
            get { return editor; }
        }

        public EditorLayoutControl RootControl
        {
            get { return rootControl; }
        }

        public EditorIcon Icon
        {
            get { return icon; }
        }

        public string Title
        {
            get { return title; }
        }

        public bool IsOpen
        {
            get { return isOpen; }
        }

        public float Width
        {
            get { return width; }
        }

        public float Height
        {
            get { return height; }
        }

        // Constructor
        protected EditorWindow()
        {
            title = GetType().Name;
        }

        // Methods
        protected internal virtual void OnShow() { }

        protected internal virtual void OnHide() { }

        internal void Resize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        // Methods
        public static T OpenEditorWindow<T>(EditorWindowLocation location = EditorWindowLocation.Center) where T : EditorWindow, new()
        {
            // Create instance
            T window = new T();

            // Request show
            OnRequestOpenWindow(window, location);
            return window;
        }
    }
}
