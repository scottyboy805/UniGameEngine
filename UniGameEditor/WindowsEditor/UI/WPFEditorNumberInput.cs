using ModernWpf.Controls;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorNumberInput : EditorNumberInput
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal NumberBox numberBox = null;

        // Properties
        public override float Width
        {
            get => (float)numberBox.ActualWidth;
            set => numberBox.Width = value;
        }
        public override float Height
        {
            get => (float)numberBox.ActualHeight;
            set => numberBox.Height = value;
        }
        public override long ValueInteger
        {
            get => (long)numberBox.Value;
            set => numberBox.Value = value;
        }
        public override double Value
        {
            get => numberBox.Value;
            set => numberBox.Value = value;
        }
        public override double MinValue
        {
            get => numberBox.Minimum;
            set => numberBox.Minimum = value;
        }
        public override double MaxValue
        {
            get => numberBox.Maximum;
            set => numberBox.Maximum = value;
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
                numberBox.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorNumberInput(Panel parent, double value, double min, double max)
        {
            numberBox = new NumberBox();
            InitializeNumberInput(value, min, max);

            parent.Children.Add(numberBox);
        }

        public WPFEditorNumberInput(ItemsControl parent, double value, double min, double max)
        {
            numberBox = new NumberBox();
            InitializeNumberInput(value, min, max);

            parent.Items.Add(numberBox);
        }

        // Methods
        private void InitializeNumberInput(double value, double min, double max)
        {
            dragDrop = new WPFDragDrop(numberBox);

            numberBox.Value = value;
            numberBox.Minimum = min;
            numberBox.Maximum = max;
            numberBox.FontSize = DefaultFontSize;
            numberBox.Height = DefaultControlHeight;
        }
    }
}
