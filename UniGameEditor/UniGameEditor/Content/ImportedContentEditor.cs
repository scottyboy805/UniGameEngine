using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using UniGameEditor.Property;
using UniGameEditor.UI;

namespace UniGameEditor.Content
{
    [ContentEditorFor(typeof(Effect), true)]
    [ContentEditorFor(typeof(SpriteFont), true)]
    [ContentEditorFor(typeof(Texture), true)]
    [ContentEditorFor(typeof(Model), true)]
    [ContentEditorFor(typeof(Song), true)]
    [ContentEditorFor(typeof(SoundEffect), true)]
    [ContentEditorFor(typeof(Video), true)]
    public class ImportedContentEditor : ContentEditor
    {
        // Methods
        protected override void OnShow()
        {
            OnShowContentType();
            OnShowImporterAndProcessor();
            OnShowImporterProperties();
        }

        public virtual void OnShowContentType(EditorLayoutControl root = null)
        {
            // Get root
            if (root == null)
                root = RootControl;

            // Create label
            root.AddLabel(Content.DisplayName);
        }

        public virtual void OnShowImporterAndProcessor(EditorLayoutControl root = null)
        {
            // Get root
            if (root == null)
                root = RootControl;

            // Get meta
            SerializedProperty metaProperty = Content.FindSerializedName("$meta");
            SerializedProperty actionProperty = metaProperty.FindSerializedName(nameof(ContentMeta.Action));
            SerializedProperty importerProperty = metaProperty.FindSerializedName(nameof(ContentMeta.Importer));
            SerializedProperty processorProperty = metaProperty.FindSerializedName(nameof(ContentMeta.Processor));


            // Action property
            {
                // Get drawer
                PropertyEditor actionEditor = PropertyEditor.ForType(actionProperty.Property.PropertyType);

                // Create drawer
                actionEditor.CreateProperty(root, actionProperty);
            }

            // Importer property
            {
                // Get drawer
                PropertyEditor importerEditor = PropertyEditor.ForType(importerProperty.Property.PropertyType);

                // Create drawer
                importerEditor.CreateProperty(root, importerProperty);
            }

            // Processor property
            {
                // Get drawer
                PropertyEditor processorEditor = PropertyEditor.ForType(processorProperty.Property.PropertyType);

                // Create drawer
                processorEditor.CreateProperty(root, processorProperty);
            }
        }

        public virtual void OnShowImporterProperties(EditorLayoutControl root = null)
        {
            // Get root
            if (root == null)
                root = RootControl;
        }
    }
}
