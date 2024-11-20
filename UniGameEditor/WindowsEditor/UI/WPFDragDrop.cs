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
                    obj.SetData("Sender", this);

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

            // Get the drop data
            object dragSender;
            object dropData;
            DragDropType type = GetDataType(e.Data, out dragSender, out dropData);

            // Check for supported type
            if(type != DragDropType.None && dragSender != this)
            {
                // Check for drop allowed
                if (DropHandler != null && DropHandler.CanDrop(type, dropData) == true)
                    element.AllowDrop = true;
            }
            e.Handled = true;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            // Get the drop data
            object dropData;
            DragDropType type = GetDataType(e.Data, out _, out dropData);

            // Check for supported type
            if (type != DragDropType.None)
            {
                // Perform the drop
                DropHandler.PerformDrop(type, dropData);

                // Reset allow drop state
                element.AllowDrop = DropHandler != null;
            }
            e.Handled = true;
        }

        private DragDropType GetDataType(IDataObject data, out object sender, out object dropData)
        {
            // Check for sender
            sender = data.GetDataPresent("Sender") == true
                ? data.GetData("Sender")
                : null;

            // Get file
            if (data.GetDataPresent(DataFormats.FileDrop) == true)
            {
                dropData = data.GetData(DataFormats.FileDrop);
                return DragDropType.File;
            }

            // Get string
            if (data.GetDataPresent(DataFormats.StringFormat) == true)
            {
                dropData = data.GetData(DataFormats.StringFormat);
                return DragDropType.String;
            }

            // Get object
            if (data.GetDataPresent("Object") == true)
            {
                dropData = data.GetData("Object");
                return DragDropType.Object;
            }

            dropData = null;
            return DragDropType.None;
        }
    }
}
