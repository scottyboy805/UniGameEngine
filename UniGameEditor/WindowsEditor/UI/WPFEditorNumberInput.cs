using ModernWpf.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorNumberInput : EditorNumberInput
    {
        // Internal
        internal NumberBox numberBox = null;

        // Properties
        public override float Width
        {
            get => (float)numberBox.Width;
            set => numberBox.Width = value;
        }
        public override float Height
        {
            get => (int)numberBox.Height;
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

        // Constructor
        public WPFEditorNumberInput(Panel parent, double value, double min, double max)
        {
            numberBox = new NumberBox();
            numberBox.Value = value;
            numberBox.Minimum = min;
            numberBox.Maximum = max;

            numberBox.FontSize = DefaultFontSize;
            numberBox.MinHeight = DefaultLineHeight;

            parent.Children.Add(numberBox);
        }

        public WPFEditorNumberInput(ItemsControl parent, double value, double min, double max)
        {
            numberBox = new NumberBox();
            numberBox.Value = value;
            numberBox.Minimum = min;
            numberBox.Maximum = max;

            numberBox.FontSize = DefaultFontSize;
            numberBox.MinHeight = DefaultLineHeight;

            parent.Items.Add(numberBox);
        }
    }
}
