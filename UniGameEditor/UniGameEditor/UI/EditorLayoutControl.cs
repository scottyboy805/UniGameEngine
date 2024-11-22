
using Microsoft.Xna.Framework;

namespace UniGameEditor.UI
{
    public enum EditorLayoutDirection
    {
        Horizontal,
        Vertical,
    }

    public abstract class EditorSplitViewLayoutControl : EditorControl
    {
        // Properties
        public abstract EditorLayoutControl LayoutA { get; }
        public abstract EditorLayoutControl LayoutB { get; }
    }

    public abstract class EditorLayoutControl : EditorControl
    {
        // Methods
        public abstract EditorFoldout AddFoldoutLayout(bool isExpanded = false);
        public abstract EditorLayoutControl AddFlowLayout();
        public abstract EditorLayoutControl AddDirectionalLayout(EditorLayoutDirection direction);
        public abstract EditorSplitViewLayoutControl AddDirectionalSplitLayout(EditorLayoutDirection direction);
        public abstract EditorLayoutControl AddScrollLayout(bool horizontal = true, bool vertical = true);

        public abstract EditorControl AddSpacer(float width, float height);
        public abstract EditorLabel AddLabel(string text);
        public EditorPropertyLabel AddPropertyLabel(SerializedProperty property) => AddPropertyLabel(property, property.DisplayName);
        public abstract EditorPropertyLabel AddPropertyLabel(SerializedProperty property, string overrideText);
        public abstract EditorImage AddImage(EditorIcon icon);
        public abstract EditorInput AddInput(string text);
        public abstract EditorNumberInput AddNumberInput(double value, double min = double.MinValue, double max = double.MaxValue);
        public abstract EditorButton AddButton();
        public abstract EditorToggleButton AddToggleButton(bool on = false);
        public abstract EditorToggle AddToggle(bool on = false);
        public abstract EditorDropdown AddDropdown();
        public abstract EditorCombinationDropdown AddCombinationDropdown();
        public abstract EditorRenderView AddRenderView(Game hostGame);
        public abstract EditorTreeView AddTreeView();
        public abstract EditorTable AddTable();

        public abstract void Clear();
    }
}
