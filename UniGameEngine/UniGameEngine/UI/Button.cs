using System.Runtime.Serialization;

namespace UniGameEngine.UI
{
    [DataContract]
    public class Button : Image
    {
        // Events
        [DataMember]
        public GameEvent OnClicked = new GameEvent();

        // Methods
        public virtual void Perform()
        {
            // Trigger event
            OnClicked.Raise();
        }
    }
}
