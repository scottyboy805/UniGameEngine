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

                    // Clear serialize info
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

        private sealed class ModifyPropertyUndoAction<T> : IUndoAction
        {
            // Private
            private SerializedProperty property = null;
            private T originalValue = default;
            private T newValue = default;

            // Constructor
            public ModifyPropertyUndoAction(SerializedProperty property, in T newValue)
            {
                this.property = property;
                this.newValue = newValue;

                // Get the original value
                property.GetValue(out originalValue, out _);

                // Perform modify now
                Perform();
            }

            // Methods
            public void Perform()
            {
                // Modify the property
                property.SetValue(newValue);
            }

            public void Rollback()
            {
                // Revert the property
                property.SetValue(originalValue);
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

        public void RecordElementCreated(GameElement newElement)
        {
            Record(new CreateElementUndoAction(newElement));
        }

        public void RecordPropertyModified<T>(SerializedProperty property, in T newValue)
        {
            Record(new ModifyPropertyUndoAction<T>(property, newValue));
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
