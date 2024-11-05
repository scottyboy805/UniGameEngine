
namespace UniGameEditor.UI
{
    public abstract class EditorInput : EditorControl
    {
        // Events
        public event Action<string> OnTextChanged;

        // Properties
        public abstract string Text { get; set; }
    }
}
