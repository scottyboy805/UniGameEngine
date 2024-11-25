
namespace UniGameEditor.UI
{
    public abstract class EditorTreeNode : EditorControl
    {
        // Events
        public event Action<EditorTreeNode> OnSelected;
        public event Action<EditorTreeNode, bool> OnExpanded;

        // Properties
        public abstract EditorLayoutControl Header { get; }
        public abstract string Tooltip { get; set; }
        public abstract bool IsExpanded { get; set; }

        public abstract IEnumerable<EditorTreeNode> Nodes { get; }
        public abstract int NodeCount { get; }

        // Methods
        public abstract EditorTreeNode AddNode();
        public abstract void RemoveNode(EditorTreeNode node);
        public abstract void ClearNodes();

        protected void OnSelectedEvent()
        {
            UniEditor.DoEvent(OnSelected, this);
        }

        protected void OnExpandedEvent(bool isExpanded)
        {
            UniEditor.DoEvent(OnExpanded, this, isExpanded);
        }
    }
}
