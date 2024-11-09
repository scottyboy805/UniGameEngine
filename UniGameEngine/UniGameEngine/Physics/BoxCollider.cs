using Jitter2.Collision.Shapes;
using Jitter2.LinearMath;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public sealed class BoxCollider : Collider
    {
        // Private
        private Vector3 extents = new Vector3(1f);

        // Internal
        internal BoxShape physicsBox = null;

        // Properties
        [DataMember]
        public Vector3 Extents
        {
            get { return extents; }
            set
            {
                extents = value;
                RebuildCollider();
            }
        }

        // Constructor
        public BoxCollider()
        {
            this.physicsBox = new BoxShape(Unsafe.As<Vector3, JVector>(ref extents));
            this.physicsShape = physicsBox;
        }

        // Methods
        internal override void RebuildCollider()
        {
            base.RebuildCollider();

            // Create final size
            Vector3 size = extents * Transform.LocalScale;

            // Update size
            physicsBox.Size = Unsafe.As<Vector3, JVector>(ref size);
        }
    }
}
