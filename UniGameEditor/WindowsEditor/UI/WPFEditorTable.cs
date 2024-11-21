using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using UniGameEditor.UI;
using UniGameEngine.Content.Contract;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorTable : EditorTable
    {
        // Internal
        internal WPFDragDrop dragDrop = null;
        internal DataGrid dataGrid = null;

        // Properties
        public override float Width
        {
            get => (float)dataGrid.Width;
            set => dataGrid.Width = value;
        }

        public override float Height
        {
            get => (float)dataGrid.Height;
            set => dataGrid.Height = value;
        }

        public override IDragHandler DragHandler
        {
            get => dragDrop.DragHandler;
            set => dragDrop.DragHandler = value;
        }

        public override IDropHandler DropHandler
        {
            get => dragDrop.DropHandler;
            set => dragDrop.DropHandler = value;
        }

        public override EditorMenu ContextMenu
        {
            get => contextMenu;
            set
            {
                contextMenu = value;
                dataGrid.ContextMenu = value != null
                    ? ((WPFEditorMenu)value).menu
                    : null;
            }
        }

        // Constructor
        public WPFEditorTable(Panel parent)
        {
            dataGrid = new DataGrid();
            dragDrop = new WPFDragDrop(dataGrid);

            dataGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            dataGrid.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            dataGrid.SelectionMode = DataGridSelectionMode.Single;
            dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            dataGrid.FontSize = DefaultFontSize;
            dataGrid.RowHeight = DefaultLineHeight;


            parent.Children.Add(dataGrid);
        }

        public WPFEditorTable(ItemsControl parent)
        {
            dataGrid = new DataGrid();
            dragDrop = new WPFDragDrop(dataGrid);

            dataGrid.SelectionMode = DataGridSelectionMode.Single;
            dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            dataGrid.FontSize = DefaultFontSize;
            dataGrid.RowHeight = DefaultLineHeight;

            parent.Items.Add(dataGrid);
        }

        // Methods
        public override void ShowData<T>(IEnumerable<T> dataRows)
        {
            // Get the contract
            DataContract contract = DataContract.ForType(typeof(T));

            // Check for no properties
            if(contract.HasSerializeProperties == false)
            {
                dataGrid.ItemsSource = null;
                return;
            }

            // Create the table
            DataTable table = new DataTable();

            // Build the columns
            foreach(DataContractProperty property in contract.SerializeProperties)
            {
                // Add the column
                table.Columns.Add(property.SerializeName, property.PropertyType);
            }

            // Add all rows
            foreach(T data in dataRows)
            {
                // Get the properties
                object[] valuesArray = new object[contract.SerializeProperties.Count];

                // Fetch all data
                for(int i = 0; i < valuesArray.Length; i++)
                {
                    // Get the property
                    DataContractProperty property = contract.SerializeProperties[i];

                    // Read the value
                    valuesArray[i] = property.GetInstanceValue(data);
                }

                // Add the row
                table.Rows.Add(valuesArray);
            }

            // Apply the table
            dataGrid.ItemsSource = table.DefaultView;
        }
    }
}
