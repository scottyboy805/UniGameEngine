
using Microsoft.Xna.Framework;
using UniGameEditor.Content;
using UniGameEditor.Property;
using UniGameEditor.UI;
using UniGameEngine;
using UniGameEngine.Content.Contract;

namespace UniGameEditor.Windows
{
    class TestObject
    {
        public Vector3 MyVecProperty = new Vector3(1, 2, 3);
        public int number;
        public string value;
    }


    internal sealed class PropertiesEditorWindow : EditorWindow
    {
        // Constructor
        public PropertiesEditorWindow()
        {
            icon = EditorIcon.FindIcon("Properties");
            title = "Properties";
        }

        // Methods
        protected internal override void OnShow()
        {
            RootControl.AddFoldoutLayout("My Foldout").AddButton("Hello");


            SerializedContent content = new SerializedContent(typeof(GameObject), new[] { new GameObject("Test") });

            // Create the editor
            ContentEditor editor = ContentEditor.ForType<GameObject>();

            // Initialize editor
            editor.CreateContent(RootControl, content);

            //PropertyEditor editor = PropertyEditor.ForType(typeof(Vector3));

            //DataContract contract = DataContract.ForType(typeof(TestObject));

            //// Create the property
            //editor.CreateProperty(RootControl, new SerializedProperty(contract.SerializeProperties[0], new[] { new TestObject() }));
        }
    }
}
