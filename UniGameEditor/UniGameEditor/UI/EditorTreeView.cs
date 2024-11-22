
namespace UniGameEditor.UI
{
    public abstract class EditorTreeView : EditorControl
    {
        // Properties
        public abstract IEnumerable<EditorTreeNode> Nodes { get; }
        public abstract int NodeCount { get; }

        // Methods
        public abstract EditorTreeNode AddNode();
        public abstract void RemoveNode(EditorTreeNode node);
        public abstract void ClearNodes();
    }
}
