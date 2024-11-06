
namespace UniGameEditor.UI
{
    public abstract class EditorOption
    {
        // Properties
        public abstract string Text { get; set; }
        public abstract string Tooltip { get; set; }
        public abstract EditorIcon Icon { get; set; }
        public abstract bool IsSelected {  get; set; }
    }
}
