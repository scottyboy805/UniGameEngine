﻿
namespace UniGameEditor.UI
{
    public abstract class EditorLabel : EditorControl
    {
        // Properties
        public abstract string Text { get; set; }
        public abstract string Tooltip { get; set; }
        public abstract EditorIcon Icon { get; set; }
    }
}