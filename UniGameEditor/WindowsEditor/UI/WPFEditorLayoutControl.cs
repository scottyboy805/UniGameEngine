using Microsoft.Xna.Framework;
using System.Windows.Controls;
using UniGameEditor;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal abstract class WPFEditorLayoutControl : EditorLayoutControl
    {
        // Internal
        internal WPFDragDrop dragDrop = null;

        // Properties
        public abstract Panel Panel { get; }

        public override float Width
        {
            get => (float)Panel.ActualWidth;
            set => Panel.Width = value;
        }
        public override float Height
        {
            get => (float)Panel.ActualHeight;
            set => Panel.Height = value;
        }

        public override IDragHandler DragHandler
        {
            get => dragDrop.DragHandler;
            set => dragDrop.DragHandler = value;
        }

        public override IDropHandler DropHandler
        {
            get => dragDrop.DropHandler;
            set => dragDrop.DropHandler = value;
        }

        public override EditorMenu ContextMenu
        {
            get => contextMenu;
            set
            {
                contextMenu = value;
                Panel.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
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

        public override EditorCombinationDropdown AddCombinationDropdown()
        {
            return new WPFEditorCombinationDropdown(Panel);
        }

        public override EditorRenderView AddRenderView(Game gameHost)
        {
            return new WPFEditorRenderView(Panel, gameHost);
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

        public override EditorSplitViewLayoutControl AddHorizontalSplitLayout()
        {
            return new WPFSplitView(Panel, Orientation.Horizontal);
        }

        public override EditorSplitViewLayoutControl AddVerticalSplitLayout()
        {
            return new WPFSplitView(Panel, Orientation.Vertical);
        }

        public override EditorLayoutControl AddScrollLayout(bool horizontal = true, bool vertical = true)
        {
            return new WPFEditorScrollView(Panel, horizontal, vertical);
        }

        public override void Clear()
        {
            Panel.Children.Clear();
        }
    }
}
