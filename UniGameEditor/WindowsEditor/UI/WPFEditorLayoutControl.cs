using System.Windows.Controls;
using UniGameEditor;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal abstract class WPFEditorLayoutControl : EditorLayoutControl
    {
        // Properties
        public abstract Panel Panel { get; }

        public override float Width
        {
            get => (float)Panel.Width;
            set => Panel.Width = value;
        }
        public override float Height
        {
            get => (int)Panel.Height;
            set => Panel.Height = value;
        }

        // Methods
        public override EditorLabel AddLabel(string text)
        {
            return new WPFEditorLabel(Panel, text);
        }

        public override EditorPropertyLabel AddPropertyLabel(SerializedProperty property, string overrideText)
        {
            return new WPFEditorPropertyLabel(Panel, property, overrideText);
        }

        public override EditorInput AddInput(string text)
        {
            return new WPFEditorInput(Panel, text);
        }

        public override EditorNumberInput AddNumberInput(double value, double min = double.MinValue, double max = double.MaxValue)
        {
            return new WPFEditorNumberInput(Panel, value, min, max);
        }

        public override EditorButton AddButton(string text)
        {
            return new WPFEditorButton(Panel, text);
        }

        public override EditorToggleButton AddToggleButton(string text, bool on)
        {
            return new WPFEditorToggleButton(Panel, text, on);
        }

        public override EditorToggle AddToggle(string text, bool on)
        {
            return new WPFEditorToggle(Panel, text, on);
        }

        public override EditorDropdown AddDropdown()
        {
            return new WPFEditorDropdown(Panel);
        }

        public override EditorRenderView AddRenderView(Action OnRender)
        {
            return new WPFEditorRenderView(Panel, OnRender);
        }

        public override EditorTreeView AddTreeView()
        {
            return new WPFEditorTreeView(Panel);
        }

        public override EditorFoldout AddFoldoutLayout(string text, bool isExpanded = false)
        {
            return new WPFEditorFoldout(Panel, text, isExpanded);
        }

        public override EditorLayoutControl AddFlowLayout()
        {
            return new WPFEditorWrapPanelLayout(Panel);
        }

        public override EditorLayoutControl AddHorizontalLayout()
        {
            return new WPFEditorStackLayout(Panel, Orientation.Horizontal);
        }

        public override EditorLayoutControl AddVerticalLayout()
        {
            return new WPFEditorStackLayout(Panel, Orientation.Vertical);
        }
    }
}
