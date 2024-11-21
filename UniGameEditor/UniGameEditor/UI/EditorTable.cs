
namespace UniGameEditor.UI
{
    public abstract class EditorTable : EditorControl
    {
        // Methods
        public abstract void ShowData<T>(IEnumerable<T> dataRows);
    }
}
