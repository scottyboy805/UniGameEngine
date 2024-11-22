
namespace UniGameEditor.UI
{
    public abstract class EditorButton : EditorControl
    {
        // Events
        public event Action OnClicked;

        // Properties
        public abstract EditorLayoutControl Content { get; }
        public abstract string Tooltip { get; set; }

        // Methods
        public abstract void Perform();
    }
}
