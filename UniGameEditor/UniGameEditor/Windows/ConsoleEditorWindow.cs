using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class ConsoleEditorWindow : EditorWindow
    {
        // Constructor
        public ConsoleEditorWindow()
        {
            title = "Console";
        }

        // Methods
        protected internal override void OnShow()
        {
            // Add toolbar
            EditorLayoutControl topBar = RootControl.AddHorizontalLayout();

            // Add button
            EditorButton clearButton = topBar.AddButton("Clear");

            // Add dropdown
            EditorDropdown dropdown = topBar.AddDropdown();
            dropdown.Width = 150;
            dropdown.AddOption("Hello");
            dropdown.AddOption("World");

            // Add search
            topBar.AddLabel("Search:");
            topBar.AddInput("").Width = 250;

            // Add toggle
            topBar.AddToggleButton("Message");
            topBar.AddToggleButton("Warning");
            topBar.AddToggleButton("Error");
        }
    }
}
