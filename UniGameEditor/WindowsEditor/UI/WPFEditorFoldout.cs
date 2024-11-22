using Microsoft.Xna.Framework;
using System.Windows;
using System.Windows.Controls;
using UniGameEditor;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorFoldout : EditorFoldout
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal Expander expander = null;
        internal WPFEditorLayoutControl layout = null;
        internal Panel content = null;

        // Properties
        public override float Width
        {
            get => (float)expander.ActualWidth;
            set => expander.Width = value;
        }
        public override float Height
        {
            get => (float)expander.ActualHeight;
            set => expander.Height = value;
        }
        public override bool IsExpanded
        {
            get => expander.IsExpanded;
            set => expander.IsExpanded = value;
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
                expander.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        public override EditorLayoutControl Header
        {
            get { return layout; }
        }

        public override string Tooltip
        {
            get => (string)expander.ToolTip;
            set => expander.ToolTip = value;
        }

        // Constructor
        public WPFEditorFoldout(Panel parent, bool isExpanded)
        {
            expander = new Expander();
            InitializeExpanded(isExpanded);

            parent.Children.Add(expander);
        }

        public WPFEditorFoldout(ItemsControl parent, bool isExpanded)
        {
            expander = new Expander();
            InitializeExpanded(isExpanded);

            parent.Items.Add(expander);
        }

        // Methods
        private void InitializeExpanded(bool isExpanded)
        {
            dragDrop = new WPFDragDrop(expander);
            layout = new WPFEditorLayoutControl((Panel)null, new StackPanel
            {
                Orientation = Orientation.Horizontal,
                MinHeight = DefaultLineHeight,
            });

            // Set content
            expander.Content = content = new StackPanel();
            expander.Header = layout.panel;
            expander.IsExpanded = isExpanded;

            // Add listener
            expander.Expanded += (object sender, RoutedEventArgs e) => OnExpandedEvent(true);
            expander.Collapsed += (object sender, RoutedEventArgs e) => OnExpandedEvent(false);
        }

        public override EditorLabel AddLabel(string text)
        {
            return new WPFEditorLabel(content, text);
        }

        public override EditorPropertyLabel AddPropertyLabel(SerializedProperty property, string overrideText)
        {
            return new WPFEditorPropertyLabel(content, property, overrideText);
        }

        public override EditorInput AddInput(string text)
        {
            return new WPFEditorInput(content, text);
        }

        public override EditorNumberInput AddNumberInput(double value, double min = double.MinValue, double max = double.MaxValue)
        {
            return new WPFEditorNumberInput(content, value, min, max);
        }

        public override EditorImage AddImage(EditorIcon icon)
        {
            return new WPFEditorImage(content, icon);
        }

        public override EditorButton AddButton()
        {
            return new WPFEditorButton(content);
        }

        public override EditorToggleButton AddToggleButton(bool on)
        {
            return new WPFEditorToggleButton(content, on);
        }

        public override EditorToggle AddToggle(bool on)
        {
            return new WPFEditorToggle(content, on);
        }

        public override EditorDropdown AddDropdown()
        {
            return new WPFEditorDropdown(content);
        }

        public override EditorCombinationDropdown AddCombinationDropdown()
        {
            return new WPFEditorCombinationDropdown(content);
        }

        public override EditorRenderView AddRenderView(Game gameHost)
        {
            return new WPFEditorRenderView(content, gameHost);
        }

        public override EditorTreeView AddTreeView()
        {
            return new WPFEditorTreeView(content);
        }

        public override EditorTable AddTable()
        {
            return new WPFEditorTable(content);
        }

        public override EditorFoldout AddFoldoutLayout(bool isExpanded = false)
        {
            return new WPFEditorFoldout(content, isExpanded);
        }

        public override EditorLayoutControl AddFlowLayout()
        {
            return new WPFEditorWrapPanelLayout(content);
        }

        public override EditorLayoutControl AddDirectionalLayout(EditorLayoutDirection direction)
        {
            return new WPFEditorStackLayout(content, (Orientation)direction);
        }

        public override EditorSplitViewLayoutControl AddDirectionalSplitLayout(EditorLayoutDirection direction)
        {
            return new WPFSplitView(content, (Orientation)direction);
        }

        public override EditorLayoutControl AddScrollLayout(bool horizontal = true, bool vertical = true)
        {
            return new WPFEditorScrollView(content, horizontal, vertical);
        }

        public override void Clear()
        {
            content.Children.Clear();
        }
    }
}
