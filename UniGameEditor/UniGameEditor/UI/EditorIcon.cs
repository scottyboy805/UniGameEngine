
using UniGameEngine;

namespace UniGameEditor.UI
{
    public abstract class EditorIcon
    {
        // Private        
        private static readonly Dictionary<string, EditorIcon> loadedIcons = new Dictionary<string, EditorIcon>();

        private string iconName = null;
        private Uri iconSource = null;

        // Protected
        protected static Func<string, EditorIcon> IconProvider = null;

        // Properties
        public Uri IconSource
        {
            get { return iconSource; }
        }

        // Constructor
        protected EditorIcon(Uri iconSource, string iconName)
        {
            this.iconSource = iconSource;
            this.iconName = iconName;
        }

        // Methods
        public static EditorIcon FindIcon(string iconName)
        {
            // Check for empty
            if (string.IsNullOrEmpty(iconName) == true)
                return null;

            // Check for cached
            EditorIcon icon;
            if (loadedIcons.TryGetValue(iconName, out icon) == true)
                return icon;

            // Check for provider
            if (IconProvider == null)
                throw new InvalidOperationException("Icon provider has not been setup for the host application");

            try
            {
                // Create the icon instance
                icon = IconProvider(iconName);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                return null;
            }

            // Cache icon
            loadedIcons[iconName] = icon;
            return icon;
        }
    }
}
