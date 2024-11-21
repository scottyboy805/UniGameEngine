using Microsoft.Xna.Framework;
using System.Windows.Controls;
using UniGameEditor;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal class WPFEditorLayoutControl : EditorLayoutControl
    {
        // Internal
        internal Panel panel = null;
        internal WPFDragDrop dragDrop = null;

        public override float Width
        {
            get => (float)panel.ActualWidth;
            set => panel.Width = value;
        }
        public override float Height
        {
            get => (float)panel.ActualHeight;
            set => panel.Height = value;
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
                panel.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorLayoutControl(Panel parent, Panel panel)
        {
            this.panel = panel;
            this.dragDrop = new WPFDragDrop(panel);

            // Add to parent
            if(parent != null)
                parent.Children.Add(panel);
        }

        public WPFEditorLayoutControl(ItemsControl parent, Panel panel)
        {
            this.panel = panel;
            this.dragDrop = new WPFDragDrop(panel);

            // Add to parent
            if(parent != null)
                parent.Items.Add(panel);
        }

        // Methods
        public override EditorLabel AddLabel(string text)
        {
            return new WPFEditorLabel(panel, text);
        }

        public override EditorPropertyLabel AddPropertyLabel(SerializedProperty property, string overrideText)
        {
            return new WPFEditorPropertyLabel(panel, property, overrideText);
        }

        public override EditorInput AddInput(string text)
        {
            return new WPFEditorInput(panel, text);
        }

        public override EditorNumberInput AddNumberInput(double value, double min = double.MinValue, double max = double.MaxValue)
        {
            return new WPFEditorNumberInput(panel, value, min, max);
        }

        public override EditorButton AddButton(string text)
        {
            return new WPFEditorButton(panel, text);
        }

        public override EditorToggleButton AddToggleButton(string text, bool on)
        {
            return new WPFEditorToggleButton(panel, text, on);
        }

        public override EditorToggle AddToggle(string text, bool on)
        {
            return new WPFEditorToggle(panel, text, on);
        }

        public override EditorDropdown AddDropdown()
        {
            return new WPFEditorDropdown(panel);
        }

        public override EditorCombinationDropdown AddCombinationDropdown()
        {
            return new WPFEditorCombinationDropdown(panel);
        }

        public override EditorRenderView AddRenderView(Game gameHost)
        {
            return new WPFEditorRenderView(panel, gameHost);
        }

        public override EditorTreeView AddTreeView()
        {
            return new WPFEditorTreeView(panel);
        }

        public override EditorTable AddTable()
        {
            return new WPFEditorTable(panel);
        }

        public override EditorFoldout AddFoldoutLayout(string text, bool isExpanded = false)
        {
            return new WPFEditorFoldout(panel, text, isExpanded);
        }

        public override EditorLayoutControl AddFlowLayout()
        {
            return new WPFEditorWrapPanelLayout(panel);
        }

        public override EditorLayoutControl AddHorizontalLayout()
        {
            return new WPFEditorStackLayout(panel, Orientation.Horizontal);
        }

        public override EditorLayoutControl AddVerticalLayout()
        {
            return new WPFEditorStackLayout(panel, Orientation.Vertical);
        }        

        public override EditorSplitViewLayoutControl AddHorizontalSplitLayout()
        {
            return new WPFSplitView(panel, Orientation.Horizontal);
        }

        public override EditorSplitViewLayoutControl AddVerticalSplitLayout()
        {
            return new WPFSplitView(panel, Orientation.Vertical);
        }

        public override EditorLayoutControl AddScrollLayout(bool horizontal = true, bool vertical = true)
        {
            return new WPFEditorScrollView(panel, horizontal, vertical);
        }

        public override void Clear()
        {
            panel.Children.Clear();
        }
    }
}
