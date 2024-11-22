using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorToggleButton : EditorToggleButton
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal ToggleButton toggleButton = null;
        internal WPFEditorLayoutControl layout = null;

        // Properties
        public override float Width
        {
            get => (float)toggleButton.ActualWidth;
            set => toggleButton.Width = value;
        }
        public override float Height
        {
            get => (float)toggleButton.ActualHeight;
            set => toggleButton.Height = value;
        }

        public override bool IsChecked
        {
            get => (bool)toggleButton.IsChecked;
            set => toggleButton.IsChecked = value;
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
                toggleButton.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        public override EditorLayoutControl Content
        {
            get { return layout; }
        }

        public override string Tooltip
        {
            get => (string)toggleButton.ToolTip;
            set => toggleButton.ToolTip = value;
        }

        // Constructor
        public WPFEditorToggleButton(Panel parent, bool on)
        {
            toggleButton = new ToggleButton();
            InitializeToggleButton(on);

            parent.Children.Add(toggleButton);
        }

        public WPFEditorToggleButton(ItemsControl parent, bool on)
        {
            toggleButton = new ToggleButton();
            InitializeToggleButton(on);

            parent.Items.Add(toggleButton);
        }

        // Methods
        private void InitializeToggleButton(bool on)
        {
            dragDrop = new WPFDragDrop(toggleButton);
            layout = new WPFEditorLayoutControl((Panel)null, new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Height = DefaultLineHeight,
            });

            // Set content
            toggleButton.Content = layout.panel;
            toggleButton.IsChecked = on;
            toggleButton.FontSize = DefaultFontSize;
            toggleButton.Height = DefaultControlHeight;
        }
    }
}
