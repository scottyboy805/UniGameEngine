using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    public unsafe sealed class PhysicsMaterial : GameElement
    {
        // Private
        private float staticFriction = 0.5f;
        private float dynamicFriction = 0.5f;
        private float restitution = 0f;

        // Properties
        [DataMember]
        public float StaticFriction
        {
            get { return staticFriction; }
            set
            {
                staticFriction = value;
            }
        }

        [DataMember]
        public float DynamicFriction
        {
            get { return dynamicFriction; }
            set
            {
                dynamicFriction = value;
            }
        }

        [DataMember]
        public float Restitution
        {
            get { return restitution; }
            set
            {
                restitution = value;
            }
        }

        // Constructor
        public PhysicsMaterial(string name)
            : base(name)
        {
        }

        // Methods
        protected internal override void OnLoaded()
        {
        }

        protected internal override void OnDestroy()
        {
        }
    }
}
