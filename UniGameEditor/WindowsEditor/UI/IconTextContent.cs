using System.Windows;
using System.Windows.Controls;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class IconTextContent
    {
        // Private
        private EditorIcon icon = null;

        // Internal
        internal Control mainControl = null;
        internal Label itemLabel = null;
        internal Image itemIcon = null;

        // Properties
        public float Width
        {
            get => (float)mainControl.Width;
            set => mainControl.Width = value;
        }

        public float Height
        {
            get => (float)mainControl.Height;
            set => mainControl.Height = value;
        }

        public string Text
        {
            get => (string)itemLabel.Content;
            set
            {
                itemLabel.Content = string.IsNullOrEmpty(value) == false
                    ? value
                    : null;
                itemLabel.IsEnabled = itemLabel.Content != null;
            }
        }

        public EditorIcon Icon
        {
            get => icon;
            set
            {
                // Check source
                itemIcon.Source = value is WPFEditorIcon
                    ? ((WPFEditorIcon)value).image
                    : null;
                itemIcon.IsEnabled = itemIcon.Source != null;
                icon = value;
            }
        }

        public string Tooltip
        {
            get => (string)mainControl.ToolTip;
            set => mainControl.ToolTip = value;
        }

        // Constructor
        public IconTextContent(Panel parent, ContentControl mainControl, string text)
        {
            this.mainControl = mainControl;

            mainControl.Content = CreateIconTextGroup(text);

            if(parent != null)
                parent.Children.Add(mainControl);            
        }

        public IconTextContent(ItemsControl parent, ContentControl mainControl, string text)
        {
            this.mainControl = mainControl;

            mainControl.Content = CreateIconTextGroup(text);

            if (parent != null)
                parent.Items.Add(mainControl);            
        }

        public IconTextContent(ItemCollection parent, ContentControl mainControl, string text)
        {
            this.mainControl = mainControl;

            mainControl.Content = CreateIconTextGroup(text);

            if (parent != null)
                parent.Add(mainControl);
        }

        public IconTextContent(TreeViewItem treeItem, string text)
        {
            treeItem.Header = CreateIconTextGroup(text);
        }

        // Methods
        private StackPanel CreateIconTextGroup(string text)
        {
            // Create stack panel
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            // Create icon
            itemIcon = new Image();
            itemIcon.IsEnabled = false;
            itemIcon.MinHeight = EditorControl.DefaultLineHeight;
            itemIcon.MaxHeight = EditorControl.DefaultLineHeight;
            itemIcon.Margin = new Thickness(0, 0, 6, 0);
            stackPanel.Children.Add(itemIcon);

            // Create label
            itemLabel = new Label();
            itemLabel.IsEnabled = string.IsNullOrEmpty(text) == false;
            itemLabel.VerticalContentAlignment = VerticalAlignment.Center;
            itemLabel.Content = text;
            itemLabel.FontSize = EditorControl.DefaultFontSize;
            itemLabel.MinHeight = EditorControl.DefaultLineHeight;
            stackPanel.Children.Add(itemLabel);

            return stackPanel;
        }
    }
}
