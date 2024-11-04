using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace UniGameEngine
{
    public abstract class GameElement
    {
        // Private
        private UniGame game = null;        

        private string name = "";
        private string guid = "";
        private string contentPath = "";

        private bool isReadOnly = false;
        private bool isDestroying = false;
        private bool isDestroyed = false;

        // Internal
        internal static ConstructorInfo initializer = typeof(GameElement).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);

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

        public string ContentPath
        {
            get { return contentPath; }
            internal set { contentPath = value; }
        }

        // Constructor
        private GameElement() : this(null) { }

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

        protected internal virtual void OnLoaded() { }

        protected internal virtual void OnDestroy() { }

        public static void Destroy(GameElement element)
        {
            Destroy<GameElement>(element);
        }

        public static void Destroy<T>(T element) where T : GameElement
        {
            // Check for null
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            // Add for destruction
            if (element.Game.scheduleDestroyElements.Contains(element) == false)
                element.Game.scheduleDestroyElements.Enqueue(element);
        }

        public static void DestroyDelayed(GameElement element, float delay)
        {
            DestroyDelayed<GameElement>(element, delay);
        }

        public static void DestroyDelayed<T>(T element, float delay) where T : GameElement
        {
            // Check for delay
            if (delay > 0f)
            {
                // Check for null
                if (element == null)
                    throw new ArgumentNullException(nameof(element));

                // Add for delayed destruction
                if(element.Game.scheduledDestroyDelayElements.Contains(element) == false)
                {
                    element.scheduledDestroyTime = delay;
                    element.Game.scheduledDestroyDelayElements.Add(element);
                }
            }
            else
            {
                // Destroy now
                Destroy<T>(element);
            }
        }

        internal static void DoGameElementLoadedEvents(GameElement element)
        {
            // Check for null
            if (element == null)
                return;

            // Trigger event
            try
            {
                element.OnLoaded();
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
