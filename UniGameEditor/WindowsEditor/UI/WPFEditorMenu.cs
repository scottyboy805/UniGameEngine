using System.Windows;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorMenuItem : EditorMenuItem
    {
        // Internal
        internal MenuItem item = null;

        // Properties
        public override string Text 
        { 
            get => (string)item.Header; 
            set => item.Header = value; 
        }

        public override bool IsEnabled 
        { 
            get => item.IsEnabled; 
            set => item.IsEnabled = value; 
        }

        public override bool IsChecked 
        { 
            get => item.IsChecked; 
            set => item.IsChecked = value; 
        }

        // Constructor
        public WPFEditorMenuItem(string text)
        {            
            item = new MenuItem
            {
                Header = text,                
            };
            item.Click += (object sender, RoutedEventArgs e) => InvokeOnClicked();
            item.IsVisibleChanged += (object sender, DependencyPropertyChangedEventArgs e) =>
            {
                if (item.IsVisible == true)
                    InvokeOnShown();
            };
        }

        // Methods
        public override EditorMenuItem AddItem(string text)
        {
            // Create item
            WPFEditorMenuItem item = new WPFEditorMenuItem(text);
            this.item.Items.Add(item.item);

            return item;
        }

        public override void RemoveItem(EditorMenuItem item)
        {
            if (item is WPFEditorMenuItem wpfItem && this.item.Items.Contains(wpfItem.item) == true)
                this.item.Items.Remove(wpfItem.item);
        }

        public override void AddSeparator()
        {
            this.item.Items.Add(new Separator());
        }
    }

    internal sealed class WPFEditorMenu : EditorMenu
    {
        // Internal
        internal ContextMenu menu = null;
        internal ItemCollection items = null;

        // Constructor
        public WPFEditorMenu()
        {
            menu = new ContextMenu();
            items = menu.Items;
        }

        public WPFEditorMenu(Menu existingMenu)
        {
            items = existingMenu.Items;
        }

        public WPFEditorMenu(MenuItem parent)
        {
            items = parent.Items;
        }

        // Methods
        public override EditorMenuItem AddItem(string text)
        {
            // Create item
            WPFEditorMenuItem item = new WPFEditorMenuItem(text);
            items.Add(item.item);

            return item;
        }

        public override void RemoveItem(EditorMenuItem item)
        {
            if(item is WPFEditorMenuItem wpfItem && menu.Items.Contains(wpfItem.item) == true)
                items.Remove(wpfItem.item);
        }

        public override void AddSeparator()
        {
            items.Add(new Separator());
        }

        public static void InitializeMenuProvider()
        {
            MenuProvider = CreatePlatformMenu;
        }

        private static EditorMenu CreatePlatformMenu()
        {
            return new WPFEditorMenu();
        }
    }
}
