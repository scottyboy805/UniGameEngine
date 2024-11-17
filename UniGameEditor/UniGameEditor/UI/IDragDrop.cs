
namespace UniGameEditor.UI
{
    public enum DragDropVisual
    {
        Copy,
        Move,
    }

    public interface IDragHandler
    {
        // Methods
        bool PerformDrag(out object dragData, out DragDropVisual visual);
    }

    public interface IDropHandler
    {
        // Methods
        bool CanDrop(object dragData);
        void PerformDrop(object dragData);
    }
}
