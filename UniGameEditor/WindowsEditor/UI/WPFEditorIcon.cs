using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniGameEditor.UI;

namespace WindowsEditor.UI
{
    internal sealed class WPFEditorIcon : EditorIcon
    {
        // Private
        private const string resourcesBase = @"pack://application:,,,/_Icon/";

        // Internal
        internal ImageSource image = null;

        // Constructor
        public WPFEditorIcon(string iconName)
            : base(InitializeIcon(iconName), iconName)
        {
            image = new BitmapImage(IconSource);
        }

        // Methods
        private static Uri InitializeIcon(string iconName)
        {
            // Get the full icon path
            string iconPath = resourcesBase + iconName;

            // Check for extension
            if (Path.HasExtension(iconName) == false)
                iconPath += ".png";

            // Create the resources uri
            return new Uri(iconPath);            
        }


        public static void InitializeIconProvider()
        {
            IconProvider = CreatePlatformIcon;
        }

        private static EditorIcon CreatePlatformIcon(string iconName)
        {
            return new WPFEditorIcon(iconName);
        }
    }
}
