
namespace UniGameEditor.UI
{
    public abstract class EditorSplitViewLayoutControl : EditorControl
    {
        // Properties
        public abstract EditorLayoutControl LayoutA { get; }
        public abstract EditorLayoutControl LayoutB { get; }
    }
}
