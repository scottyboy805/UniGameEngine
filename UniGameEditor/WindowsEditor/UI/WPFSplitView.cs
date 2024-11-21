using System.Windows;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFSplitView : EditorSplitViewLayoutControl
    {
        // Internal
        internal Grid grid = null;
        internal WPFDragDrop dragDrop = null;
        internal WPFEditorLayoutControl layoutA = null;
        internal WPFEditorLayoutControl layoutB = null;

        // Properties
        public override float Width
        {
            get => (float)grid.ActualWidth;
            set => grid.Width = value;
        }
        public override float Height
        {
            get => (float)grid.ActualHeight;
            set => grid.Height = value;
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
                grid.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        public override EditorLayoutControl LayoutA => layoutA;
        public override EditorLayoutControl LayoutB => layoutB;

        // Constructor
        internal WPFSplitView(Panel parent, Orientation orientation)
        {
            grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Check orientation
            if (orientation == Orientation.Horizontal)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Auto) });


                // Add layouts
                layoutA = new WPFEditorStackLayout(grid, Orientation.Vertical);
                layoutB = new WPFEditorStackLayout(grid, Orientation.Vertical);
                layoutA.Panel.MinWidth = 250;
                layoutA.Panel.HorizontalAlignment = HorizontalAlignment.Stretch;
                layoutB.Panel.MinWidth = 150;
                layoutB.Panel.HorizontalAlignment= HorizontalAlignment.Stretch;

                // Add splitter
                GridSplitter splitter = new GridSplitter();
                splitter.HorizontalAlignment = HorizontalAlignment.Stretch;
                grid.Children.Add(splitter);

                // Setup layout
                Grid.SetColumn(layoutA.Panel, 0);
                Grid.SetColumn(splitter, 1);
                Grid.SetColumn(layoutB.Panel, 2);
            }
            else
            {

            }

            // Update parent
            parent.Children.Add(grid);
        }
    }
}
