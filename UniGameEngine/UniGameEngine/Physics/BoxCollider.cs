using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public unsafe sealed class BoxCollider : Collider
    {
        // Private
        private Vector3 extents = new Vector3(1f);

        // Internal
        internal Box physicsBox = default;

        // Properties
        public Vector3 Extents
        {
            get { return extents; }
            set
            {
                extents = value;
                RebuildCollider();
            }
        }

        public override BoundingBox Bounds
        {
            get
            {
                return new BoundingBox(
                    Transform.WorldPosition - (extents / 2),
                    Transform.WorldPosition + (extents / 2));
            }
        }

        // Constructor
        public BoxCollider()
        {
            this.physicsBox = new Box(extents.X, extents.Y, extents.Z);
        }

        // Methods
        internal override TypedIndex CreatePhysicsShape()
        {
            return Physics.simulation.Shapes.Add(physicsBox);
        }

        internal override BodyInertia GetBodyInertia(float mass)
        {
            return physicsBox.ComputeInertia(mass);
        }
    }
}
