using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorSpacer : EditorControl
    {
        // Internal
        internal Separator spacer = null;

        // Properties
        public override float Width
        {
            get => (float)spacer.ActualWidth;
            set => spacer.Width = value;
        }

        public override float Height
        {
            get => (float)spacer.ActualHeight;
            set => spacer.Height = value;
        }

        public override IDragHandler DragHandler
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override IDropHandler DropHandler
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override EditorMenu ContextMenu
        {
            get => contextMenu;
            set
            {
                contextMenu = value;
                spacer.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorSpacer(Panel parent, float width, float height)
        {
            spacer = new Separator();
            InitializeSpacer(width, height);

            parent.Children.Add(spacer);
        }

        public WPFEditorSpacer(ItemsControl parent, float width, float height)
        {
            spacer = new Separator();
            InitializeSpacer(width, height);

            parent.Items.Add(spacer);
        }

        // Methods
        private void InitializeSpacer(float width, float height)
        {
            spacer.Visibility = System.Windows.Visibility.Hidden;
            spacer.Width = width;
            spacer.Height = height;
        }
    }
}
