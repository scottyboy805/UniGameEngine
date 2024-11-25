
namespace UniGameEditor.UI
{
    public abstract class EditorDropdown : EditorControl
    {
        // Events
        public event Action<EditorDropdown, int> OnSelectedIndexChanged;
        public event Action<EditorDropdown, EditorOption> OnSelectedOptionChanged;

        // Properties
        public abstract int SelectedIndex { get; set; }
        public abstract EditorOption SelectedOption { get; set; }
        public abstract string Tooltip { get; set; }
        public abstract bool IsReadOnly { get; set; }

        // Methods
        public abstract EditorOption AddOption();
        public abstract void RemoveOption(EditorOption option);

        protected void OnSelectedIndexChangedEvent(int index)
        {
            UniEditor.DoEvent(OnSelectedIndexChanged, this, index);
        }

        protected void OnSelectedOptionChangedEvent(EditorOption option)
        {
            UniEditor.DoEvent(OnSelectedOptionChanged, this, option);
        }
    }
}
