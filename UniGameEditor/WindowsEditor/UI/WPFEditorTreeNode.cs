using System.Windows;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorTreeNode : EditorTreeNode
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal WPFEditorTreeView treeView = null;
        internal TreeViewItem treeItem = null;
        internal WPFEditorLayoutControl layout = null;

        internal ItemCollection treeItems = null;
        internal List<EditorTreeNode> nodes = null;

        // Properties
        public override float Width
        {
            get => (float)treeItem.ActualWidth;
            set => treeItem.Width = value;
        }

        public override float Height
        {
            get => (float)treeItem.ActualHeight;
            set => treeItem.Height = value;
        }

        public override bool IsExpanded
        {
            get => treeItem.IsExpanded;
            set => treeItem.IsExpanded = value;
        }

        public override IEnumerable<EditorTreeNode> Nodes
        {
            get
            {
                if (nodes != null)
                    return nodes;

                return Enumerable.Empty<EditorTreeNode>();
            }
        }

        public override int NodeCount
        {
            get => nodes != null ? nodes.Count : 0;
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
                treeItem.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        public override EditorLayoutControl Header
        {
            get { return layout; }
        }

        public override string Tooltip
        {
            get => (string)treeItem.ToolTip;
            set => treeItem.ToolTip = value;
        }

        // Constructor
        public WPFEditorTreeNode(WPFEditorTreeView treeView)
        {
            this.treeView = treeView;
            this.treeItems = treeView.treeView.Items;

            InitializeTreeItem();
        }        

        public WPFEditorTreeNode(WPFEditorTreeNode treeNode)
        {
            this.treeView = treeNode.treeView;
            this.treeItems = treeNode.treeItem.Items;

            InitializeTreeItem();
        }

        // Methods
        private void InitializeTreeItem()
        {
            this.treeItem = new TreeViewItem();
            this.dragDrop = new WPFDragDrop(treeItem);
            layout = new WPFEditorLayoutControl((Panel)null, new StackPanel
            {
                Orientation = Orientation.Horizontal,
                MinHeight = DefaultLineHeight,
            });

            // Set content
            treeItem.Header = layout.panel;

            // Add listeners
            treeItem.Selected += (object sender, RoutedEventArgs e) => { if (treeItem.IsSelected) OnSelectedEvent(); };
            treeItem.Expanded += (object sender, RoutedEventArgs e) => OnExpandedEvent(treeItem.IsExpanded);
            treeItem.Collapsed += (object sender, RoutedEventArgs e) => OnExpandedEvent(treeItem.IsExpanded);
 
            treeItems.Add(treeItem);
        }

        public override EditorTreeNode AddNode()
        {
            // Create node
            WPFEditorTreeNode node = new WPFEditorTreeNode(this);

            // Create nodes
            if(nodes == null)
                nodes = new List<EditorTreeNode>();

            // Add to nodes
            nodes.Add(node);

            return node;
        }

        public override void RemoveNode(EditorTreeNode node)
        {
            // Check fro found
            if(nodes.Contains(node) == true)
            {
                // Remove from nodes
                nodes.Remove(node);

                // Remove from tree
                treeItems.Remove(((WPFEditorTreeNode)node).treeItem);
            }
        }

        public override void ClearNodes()
        {
            treeItems.Clear();

            if(nodes != null)
                nodes.Clear();
        }
    }
}
