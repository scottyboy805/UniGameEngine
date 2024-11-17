using System.Windows;
using System.Windows.Input;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFDragDrop
    {
        // Private
        private UIElement element = null;

        // Internal
        internal IDragHandler dragHandler = null;
        internal IDropHandler dropHandler = null;

        // Properties
        public IDragHandler DragHandler
        {
            get => dragHandler;
            set
            {
                dragHandler = value;
                element.AllowDrop = value != null;
            }
        }

        public IDropHandler DropHandler
        {
            get => dropHandler;
            set
            {
                dropHandler = value;
                element.AllowDrop = value != null;
            }
        }

        // Constructor
        public WPFDragDrop(UIElement element)
        {
            if(element == null)
                throw new ArgumentNullException(nameof(element));

            this.element = element;

            // Add listeners
            element.MouseMove += OnMouseMove;
            element.DragOver += OnDragOver;
            element.Drop += OnDrop;
        }

        // Methods
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Check for drag handler
            if (DragHandler != null && e.LeftButton == MouseButtonState.Pressed)
            {
                // Check for drag begin
                object data;
                DragDropVisual visual;
                if (DragHandler.PerformDrag(out data, out visual) == true)
                {
                    // Commence the drag operation
                    DataObject obj = new DataObject("Object", data);

                    // Check for string
                    if (data is string)
                        obj.SetText((string)data);

                    // Get the visual
                    DragDropEffects effect = visual switch
                    {
                        DragDropVisual.Move => DragDropEffects.Move,
                        _ => DragDropEffects.Copy,
                    };

                    // Start the operation
                    DragDrop.DoDragDrop(element, obj, effect);
                }
            }
        }

        private void OnDragOver(object sender, DragEventArgs e) 
        {
            element.AllowDrop = false;
            if (e.Data.GetDataPresent("Object") == true)
            {
                // Get the object
                object obj = e.Data.GetData("Object");

                // Check for drop allowed
                if (DropHandler != null && DropHandler.CanDrop(obj) == true)
                {
                    element.AllowDrop = true;
                }
            }
            e.Handled = true;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object") == true)
            {
                // Get the object
                object obj = e.Data.GetData("Object");

                // Drop the object
                DropHandler.PerformDrop(obj);

                // Reset allow drop state
                element.AllowDrop = dropHandler != null;
            }
            e.Handled = true;
        }
    }
}
