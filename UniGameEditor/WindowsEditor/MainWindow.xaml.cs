using ModernWpf;
using MonoGame.Framework.WpfInterop;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using UniGameEditor;
using UniGameEditor.Content;
using UniGameEditor.Property;
using UniGameEditor.UI;
using UniGameEditor.Windows;
using UniGameEngine;
using WindowsEditor.UI;

namespace WindowsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Private
        private UniEditor editor = new UniEditor();
        private WPFWindowManager windowManager = null;

        public MainWindow()
        {
            InitializeComponent();            

            // Set title
            Title = "UniGameEditor, " + UniGame.EngineVersion;

            // Use single graphics device per render view
            WpfGame.UseASingleSharedGraphicsDevice = true;

            // Set theme
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            ThemeManager.Current.AccentColor = Colors.LightBlue;

            // Initialize window manager
            windowManager = new WPFWindowManager(editor);

            // Initialize menu
            editor.menu = new WPFEditorMenu(Menu);
            editor.fileMenu = new WPFEditorMenu(FileMenu);
            editor.editMenu = new WPFEditorMenu(EditMenu);
            editor.contentMenu = new WPFEditorMenu(ContentMenu);
            editor.gameObjectMenu = new WPFEditorMenu(GameObjectMenu);
            editor.componentMenu = new WPFEditorMenu(ComponentMenu);
            editor.windowMenu = new WPFEditorMenu(WindowMenu);

            // Initialize windows
            windowManager.AddWindowDock(new WPFWindowControl(CenterGrid, null, CenterTab, EditorWindowLocation.Center));
            windowManager.AddWindowDock(new WPFWindowControl(RightGrid, RightSplitter, RightTab, EditorWindowLocation.Right));
            windowManager.AddWindowDock(new WPFWindowControl(LeftGrid, LeftSplitter, LeftTab, EditorWindowLocation.Left));
            windowManager.AddWindowDock(new WPFWindowControl(BottomGrid, BottomSplitter, BottomTab, EditorWindowLocation.Bottom));

            // Initialize icons and menus
            WPFEditorIcon.InitializeIconProvider();
            WPFEditorMenu.InitializeMenuProvider();

            // Initialize editors
            PropertyEditor.InitializePropertyEditors(editor);
            ContentEditor.InitializePropertyEditors(editor);

            // Listen for window loaded
            Loaded += OnLoaded;
            editor.GameInstance.OnInitialized += OnGameLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Show windows
            windowManager.ShowDefaultWindows();

            // Initialize editor
            editor.Initialize();            
        }

        protected override void OnClosing(CancelEventArgs e)
        {            
            editor.Shutdown();
            editor = null;
        }

        private void OnGameLoaded()
        {
            editor.OpenProject("../../../../../ExampleProject/ExampleProject.unigame");
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
            ThemeManager.Current.AccentColor = Colors.LightBlue;
        }
        private void Window_ThemeLight_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light;
        }
        private void Window_ThemeDark(object sender, RoutedEventArgs e)
        {
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            ThemeManager.Current.AccentColor = Colors.DarkSlateBlue;
        }
        private void Window_ThemeDark_MenuShowing(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MenuItem)sender).IsChecked = ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark;
        }
        #endregion
    }
}