using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    public abstract class Collider : Component, IGameEnable
    {
        // Private
        private bool isTrigger = false;
        private PhysicsMaterial material = null;

        // Internal
        internal TypedIndex physicsTypeIndex = default;
        internal StaticHandle staticHandle = default;

        // Properties
        public abstract BoundingBox Bounds { get; }

        [DataMember]
        public bool IsTrigger
        {
            get { return isTrigger; }
            set 
            { 
                isTrigger = value;
                RebuildCollider();
            }
        }

        [DataMember]
        public PhysicsMaterial Material
        {
            get { return material; }
            set 
            { 
                material = value; 
                RebuildCollider();
            }
        }

        public PhysicsSimulation Physics
        {
            get { return Game.Physics; }
        }

        public RigidBody Body
        {
            get { return GameObject.GetComponentInParent<RigidBody>(); }
        }

        // Methods
        void IGameEnable.OnEnable()
        {
            // Add type index
            physicsTypeIndex = CreatePhysicsShape();

            // Add static
            if (Body == null)
            {
                // Create static shape
                staticHandle = Physics.simulation.Statics.Add(
                    new StaticDescription(
                        PhysicsSimulation.GetPose(Transform),
                        physicsTypeIndex));
            }
            // Add dynamic
            else
            {
                Body.AttachCollider(this);
            }
        }

        void IGameEnable.OnDisable()
        {
            // Remove dynamic
            if(Body != null)
            {
                Body.DetachCollider(this);
            }

            // Remove static
            if (staticHandle != default)
            {
                // Remove statics
                Physics.simulation.Statics.Remove(staticHandle);
                staticHandle = default;
            }

            Physics.simulation.Shapes.Remove(physicsTypeIndex);
            physicsTypeIndex = default;
        }

        internal abstract TypedIndex CreatePhysicsShape();

        internal abstract BodyInertia GetBodyInertia(float mass);

        internal void RebuildCollider()
        {

        }
    }
}
