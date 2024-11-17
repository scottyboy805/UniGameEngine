
namespace UniGameEditor.UI
{
    public abstract class EditorFoldout : EditorLayoutControl
    {
        // Events
        public event Action<EditorFoldout, bool> OnExpanded;

        // Properties
        public abstract string Text { get; set; }
        public abstract bool IsExpanded { get; set; }

        protected void OnExpandedEvent(bool isExpanded)
        {
            OnExpanded?.Invoke(this, isExpanded);
        }
    }
}
