﻿using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorToggle : EditorToggle
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal CheckBox checkBox = null;
        internal WPFEditorLayoutControl layout = null;

        // Properties
        public override float Width
        {
            get => (float)checkBox.ActualWidth;
            set => checkBox.Width = value;
        }
        public override float Height
        {
            get => (float)checkBox.ActualHeight;
            set => checkBox.Height = value;
        }

        public override bool IsChecked
        {
            get => (bool)checkBox.IsChecked;
            set => checkBox.IsChecked = value;
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
                checkBox.ContextMenu = value != null
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
            get => (string)checkBox.ToolTip;
            set => checkBox.ToolTip = value;
        }

        // Constructor
        public WPFEditorToggle(Panel parent, bool on)
        {
            checkBox = new CheckBox();
            InitializeToggle(on);

            parent.Children.Add(checkBox);
        }

        public WPFEditorToggle(ItemsControl parent, bool on)
        {
            checkBox = new CheckBox();
            InitializeToggle(on);

            parent.Items.Add(checkBox);
        }

        // Methods
        private void InitializeToggle(bool on)
        {
            dragDrop = new WPFDragDrop(checkBox);
            layout = new WPFEditorLayoutControl((Panel)null, new StackPanel
            {
                Orientation = Orientation.Horizontal,
                MinHeight = DefaultLineHeight,
            });

            // Set content
            checkBox.Content = layout.panel;
            checkBox.IsChecked = on;
            checkBox.FontSize = DefaultFontSize;
            checkBox.MinHeight = DefaultLineHeight;
        }
    }
}
