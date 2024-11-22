
namespace UniGameEditor.UI
{
    public abstract class EditorFoldout : EditorLayoutControl
    {
        // Events
        public event Action<EditorFoldout, bool> OnExpanded;

        // Properties
        public abstract EditorLayoutControl Header { get; }
        public abstract string Tooltip { get; set; }
        public abstract bool IsExpanded { get; set; }

        // Methods
        protected void OnExpandedEvent(bool isExpanded)
        {
            OnExpanded?.Invoke(this, isExpanded);
        }
    }
}
