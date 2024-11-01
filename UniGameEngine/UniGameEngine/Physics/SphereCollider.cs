using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public unsafe sealed class SphereCollider : Collider
    {
        // Private
        private float radius = 0.5f;

        // Internal
        internal Sphere physicsSphere = default;

        // Properties
        [DataMember]
        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                RebuildCollider();
            }
        }

        public override BoundingBox Bounds
        {
            get 
            {
                return new BoundingBox(
                    Transform.WorldPosition - new Vector3(radius), 
                    Transform.WorldPosition + new Vector3(radius)); 
            }
        }

        // Constructor
        public SphereCollider()
        {
            this.physicsSphere = new Sphere(radius);
        }

        // Methods
        internal override TypedIndex CreatePhysicsShape()
        {
            return Physics.simulation.Shapes.Add(physicsSphere);
        }

        internal override BodyInertia GetBodyInertia(float mass)
        {
            return physicsSphere.ComputeInertia(mass);
        }    
    }
}
