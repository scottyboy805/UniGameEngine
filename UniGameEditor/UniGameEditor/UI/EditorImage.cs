
namespace UniGameEditor.UI
{
    public abstract class EditorImage : EditorControl
    {
        // Properties
        public abstract EditorIcon Icon { get; set; }
        public abstract string Tooltip { get; set; }
    }
}
