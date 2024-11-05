﻿
namespace UniGameEditor.UI
{
    public abstract class EditorControl
    {
        // Public
        public const int DefaultFontSize = 12;
        public const float DefaultLineHeight = 20;

        // Properties
        public abstract float Width { get; set; }

        public abstract float Height { get; set; }
    }
}
