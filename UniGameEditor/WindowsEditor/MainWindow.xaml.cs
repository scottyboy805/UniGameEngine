

using ModernWpf;
using System.Windows;
using System.Windows.Controls;
using UniGameEditor.Windows;
using UniGameEngine;

namespace WindowsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Private
        private WPFWindowManager windowManager = new WPFWindowManager();

        public MainWindow()
        {
            InitializeComponent();

            // Set title
            Title = "UniGameEditor, " + UniGame.EngineVersion;

            // Set theme
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;

            // Initialize windows
            windowManager.AddWindowDock(new WPFWindowControl(CenterGrid, null, CenterTab, EditorWindowLocation.Center));
            windowManager.AddWindowDock(new WPFWindowControl(RightGrid, RightSplitter, RightTab, EditorWindowLocation.Right));
            windowManager.AddWindowDock(new WPFWindowControl(LeftGrid, LeftSplitter, LeftTab, EditorWindowLocation.Left));
            windowManager.AddWindowDock(new WPFWindowControl(BottomGrid, BottomSplitter, BottomTab, EditorWindowLocation.Bottom));

            // Show windows
            windowManager.ShowDefaultWindows();
        }

        #region WindowEvents
        private void Window_ResetClicked(object sender, RoutedEventArgs e)
        {
            windowManager.ResetLayout();
        }

        private void Window_Hierarchy(object sender, RoutedEventArgs e)
        {
            // Show or hide
            windowManager.SetWindowOpen<HierarchyEditorWindow>(((MenuItem)sender).IsChecked, EditorWindowLocation.Left);
        }
        private void Window_Hierarchy_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = windowManager.IsWindowOpen<HierarchyEditorWindow>();
        }
        
        private void Window_Properties(object sender, RoutedEventArgs e)
        {
            // Show or hide
            windowManager.SetWindowOpen<PropertiesEditorWindow>(((MenuItem)sender).IsChecked, EditorWindowLocation.Right);
        }
        private void Window_Properties_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = windowManager.IsWindowOpen<PropertiesEditorWindow>();
        }

        private void Window_Content(object sender, RoutedEventArgs e)
        {
            // Show or hide
            windowManager.SetWindowOpen<ContentEditorWindow>(((MenuItem)sender).IsChecked, EditorWindowLocation.Bottom);
        }
        private void Window_Content_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = windowManager.IsWindowOpen<ContentEditorWindow>();
        }

        private void Window_Console(object sender, RoutedEventArgs e)
        {
            // Show or hide
            windowManager.SetWindowOpen<ConsoleEditorWindow>(((MenuItem)sender).IsChecked, EditorWindowLocation.Bottom);
        }
        private void Window_Console_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = windowManager.IsWindowOpen<ConsoleEditorWindow>();
        }

        private void Window_Scene(object sender, RoutedEventArgs e)
        {
            // Show or hide
            windowManager.SetWindowOpen<SceneEditorWindow>(((MenuItem)sender).IsChecked, EditorWindowLocation.Center);
        }
        private void Window_Scene_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = windowManager.IsWindowOpen<SceneEditorWindow>();
        }

        private void Window_Game(object sender, RoutedEventArgs e)
        {
            // Show or hide
            windowManager.SetWindowOpen<GameEditorWindow>(((MenuItem)sender).IsChecked, EditorWindowLocation.Center);
        }
        private void Window_Game_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = windowManager.IsWindowOpen<GameEditorWindow>();
        }

        private void Window_ThemeLight(object sender, RoutedEventArgs e)
        {
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
        }
        private void Window_ThemeLight_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light;
        }
        private void Window_ThemeDark(object sender, RoutedEventArgs e)
        {
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
        }
        private void Window_ThemeDark_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark;
        }
        #endregion
    }
}