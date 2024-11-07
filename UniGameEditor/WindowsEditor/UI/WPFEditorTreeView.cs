using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorTreeView : EditorTreeView
    {
        // Internal
        internal TreeView treeView = null;
        internal List<EditorTreeNode> nodes = null;

        // Properties
        public override float Width
        {
            get => (float)treeView.Width;
            set => treeView.Width = value;
        }
        public override float Height
        {
            get => (float)treeView.Height;
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

        // Constructor
        public WPFEditorTreeView(Panel parent)
        {
            treeView = new TreeView();
            parent.Children.Add(treeView);
        }

        public WPFEditorTreeView(ItemsControl parent)
        {
            treeView = new TreeView();
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
    }
}
