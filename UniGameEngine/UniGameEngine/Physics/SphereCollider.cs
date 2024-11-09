using Jitter2.Collision.Shapes;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public sealed class SphereCollider : Collider
    {
        // Private
        private float radius = 0.5f;

        // Internal
        internal SphereShape physicsSphere = null;

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

        // Constructor
        public SphereCollider()
        {
            this.physicsSphere = new SphereShape(radius);
            this.physicsShape = physicsSphere;
        }

        // Methods
        internal override void RebuildCollider()
        {
            // Call base
            base.RebuildCollider();

            // Get scale
            Vector3 scale = Transform.LocalScale;

            // Create final size
            float scaledRadius = MathF.Max(MathF.Max(scale.X, scale.Y), scale.Z) * radius;

            // Update radius
            physicsSphere.Radius = scaledRadius;
        }
    }
}
