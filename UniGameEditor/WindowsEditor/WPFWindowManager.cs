using UniGameEditor;
using UniGameEditor.Windows;

namespace WindowsEditor
{
    internal sealed class WPFWindowManager
    {
        // Private
        private UniEditor editor = null;
        private List<WPFWindowControl> windowDocks = new List<WPFWindowControl>();

        // Constructor
        public WPFWindowManager(UniEditor editor)
        {
            this.editor = editor;

            // Add listener
            EditorWindow.OnRequestOpenWindow += OnRequestOpenWindow;
            EditorWindow.OnRequestCloseWindow += OnRequestCloseWindow;
            EditorWindow.OnRequestFocusWindow += OnRequestFocusWindow;
        }

        // Methods
        public void AddWindowDock(WPFWindowControl windowDock)
        {
            windowDock.windowManager = this;
            windowDocks.Add(windowDock);
        }

        public void ResetLayout()
        {
            // Reset all views
            foreach(WPFWindowControl windowDock in windowDocks)
                windowDock.ResetLayout();

            // Close open windows and reset to defaults
            CloseAllWindows();
            ShowDefaultWindows();
        }

        public void ShowDefaultWindows()
        {
            // Add hierarchy window
            EditorWindow.OpenEditorWindow<HierarchyEditorWindow>(EditorWindowLocation.Left);

            // Add scene and game window
            EditorWindow sceneWindow = EditorWindow.OpenEditorWindow<SceneEditorWindow>(EditorWindowLocation.Center);
            EditorWindow.OpenEditorWindow<GameEditorWindow>(EditorWindowLocation.Center);

            // Add content and console window
            EditorWindow contentWindow = EditorWindow.OpenEditorWindow<ContentEditorWindow>(EditorWindowLocation.Bottom);
            EditorWindow.OpenEditorWindow<ConsoleEditorWindow>(EditorWindowLocation.Bottom);

            // Add properties window
            EditorWindow.OpenEditorWindow<PropertiesEditorWindow>(EditorWindowLocation.Right);


            // Show scene and content window
            sceneWindow.Focus();
            contentWindow.Focus();
        }

        public void ChangeLocation(EditorWindow window, EditorWindowLocation location)
        {
            // Get location
            EditorWindowLocation currentLocation;
            if(GetWindowLocation(window, out currentLocation) == true)
            {
                if(location != currentLocation)
                {
                    CloseWindow(window);
                    OpenWindow(window, location);
                }
            }
        }

        public bool IsWindowOpen<T>() where T : EditorWindow
        {
            // Check for open
            foreach (WPFWindowControl windowDock in windowDocks)
            {
                if(windowDock.IsWindowOpen<T>() == true)
                    return true;
            }
            return false;
        }

        public bool GetWindowLocation(EditorWindow window, out EditorWindowLocation location)
        {
            foreach (WPFWindowControl windowDock in windowDocks)
            {
                if (windowDock.IsWindowOpen(window) == true)
                {
                    location = windowDock.Location;
                    return true;
                }
            }
            location = 0;
            return false;
        }

        public void SetWindowOpen<T>(bool open, EditorWindowLocation location) where T : EditorWindow, new()
        {
            if(open == true)
            {
                // Open the window
                if (IsWindowOpen<T>() == false)
                    EditorWindow.OpenEditorWindow<T>(location);
            }
            else
            {
                // Close the window
                CloseWindow<T>();
            }
        }

        public void OpenWindow(EditorWindow window, EditorWindowLocation location)
        {
            foreach (WPFWindowControl windowDock in windowDocks)
            {
                if(windowDock.Location == location)
                {
                    window.editor = editor;
                    windowDock.OpenWindow(window);
                    break;
                }
            }
        }

        public void CloseAllWindows()
        {
            foreach (WPFWindowControl windowDock in windowDocks)
                windowDock.CloseAllWindows();
        }

        public void CloseWindow(EditorWindow window)
        {
            foreach (WPFWindowControl windowDock in windowDocks)
                windowDock.CloseWindow(window);
        }

        public void CloseWindow<T>() where T : EditorWindow
        {
            foreach (WPFWindowControl windowDock in windowDocks)
                windowDock.CloseWindow<T>();
        }

        private WPFWindowControl GetWindowDock(EditorWindow window)
        {
            foreach (WPFWindowControl windowDock in windowDocks)
            {
                if (windowDock.IsWindowOpen(window) == true)
                    return windowDock;
            }
            return null;
        }

        private void OnRequestOpenWindow(EditorWindow window, EditorWindowLocation location)
        {
            // Check for open
            if (window.isOpen == true)
                CloseWindow(window);

            // Open the window
            OpenWindow(window, location);
        }

        private void OnRequestCloseWindow(EditorWindow window)
        {
            if(window.isOpen == true)
                CloseWindow(window);
        }

        private void OnRequestFocusWindow(EditorWindow window)
        {
            // Get the window dock
            WPFWindowControl windowDock = GetWindowDock(window);

            // Check for found
            if(windowDock != null)
                windowDock.FocusWindow(window);
        }
    }
}
