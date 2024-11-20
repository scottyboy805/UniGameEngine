
namespace UniGameEditor.UI
{
    public enum DragDropVisual
    {
        Copy,
        Move,
    }

    public enum DragDropType
    {
        None = 0,
        Object,
        String,
        File,
    }

    public interface IDragHandler
    {
        // Methods
        bool PerformDrag(out object dragData, out DragDropVisual visual);
    }

    public interface IDropHandler
    {
        // Methods
        bool CanDrop(DragDropType type, object dragData);
        void PerformDrop(DragDropType type, object dragData);
    }
}
