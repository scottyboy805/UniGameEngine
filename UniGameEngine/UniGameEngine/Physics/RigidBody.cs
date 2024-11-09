using Jitter2.LinearMath;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using JitterBody = Jitter2.Dynamics.RigidBody;

namespace UniGameEngine.Physics
{
    [DataContract]
    public sealed class RigidBody : Component
    {
        // Private
        private Collider mainCollider = null;
        private List<Collider> attachedColliders = null;

        private float mass = 1f;
        private bool isKinematic = false;
        private float linearDamping = 0f;
        private float angularDamping = 0.5f;

        // Internal
        internal JitterBody dynamicBody = null;
        internal Vector3 velocity = default;
        internal Vector3 angularVelocity = default;

        // Properties
        public PhysicsSimulation Physics
        {
            get { return Game.Physics; }
        }

        [DataMember]
        public float Mass
        {
            get { return mass; }
            set
            {
                mass = value;
                RebuildBody();
            }
        }

        [DataMember]
        public bool IsKinematic
        {
            get { return isKinematic; }
            set
            {
                isKinematic = value;
                RebuildBody();
            }
        }

        [DataMember]
        public float LinearDamping
        {
            get { return linearDamping; }
            set
            {
                linearDamping = value;
                RebuildBody();
            }
        }

        [DataMember]
        public float AngularDamping
        {
            get { return angularDamping; }
            set
            {
                angularDamping = value;
                RebuildBody();
            }
        }

        [DataMember]
        public Vector3 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
                RebuildBody();
            }
        }

        [DataMember]
        public Vector3 AngularVelocity
        {
            get { return angularVelocity; }
            set
            {
                angularVelocity = value;
                RebuildBody();
            }
        }

        public Vector3 Torque
        {
            get
            {
                JVector torque = dynamicBody.Torque;
                return Unsafe.As<JVector, Vector3>(ref torque);
            }
            set
            {
                dynamicBody.Torque = Unsafe.As<Vector3, JVector>(ref value);
            }
        }

        // Methods
        public void AddForce(Vector3 velocity)
        {
            dynamicBody.AddForce(Unsafe.As<Vector3, JVector>(ref velocity));
        }

        public void AddForce(Vector3 velocity, Vector3 position)
        {
            dynamicBody.AddForce(
                Unsafe.As<Vector3, JVector>(ref velocity),
                Unsafe.As<Vector3, JVector>(ref position));
        }

        public void AddTorque(Vector3 torque)
        {
            Torque += torque;
        }

        protected override void RegisterSubSystems()
        {
            // Check for no colliders
            if (mainCollider == null)
                return;

            // Create the body
            dynamicBody = Physics.physicsWorld.CreateRigidBody();
            dynamicBody.Tag = this;

            // Check for compound collider
            if(attachedColliders != null)
            {
                // Add shapes
                foreach (Collider collider in attachedColliders)
                    dynamicBody.AddShape(collider.physicsShape);
            }
            // Single collider
            else
            {
                // Use main collider
                dynamicBody.AddShape(mainCollider.physicsShape);
            }

            // Rebuild the body
            RebuildBody();
        }

        protected override void UnregisterSubSystems()
        {
            // Check for handle
            if (dynamicBody != null)
            {
                if (attachedColliders != null)
                {
                    // Remove shapes
                    foreach(Collider collider in attachedColliders)
                        dynamicBody.RemoveShape(collider.physicsShape);
                }
                else if(mainCollider != null)
                {
                    // Remove main collider
                    dynamicBody.RemoveShape(mainCollider.physicsShape);
                }

                // Remove dynamic
                Physics.physicsWorld.Remove(dynamicBody);
                dynamicBody = null;
            }
        }

        internal void AttachCollider(Collider collider)
        {
            if (mainCollider != null)
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
            {
                // Set main collider
                mainCollider = collider;
            }

            // Attach to body
            if (dynamicBody != null)
                dynamicBody.AddShape(collider.physicsShape);
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

            // Remove collider
            if (dynamicBody != null)
                dynamicBody.RemoveShape(collider.physicsShape);
        }

        internal void RebuildBody()
        {
            // Update transform
            PhysicsSimulation.ApplyTransform(dynamicBody, Transform);

            // Update momentum
            PhysicsSimulation.ApplyMomentum(dynamicBody, this);

            // Check for kinematic
            if (isKinematic == false)
            {
                dynamicBody.SetMassInertia(mass);
                dynamicBody.Damping = (linearDamping, angularDamping);
            }
            else
            {
                dynamicBody.SetMassInertia(JMatrix.Zero, 1e-3f, true);
                dynamicBody.Damping = (0f, 0f);
            }
        }
    }
}
