using System;
using System.Runtime.Serialization;

namespace UniGameEngine
{
    public abstract class GameElement
    {
        // Events
        public readonly GameEvent OnWillDestroy = new GameEvent();

        // Private
        private UniGame game = null;        

        private string name = "";
        private string type = "";
        private string guid = "";

        private bool isReadOnly = false;
        private bool isDestroying = false;
        private bool isDestroyed = false;

        // Internal
        internal Type elementType = null;
        internal float scheduledDestroyTime = 0f;

        // Properties
        public UniGame Game
        {
            get { return game; }
        }

        public bool IsReadOnly
        {
            get { return isReadOnly; }
        }

        public bool IsDestroying
        {
            get { return isDestroying; }
        }

        public bool IsDestroyed
        {
            get { return isDestroyed; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Type
        {
            get { return type; }
            internal set { type = value; }
        }

        [DataMember]
        public string Guid
        {
            get
            {
                // Check for invalid - create on demand
                if (string.IsNullOrEmpty(guid) == true)
                    guid = System.Guid.NewGuid().ToString();

                return guid;
            }
            internal set { guid = value; }
        }

        // Constructor
        protected GameElement(string name)
        {
            this.name = name;
            this.game = UniGame.Current;
            this.elementType = GetType();

            // Get fallback name
            if (name == null)
                this.name = elementType.Name;
        }

        // Methods
        public override string ToString()
        {
            return string.Format("{0}({1})", GetType().FullName, name);
        }

        protected virtual void OnDestroy() { }
    }
}
