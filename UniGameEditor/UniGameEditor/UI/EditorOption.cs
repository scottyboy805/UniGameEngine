
namespace UniGameEditor.UI
{
    public abstract class EditorOption
    {
        // Properties
        public abstract EditorLayoutControl Content { get; }
        public abstract string Tooltip { get; set; }
        public abstract bool IsSelected {  get; }
    }
}
