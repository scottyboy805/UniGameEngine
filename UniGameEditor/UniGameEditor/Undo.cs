using UniGameEngine;
using UniGameEngine.Content.Serializers;

namespace UniGameEditor
{
    public sealed class Undo
    {
        // Type
        private interface IUndoAction
        {
            void Perform();
            void Rollback();
        }

        private sealed class CreateElementUndoAction : IUndoAction
        {
            // Private
            private GameElement createdElement;
            private Type serializedType = null;
            private string serializedJson = null;

            // Constructor
            public CreateElementUndoAction(GameElement createdElement)
            {
                this.createdElement = createdElement;
            }

            // Methods
            public void Perform()
            {
                if (createdElement == null)
                {
                    // Create element
                    createdElement = (GameElement)Serializer.DeserializeJson(serializedJson, serializedType);

                    // Clear serailzie info
                    serializedType = null;
                    serializedJson = null;
                }
            }

            public void Rollback()
            {
                if (createdElement != null)
                {
                    // Serialize the changes
                    serializedType = createdElement.GetType();
                    serializedJson = Serializer.SerializeJson(createdElement);

                    // Destroy the element
                    GameElement.Destroy(createdElement);
                    createdElement = null;
                }                
            }
        }

        // Private
        private List<IUndoAction> actions = new List<IUndoAction>();
        private int currentAction = 0;

        // Methods
        public bool CanUndo
        {
            get { return currentAction > 0; }
        }

        public bool CanRedo
        {
            get { return currentAction < actions.Count - 1; }
        }

        // Methods
        public void PerformUndo()
        {
            if(currentAction > 0)
            {
                // Get the action
                IUndoAction action = actions[currentAction];
                currentAction--;

                // Rollback the action
                try
                {
                    action.Rollback();
                }
                catch (Exception e) { Debug.LogException(e); }
            }
        }

        public void PerformRedo()
        {
            if (CanRedo == true)
            {
                // Get the action
                IUndoAction action = actions[currentAction];
                currentAction++;

                // Perform the action
                try
                {
                    action.Perform();
                }
                catch (Exception e) { Debug.LogException(e); };
            }
        }

        public void RecordElementCreated(GameElement element)
        {
            Record(new CreateElementUndoAction(element));
        }

        private void Record(IUndoAction action)
        {
            actions.Add(action);
            currentAction++;

            // Perform the action
            try
            {
                action.Perform();
            }
            catch (Exception e) { Debug.LogException(e); };

            // Remove items after the current
            actions.RemoveRange(currentAction, actions.Count - currentAction);
        }
    }
}
