﻿
namespace UniGameEditor.UI
{
    public abstract class EditorDropdown : EditorControl
    {
        // Events
        public event Action<int> OnSelectedIndexChanged;
        public event Action<EditorOption> OnSelectedOptionChanged;

        // Properties
        public abstract int SelectedIndex { get; set; }
        public abstract EditorOption SelectedOption { get; set; }
        public abstract string Tooltip { get; set; }
        public abstract bool IsReadOnly { get; set; }

        // Methods
        public abstract EditorOption AddOption();
        public abstract void RemoveOption(EditorOption option);
    }
}
