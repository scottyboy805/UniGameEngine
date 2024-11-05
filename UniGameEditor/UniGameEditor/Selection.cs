using UniGameEngine;

namespace UniGameEditor
{
    public sealed class Selection
    {
        // Events
        public event Action OnSelectionChanged;

        // Private
        private List<object> selectedObjects = new List<object>();

        // Properties
        public bool HasAnySelection
        {
            get { return selectedObjects.Count > 0; }
        }

        // Methods
        public GameElement GetSelectedElement()
        {
            foreach(object selection in selectedObjects)
            {
                if (selection is GameElement)
                    return (GameElement)selection;
            }
            return null;
        }

        public void Select<T>(T selection)
        {
            // Clear current selection
            selectedObjects.Clear();

            // Add selection
            if(selection != null)
                selectedObjects.Add(selection);

            // Trigger event
            OnSelectionChanged();
        }

        public void Select<T>(IEnumerable<T> selected)
        {
            // Clear current selection
            selectedObjects.Clear();

            // Add selection
            foreach(T selection in selected)
            {
                if(selected != null)
                    selectedObjects.Add(selection);
            }

            // Trigger event
            OnSelectionChanged();
        }
    }
}
