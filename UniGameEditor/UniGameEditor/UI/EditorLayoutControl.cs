
namespace UniGameEditor.UI
{
    public abstract class EditorLayoutControl : EditorControl
    {
        // Methods
        public abstract EditorFoldout AddFoldoutLayout(string text, bool isExpanded = false);
        public abstract EditorLayoutControl AddFlowLayout();
        public abstract EditorLayoutControl AddHorizontalLayout();
        public abstract EditorLayoutControl AddVerticalLayout();
        public abstract EditorLayoutControl AddScrollLayout(bool horizontal = true, bool vertical = true);

        public abstract EditorLabel AddLabel(string text);
        public EditorPropertyLabel AddPropertyLabel(SerializedProperty property) => AddPropertyLabel(property, property.DisplayName);
        public abstract EditorPropertyLabel AddPropertyLabel(SerializedProperty property, string overrideText);
        public abstract EditorInput AddInput(string text);
        public abstract EditorNumberInput AddNumberInput(double value, double min = double.MinValue, double max = double.MaxValue);
        public abstract EditorButton AddButton(string text);
        public abstract EditorToggleButton AddToggleButton(string text, bool on = false);
        public abstract EditorToggle AddToggle(string text, bool on = false);
        public abstract EditorDropdown AddDropdown();        
        public abstract EditorRenderView AddRenderView(Action OnRender);
        public abstract EditorTreeView AddTreeView();
    }
}
