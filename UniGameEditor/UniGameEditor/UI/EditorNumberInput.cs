﻿
namespace UniGameEditor.UI
{
    public abstract class EditorNumberInput : EditorControl
    {
        // Properties
        public abstract long ValueInteger { get; set; }
        public abstract double Value { get; set; }
        public abstract double MinValue { get; set; }
        public abstract double MaxValue { get; set; }
        
    }
}