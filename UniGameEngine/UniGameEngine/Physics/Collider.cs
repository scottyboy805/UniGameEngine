using Jitter2.Collision.Shapes;
using Jitter2.LinearMath;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using JitterBody = Jitter2.Dynamics.RigidBody;

namespace UniGameEngine.Physics
{
    public abstract class Collider : Component
    {
        // Private
        private bool isTrigger = false;
        private PhysicsMaterial material = null;

        // Internal
        internal RigidBodyShape physicsShape = null;
        internal JitterBody staticBody = null;
        internal RigidBody attachedBody = null;

        // Properties
        public virtual BoundingBox Bounds
        {
            get
            {
                // Get the box
                JBBox box = physicsShape.WorldBoundingBox;
                return Unsafe.As<JBBox, BoundingBox>(ref box);
            }
        }

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
            get 
            {
                if (attachedBody != null)
                    return attachedBody;

                return GameObject.GetComponentInParent<RigidBody>(); 
            }
        }

        // Methods
        protected override void RegisterSubSystems()
        {
            // Register collider
            Physics.activeColliders[physicsShape] = this;

            // Add static
            if (Body == null)
            {
                // Create static body
                staticBody = Physics.physicsWorld.CreateRigidBody();
                staticBody.AddShape(physicsShape);
                staticBody.Tag = this;
                staticBody.IsStatic = true;

                // Rebuild
                RebuildCollider();

                // Update attached body
                attachedBody = Body;
            }
            // Add dynamic
            else
            {
                Body.AttachCollider(this);
            }
        }

        protected override void UnregisterSubSystems()
        {
            // Remove dynamic
            if (attachedBody != null)
            {
                attachedBody.DetachCollider(this);
                attachedBody = null;
            }
            else
            {
                // Remove shape from body
                staticBody.RemoveShape(physicsShape);
            }

            // Remove collider
            Physics.activeColliders.Remove(physicsShape);
        }

        internal virtual void RebuildCollider() 
        {
            // Update transform
            PhysicsSimulation.ApplyTransform(staticBody, Transform);
        }
    }
}
