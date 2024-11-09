﻿using ModernWpf.Controls;
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
            internal IconTextContent content = null;

            // Properties
            public override string Text
            {
                get => content.Text;
                set => content.Text = value;
            }

            public override string Tooltip
            {
                get => content.Tooltip;
                set => content.Tooltip = value;
            }

            public override EditorIcon Icon
            {
                get => content.Icon;
                set => content.Icon = value;
            }

            public override bool IsSelected
            {
                get => item.IsSelected;
            }

            // Constructor
            public DropdownEditorOption(ItemCollection parent, string text)
            {
                item = new ComboBoxItem();
                content = new IconTextContent(parent, item, text);
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
            get => (float)combo.ActualWidth;
            set => combo.Width = value;
        }

        public override float Height
        {
            get => (float)combo.ActualHeight;
            set => combo.Height = value;
        }

        // Constructor
        public WPFEditorDropdown(Panel parent)
        {
            combo = new ComboBox();
            combo.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;

            combo.FontSize = DefaultFontSize;
            combo.MinHeight = DefaultLineHeight;

            parent.Children.Add(combo);
        }

        public WPFEditorDropdown(ItemsControl parent)
        {
            combo = new ComboBox();
            combo.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;

            combo.FontSize = DefaultFontSize;
            combo.MinHeight = DefaultLineHeight;

            parent.Items.Add(combo);
        }

        // Methods
        public override EditorOption AddOption(string text)
        {
            // Add to options
            if (options == null)
                options = new List<EditorOption>();

            // Create option
            DropdownEditorOption option = new DropdownEditorOption(combo.Items, text);

            // Add to options
            options.Add(option);

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