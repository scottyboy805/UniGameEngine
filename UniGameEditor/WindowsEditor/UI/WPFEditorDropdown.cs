using ModernWpf.Controls;
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

            public override string Text
            {
                get => (string)item.Content;
                set => item.Content = value;
            }
            public override bool IsSelected
            {
                get => item.IsSelected;
                set => item.IsSelected = value;
            }

            // Constructor
            public DropdownEditorOption(string text)
            {
                item = new ComboBoxItem();
                item.Content = text;
            }
        }

        // Internal
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
            get => (float)combo.Width;
            set => combo.Width = value;
        }
        public override float Height
        {
            get => (int)combo.Height;
            set => combo.Height = value;
        }

        // Constructor
        public WPFEditorDropdown(Panel parent)
        {
            combo = new ComboBox();

            combo.FontSize = DefaultFontSize;
            combo.MinHeight = DefaultLineHeight;

            parent.Children.Add(combo);
        }

        public WPFEditorDropdown(ItemsControl parent)
        {
            combo = new ComboBox();

            combo.FontSize = DefaultFontSize;
            combo.MinHeight = DefaultLineHeight;

            parent.Items.Add(combo);
        }

        // Methods
        public override EditorOption AddOption(string text)
        {
            // Create option
            DropdownEditorOption option = new DropdownEditorOption(text);

            // Add to options
            if (options == null)
                options = new List<EditorOption>();

            options.Add(option);

            // Add to combo
            combo.Items.Add(option.item);

            // Update selection
            if(combo.SelectedIndex < 0)
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
