
namespace UniGameEditor.UI
{
    public abstract class EditorToggleButton : EditorControl
    {
        // Events
        public event Action OnClicked;
        public event Action<bool> OnChecked;

        // Properties
        public abstract string Text { get; set; }
        public abstract bool IsChecked { get; set; }   
    }
}
