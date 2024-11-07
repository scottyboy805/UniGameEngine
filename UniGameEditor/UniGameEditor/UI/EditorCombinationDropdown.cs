
namespace UniGameEditor.UI
{
    public abstract class EditorCombinationDropdown : EditorControl
    {
        // Events
        public event Action<int[]> OnSelectedIndexesChanged;
        public event Action<EditorOption[]> OnSelectedOptionsChanged;

        // Properties
        public abstract int SelectedCount { get; }
        public abstract int[] SelectedIndexes { get; set; }
        public abstract EditorOption[] SelectedOptions { get; set; }

        // Methods
        public abstract EditorOption AddOption(string text);
        public abstract void RemoveOption(EditorOption option);
    }
}
