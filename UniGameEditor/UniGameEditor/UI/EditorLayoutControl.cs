
namespace UniGameEditor.UI
{
    public abstract class EditorLayoutControl : EditorControl
    {
        // Methods
        public abstract EditorLayoutControl AddHorizontalLayout();
        public abstract EditorLayoutControl AddVerticalLayout();

        public abstract EditorLabel AddLabel(string text);

        public abstract EditorRenderView AddRenderView(Action OnRender);
    }
}
