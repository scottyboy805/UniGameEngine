using ModernWpf.Controls;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorTreeView : EditorTreeView
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal TreeView treeView = null;
        internal List<EditorTreeNode> nodes = null;

        // Properties
        public override float Width
        {
            get => (float)treeView.ActualWidth;
            set => treeView.Width = value;
        }
        public override float Height
        {
            get => (float)treeView.ActualHeight;
            set => treeView.Height = value;
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
                treeView.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorTreeView(Panel parent)
        {
            treeView = new TreeView();
            dragDrop = new WPFDragDrop(treeView);
            parent.Children.Add(treeView);
        }

        public WPFEditorTreeView(ItemsControl parent)
        {
            treeView = new TreeView();
            dragDrop = new WPFDragDrop(treeView);
            parent.Items.Add(treeView);
        }

        // Methods
        public override EditorTreeNode AddNode(string text)
        {
            // Create node
            WPFEditorTreeNode node = new WPFEditorTreeNode(this, text);

            // Create nodes
            if (nodes == null)
                nodes = new List<EditorTreeNode>();

            // Add to nodes
            nodes.Add(node);
            
            return node;
        }

        public override void RemoveNode(EditorTreeNode node)
        {
            // Check fro found
            if (nodes.Contains(node) == true)
            {
                // Remove from nodes
                nodes.Remove(node);

                // Remove from tree
                treeView.Items.Remove(((WPFEditorTreeNode)node).treeItem);
            }
        }

        public override void ClearNodes()
        {
            treeView.Items.Clear();

            if(nodes != null)
                nodes.Clear();
        }
    }
}
