using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorTreeNode : EditorTreeNode
    {
        // Internal
        internal WPFEditorTreeView treeView = null;
        internal TreeViewItem treeItem = null;
        internal IconTextContent content = null;

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

            InitializeTreeItem(text);

        }        

        public WPFEditorTreeNode(WPFEditorTreeNode treeNode, string text)
        {
            this.treeView = treeNode.treeView;
            this.treeItems = treeNode.treeItem.Items;

            InitializeTreeItem(text);
        }

        // Methods
        private void InitializeTreeItem(string text)
        {
            this.treeItem = new TreeViewItem();

            // Create content
            this.content = new IconTextContent(treeItem, text);

            // Add listeners
            treeItem.Selected += (object sender, RoutedEventArgs e) => OnSelectedEvent();
            treeItem.Expanded += (object sender, RoutedEventArgs e) => OnExpandedEvent(treeItem.IsExpanded);
            treeItem.Collapsed += (object sender, RoutedEventArgs e) => OnExpandedEvent(treeItem.IsExpanded);
 
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
