
namespace UniGameEditor.UI
{
    public abstract class EditorTreeNode : EditorControl
    {
        // Events
        public event Action OnSelected;
        public event Action<bool> OnExpanded;

        // Properties
        public abstract string Text { get; set; }
        public abstract bool IsExpanded { get; set; }

        public abstract IEnumerable<EditorTreeNode> Nodes { get; }
        public abstract int NodeCount { get; }

        // Methods
        public abstract EditorTreeNode AddNode(string text);
        public abstract void RemoveNode(EditorTreeNode node);
    }
}
