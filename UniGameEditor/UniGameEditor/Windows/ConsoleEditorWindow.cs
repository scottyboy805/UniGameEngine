using UniGameEditor.UI;

namespace UniGameEditor.Windows
{
    internal sealed class ConsoleEditorWindow : EditorWindow
    {
        // Constructor
        public ConsoleEditorWindow()
        {
            icon = EditorIcon.FindIcon("Console");
            title = "Console";
        }

        // Methods
        protected internal override void OnShow()
        {
            // Get icons
            EditorIcon infoIcon = EditorIcon.FindIcon("Info");
            EditorIcon warningIcon = EditorIcon.FindIcon("Warning");
            EditorIcon errorIcon = EditorIcon.FindIcon("Error");
            

            // Add toolbar
            EditorLayoutControl topBar = RootControl.AddDirectionalLayout(EditorLayoutDirection.Horizontal);

            // Add button
            EditorButton clearButton = topBar.AddButton();
            clearButton.Content.AddLabel("Clear");

            // Add dropdown
            EditorDropdown dropdown = topBar.AddDropdown();
            dropdown.Width = 150;
            dropdown.AddOption().Content.AddLabel("Hello");
            dropdown.AddOption().Content.AddLabel("World");

            // Add search
            topBar.AddLabel("Search:");
            topBar.AddInput("").Width = 250;

            // Add toggle
            EditorToggleButton messageButton = topBar.AddToggleButton();
            messageButton.Content.AddImage(infoIcon);
            messageButton.Content.AddLabel("Message");

            EditorToggleButton warningButton = topBar.AddToggleButton();
            warningButton.Content.AddImage(warningIcon);
            warningButton.Content.AddLabel("Error");

            EditorToggleButton errorButton = topBar.AddToggleButton();
            errorButton.Content.AddImage(errorIcon);
            errorButton.Content.AddLabel("Error");


            EditorCombinationDropdown drop = RootControl.AddCombinationDropdown();

            drop.AddOption().Content.AddLabel("Option 1");
            //drop.AddOption("Option2");
            //drop.AddOption("Option3");
        }
    }
}
