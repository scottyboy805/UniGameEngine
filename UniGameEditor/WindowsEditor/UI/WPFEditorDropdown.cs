using System.Windows;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorDropdown : EditorDropdown
    {
        // Type
        internal sealed class DropdownEditorOption : EditorOption
        {
            // Internal            
            internal ComboBoxItem item = null;
            internal WPFEditorLayoutControl layout = null;

            // Properties
            public override EditorLayoutControl Content
            {
                get { return layout; }
            }

            public override string Tooltip
            {
                get => (string)item.ToolTip;
                set => item.ToolTip = value;
            }

            public override bool IsSelected
            {
                get => item.IsSelected;
            }

            // Constructor
            public DropdownEditorOption(ItemsControl parent)
            {
                item = new ComboBoxItem();
                layout = new WPFEditorLayoutControl(parent, new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Height = DefaultLineHeight,
                });
            }
        }

        // Internal
        internal WPFDragDrop dragDrop = null;
        internal ComboBox combo = null;
        internal List<EditorOption> options = null;

        // Properties
        public override int SelectedIndex
        {
            get => combo.SelectedIndex;
            set => combo.SelectedIndex = value;
        }

        public override EditorOption SelectedOption
        {
            get => options[combo.SelectedIndex];
            set => combo.SelectedIndex = options.IndexOf(value);
        }

        public override float Width
        {
            get => (float)combo.ActualWidth;
            set => combo.Width = value;
        }

        public override float Height
        {
            get => (float)combo.ActualHeight;
            set => combo.Height = value;
        }

        public override string Tooltip
        {
            get => (string)combo.ToolTip;
            set => combo.ToolTip = value;
        }

        public override bool IsReadOnly
        {
            get => combo.IsReadOnly;
            set => combo.IsReadOnly = value;
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
                combo.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorDropdown(Panel parent)
        {
            combo = new ComboBox();
            InitializeDropdown();
            
            parent.Children.Add(combo);
        }

        public WPFEditorDropdown(ItemsControl parent)
        {
            combo = new ComboBox();
            InitializeDropdown();

            parent.Items.Add(combo);
        }

        // Methods
        private void InitializeDropdown()
        {
            dragDrop = new WPFDragDrop(combo);

            combo.Width = 150;  // Hack - otherwise the combo keeps expanding until off screen
            combo.HorizontalContentAlignment = HorizontalAlignment.Left;
            combo.FontSize = DefaultFontSize;
            combo.Height = DefaultControlHeight;

            // Add listeners
            combo.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
                // Trigger events
                OnSelectedIndexChangedEvent(SelectedIndex);
                OnSelectedOptionChangedEvent(SelectedOption);
            };
        }

        public override EditorOption AddOption()
        {
            // Add to options
            if (options == null)
                options = new List<EditorOption>();

            // Create option
            DropdownEditorOption option = new DropdownEditorOption(combo);

            // Add to options
            options.Add(option);

            // Update selection
            if (combo.SelectedIndex < 0)
                combo.SelectedIndex = 0;

            return option;
        }

        public override void RemoveOption(EditorOption option)
        {
            // Check for added
            if(options.Contains(option) == true)
            {
                // Remove option
                options.Remove(option);

                // Remove from combo
                combo.Items.Remove(((DropdownEditorOption)option).item);
            }
        }
    }
}
