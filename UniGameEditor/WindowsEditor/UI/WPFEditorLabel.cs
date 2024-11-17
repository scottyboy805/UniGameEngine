﻿using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal class WPFEditorLabel : EditorLabel
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal IconTextContent content = null;

        // Properties
        public override float Width
        {
            get => (float)content.Width;
            set => content.Width = value;
        }

        public override float Height
        {
            get => (float)content.Height;
            set => content.Height = value;
        }

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

        // Constructor
        public WPFEditorLabel(Panel parent, string text)
        {
            content = new IconTextContent(parent, new Label(), text);
            dragDrop = new WPFDragDrop(content.mainControl);
        }

        public WPFEditorLabel(ItemsControl parent, string text)
        {
            content = new IconTextContent(parent, new Label(), text);
            dragDrop = new WPFDragDrop(content.mainControl);
        }
    }
}
