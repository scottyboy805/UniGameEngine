using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorTreeNode : EditorTreeNode
    {
        // Internal
        internal WPFEditorTreeView treeView = null;
        internal TreeViewItem treeItem = null;
        internal ItemCollection treeItems = null;
        internal List<EditorTreeNode> nodes = null;

        // Properties
        public override float Width
        {
            get => (float)treeItem.Width;
            set => treeItem.Width = value;
        }

        public override float Height
        {
            get => (int)treeItem.Height;
            set => treeItem.Height = value;
        }

        public override string Text
        {
            get => (string)treeItem.Header;
            set => treeItem.Header = value;
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

        // Constructor
        public WPFEditorTreeNode(WPFEditorTreeView treeView, string text)
        {
            this.treeView = treeView;
            this.treeItems = treeView.treeView.Items;
            this.treeItem = new TreeViewItem();
            this.treeItem.Header = text;

            treeItem.FontSize = DefaultFontSize;
            treeItem.MinHeight = DefaultLineHeight;
            
            treeItems.Add(treeItem);
        }        

        public WPFEditorTreeNode(WPFEditorTreeNode treeNode, string text)
        {
            this.treeView = treeNode.treeView;
            this.treeItems = treeNode.treeItem.Items;
            this.treeItem = new TreeViewItem();
            this.treeItem.Header = text;

            treeItem.FontSize = DefaultFontSize;
            treeItem.MinHeight = DefaultLineHeight;

            treeItems.Add(treeItem);
        }

        public override EditorTreeNode AddNode(string text)
        {
            // Create node
            WPFEditorTreeNode node = new WPFEditorTreeNode(this, text);

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
    }
}
