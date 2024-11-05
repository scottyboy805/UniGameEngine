﻿using System.Windows;
using System.Windows.Controls;
using UniGameEditor.Windows;
using UniGameEngine;
using WindowsEditor.UI;

namespace WindowsEditor
{
    internal sealed class WPFWindowControl
    {
        // Internal
        internal WPFWindowManager windowManager = null;

        // Private
        private Grid gridControl = null;
        private GridSplitter gridSplitterControl = null;
        private TabControl tabControl = null;
        private EditorWindowLocation location = 0;

        private Dictionary<EditorWindow, TabItem> displayedWindows = new Dictionary<EditorWindow, TabItem>();

        private GridLength[] initialColumnWidths;
        private GridLength[] initialRowHeights;

        // Properties
        public EditorWindowLocation Location
        {
            get { return location; }
        }

        // Constructor
        public WPFWindowControl(Grid grid, GridSplitter gridSplitter, TabControl tab, EditorWindowLocation location)
        {
            this.gridControl = grid;
            this.gridSplitterControl = gridSplitter;
            this.tabControl = tab;
            this.location = location;

            // Set grid margin
            tab.Padding = new Thickness(0, 12, 0, 12);
            grid.Margin = new Thickness(8, 0, 8, 0);

            initialColumnWidths = grid.ColumnDefinitions.Select(d => d.Width).ToArray();
            initialRowHeights = grid.RowDefinitions.Select(d => d.Height).ToArray();
            
            tabControl.UpdateDefaultStyle();
        }

        // Methods
        public void ResetLayout()
        {
            for(int i = 0; i < gridControl.ColumnDefinitions.Count; i++)
                gridControl.ColumnDefinitions[i].Width = initialColumnWidths[i];

            for(int i = 0; i < gridControl.RowDefinitions.Count; i++)
                gridControl.RowDefinitions[i].Height = initialRowHeights[i];
        }

        public bool IsWindowOpen(EditorWindow window)
        {
            return displayedWindows.ContainsKey(window);
        }

        public bool IsWindowOpen<T>() where T : EditorWindow
        {
            // Check all windows
            foreach (EditorWindow window in displayedWindows.Keys)
            {
                if (window is T)
                    return true;
            }
            return false;
        }

        public void OpenWindow(EditorWindow window)
        {
            // Check for null
            if (window == null)
                return;

            // Check for already opened
            if (displayedWindows.ContainsKey(window) == true)
                return;

            // Create root item
            Grid rootGrid = new Grid();
            window.rootControl = new WPFEditorStackLayout(rootGrid, Orientation.Vertical);
            window.isOpen = true;

            // Create tab label
            Label headerLabel = new Label();
            headerLabel.Content = window.title;
            headerLabel.ContextMenu = CreateWindowContextMenu(window);

            // Create tab item
            TabItem newTab = new TabItem
            {
                Header = headerLabel,
                Content = rootGrid,
            };

            // Create a new tab
            tabControl.Items.Add(newTab);

            // Select the tab
            tabControl.SelectedItem = newTab;

            // Add display window
            displayedWindows.Add(window, newTab);

            // Show the window
            try
            {
                window.OnShow();
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }

            // Update visibility
            RefreshGridControl();
        }

        public void CloseWindow(EditorWindow window)
        {
            // Check for null
            if (window == null)
                return;

            // Check for opened
            if (displayedWindows.ContainsKey(window) == false)
                return;

            // Get the tab
            TabItem tab = displayedWindows[window];

            // Remove window
            displayedWindows.Remove(window);

            // Remove the tab
            tabControl.Items.Remove(tab);

            // Close the window
            try
            {
                window.OnHide();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            // Reset window
            window.rootControl = null;
            window.isOpen = false;

            // Update visibility
            RefreshGridControl();
        }

        public void CloseWindow<T>() where T : EditorWindow
        {
            // Create copy to avoid modifying collection
            EditorWindow[] windowsToClose = displayedWindows.Keys.ToArray();

            // Close all
            foreach (EditorWindow window in windowsToClose)
            {
                if (window is T)
                    CloseWindow(window);
            }
        }

        public void CloseAllWindows()
        {
            // Create copy to avoid modifying collection
            EditorWindow[] windowsToClose = displayedWindows.Keys.ToArray();

            // Close all
            foreach (EditorWindow window in windowsToClose)
                CloseWindow(window);
        }

        private void RefreshGridControl()
        {
            gridControl.Visibility = displayedWindows.Count > 0
                ? Visibility.Visible
                : Visibility.Collapsed;

            gridControl.IsEnabled = displayedWindows.Count > 0;

            // Update splitter also
            if (gridSplitterControl != null)
            {
                gridSplitterControl.Visibility = gridControl.Visibility;
                gridSplitterControl.IsEnabled = gridControl.IsEnabled;
            }
        }

        private ContextMenu CreateWindowContextMenu(EditorWindow window)
        {
            ContextMenu menu = new ContextMenu();

            // Add close option
            MenuItem closeOption = new MenuItem
            {
                Header = "Close",
            };
            menu.Items.Add(closeOption);
            closeOption.Click += (object sender, RoutedEventArgs e) => CloseWindow(window);

            // Add dock option
            MenuItem dockOption = new MenuItem
            {
                Header = "Dock"
            };
            menu.Items.Add(dockOption);

            // Dock locations
            foreach(EditorWindowLocation location in Enum.GetValues<EditorWindowLocation>())
            {
                EditorWindowLocation selectedLocation = location;
                MenuItem dockSelection = new MenuItem
                {
                    Header = location.ToString(),
                };
                dockOption.Items.Add(dockSelection);
                dockSelection.Click += (object sender, RoutedEventArgs e) => windowManager.ChangeLocation(window, selectedLocation);
            }



            return menu;
        }
    }
}
