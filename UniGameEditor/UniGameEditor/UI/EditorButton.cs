﻿
namespace UniGameEditor.UI
{
    public abstract class EditorButton : EditorControl
    {
        // Events
        public event Action OnClicked;

        // Properties
        public abstract string Text { get; set; }
        public abstract string Tooltip { get; set; }
        public abstract EditorIcon Icon { get; set; }

        // Methods
        public abstract void Perform();
    }
}