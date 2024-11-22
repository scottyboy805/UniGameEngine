
namespace UniGameEditor.UI
{
    public abstract class EditorControl
    {
        // Public
        public const int DefaultFontSize = 12;
        public const float DefaultLineHeight = 20;
        public const float DefaultControlHeight = 32;
        public const float DefaultHeaderHeight = 30;

        // Protected
        protected EditorMenu contextMenu = null;

        // Properties
        public abstract float Width { get; set; }

        public abstract float Height { get; set; }

        public abstract IDragHandler DragHandler { get; set; }

        public abstract IDropHandler DropHandler { get; set; }

        public abstract EditorMenu ContextMenu { get; set; }
    }
}
