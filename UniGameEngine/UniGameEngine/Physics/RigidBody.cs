using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public unsafe sealed class RigidBody : Component
    {
        // Private
        private Collider mainCollider = null;
        private List<Collider> attachedColliders = null;

        private float mass = 1f;

        // Internal
        internal BodyHandle bodyHandle = default;
        internal Vector3 velocity = default;
        internal Vector3 angularVelocity = default;

        // Properties
        public PhysicsSimulation Physics
        {
            get { return Game.Physics; }
        }

        // Methods
        protected override void RegisterSubSystems()
        {
            // Check for no colliders
            if (mainCollider == null)
                return;

            TypedIndex shape = default;
            BodyInertia inertia = default;

            // Check for compound collider
            if(attachedColliders != null)
            {

            }
            // Single collider
            else
            {
                // Get shape
                shape = mainCollider.CreatePhysicsShape();

                // Get inertia
                inertia = mainCollider.GetBodyInertia(mass);
            }

            // Add body
            bodyHandle = Physics.simulation.Bodies.Add(
                BodyDescription.CreateDynamic(
                    PhysicsSimulation.GetPose(Transform),
                    inertia, 
                    new CollidableDescription(shape),
                    new BodyActivityDescription(0.01f)));

            // Register body with simulation
            Physics.dynamicBodies[bodyHandle] = this;
        }

        protected override void UnregisterSubSystems()
        {
            // Check for handle
            if (bodyHandle != default)
            {
                // Remove body
                Physics.dynamicBodies.Remove(bodyHandle);

                // Remove dynamic
                Physics.simulation.Bodies.Remove(bodyHandle);
                bodyHandle = default;
            }
        }

        internal void AttachCollider(Collider collider)
        {
            if(mainCollider != null)
            {
                // Create list if required
                if (attachedColliders == null)
                {
                    attachedColliders = new List<Collider>(8);
                    attachedColliders.Add(mainCollider);
                }

                // Update colliders
                attachedColliders.Add(collider);
            }
            else
                mainCollider = collider;
        }

        internal void DetachCollider(Collider collider)
        {
            if(attachedColliders != null)
            {
                // Remove if found
                if(attachedColliders.Contains(collider) == true)
                    attachedColliders.Remove(collider);

                // Check for none
                if (attachedColliders.Count == 0)
                    mainCollider = null;
            }

            // Check for main
            if(mainCollider == collider && attachedColliders.Count > 0)
                mainCollider = attachedColliders[0];
        }
    }
}
