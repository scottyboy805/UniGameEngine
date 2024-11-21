﻿using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorInput : EditorInput
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal TextBox textBox = null;

        // Properties
        public override float Width
        {
            get => (float)textBox.ActualWidth;
            set => textBox.Width = value;
        }
        public override float Height
        {
            get => (float)textBox.ActualHeight;
            set => textBox.Height = value;
        }

        public override string Text
        {
            get => textBox.Text;
            set => textBox.Text = value;
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
                textBox.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorInput(Panel parent, string text)
        {
            textBox = new TextBox();
            dragDrop = new WPFDragDrop(textBox);
            textBox.Text = text;

            textBox.FontSize = DefaultFontSize;
            textBox.MinHeight = DefaultLineHeight;

            parent.Children.Add(textBox);
        }

        public WPFEditorInput(ItemsControl parent, string text)
        {
            textBox = new TextBox();
            dragDrop = new WPFDragDrop(textBox);
            textBox.Text = text;

            textBox.FontSize = DefaultFontSize;
            textBox.MinHeight = DefaultLineHeight;

            parent.Items.Add(textBox);
        }
    }
}
