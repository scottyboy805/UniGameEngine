﻿using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorCombinationDropdown : EditorCombinationDropdown
    {
        // Type
        internal sealed class DropdownCombinationEditorOption : EditorOption
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
            public DropdownCombinationEditorOption(ItemCollection parent, string text)
            {
                item = new ComboBoxItem();
                content = new IconTextContent(parent, item, text);
            }
        }

        // Internal
        internal WPFDragDrop dragDrop = null;
        internal ComboBox combo = null;
        internal List<EditorOption> options = null;

        // Properties
        public override int SelectedCount
        {
            get => options.Count(o => o.IsSelected == true);
        }

        public override int[] SelectedIndexes
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override EditorOption[] SelectedOptions
        {
            get => options.Where(o => o.IsSelected == true).ToArray();
            set
            {
                foreach(EditorOption option in options)
                {
                    ((DropdownCombinationEditorOption)option).item.IsSelected = value.Contains(option) == true
                        ? true
                        : false;
                }
            }
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
        public WPFEditorCombinationDropdown(Panel parent)
        {
            combo = new ComboBox();
            dragDrop = new WPFDragDrop(combo);
            combo.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;

            combo.FontSize = DefaultFontSize;
            combo.MinHeight = DefaultLineHeight;

            parent.Children.Add(combo);
        }

        public WPFEditorCombinationDropdown(ItemsControl parent)
        {
            combo = new ComboBox();
            dragDrop = new WPFDragDrop(combo);
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
            DropdownCombinationEditorOption option = new DropdownCombinationEditorOption(combo.Items, text);

            // Add to options
            options.Add(option);

            return option;
        }

        public override void RemoveOption(EditorOption option)
        {
            // Check for added
            if (options.Contains(option) == true)
            {
                // Remove option
                options.Remove(option);

                // Remove from combo
                combo.Items.Remove(((DropdownCombinationEditorOption)option).item);
            }
        }
    }
}
