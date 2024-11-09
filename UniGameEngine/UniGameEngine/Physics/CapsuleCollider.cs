using Jitter2.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public sealed class CapsuleCollider : Collider
    {
        // Private
        private float radius = 0.5f;
        private float length = 1f;

        // Internal
        internal CapsuleShape physicsCapsule = null;

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

        [DataMember]
        public float Length
        {
            get { return length; }
            set
            {
                length = value;
                RebuildCollider();
            }
        }

        // Constructor
        public CapsuleCollider()
        {
            this.physicsCapsule = new CapsuleShape(radius, length);
            this.physicsShape = physicsCapsule;
        }

        // Methods
        internal override void RebuildCollider()
        {
            base.RebuildCollider();

            // Get scale
            Vector3 scale = Transform.LocalScale;

            // Create final size
            float scaledRadius = MathF.Max(MathF.Min(scale.X, scale.Y), scale.Z) * radius;
            float scaledLength = MathF.Max(MathF.Min(scale.X, scale.Y), scale.Z) * length;

            // Update size
            physicsCapsule.Radius = scaledRadius;
            physicsCapsule.Length = scaledLength;
        }
    }
}
