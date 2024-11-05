
namespace UniGameEditor.UI
{
    public abstract class EditorFoldout : EditorLayoutControl
    {
        // Properties
        public abstract string Text { get; set; }
        public abstract bool IsExpanded { get; set; }
    }
}
