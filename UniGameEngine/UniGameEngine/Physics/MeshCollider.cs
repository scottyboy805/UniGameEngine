using Jitter2.Collision.Shapes;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public sealed class MeshCollider : Collider
    {
        // Private
        private PhysicsMesh mesh = null;
        private bool isConvex = false;

        // Internal
        internal RigidBodyShape meshShape = null;

        // Properties
        [DataMember]
        public PhysicsMesh Mesh
        {
            get { return mesh; }
            set
            {
                mesh = value;
                RebuildCollider();
            }
        }

        [DataMember]
        public bool IsConvex
        {
            get { return isConvex; }
            set
            {
                isConvex = value;
                RebuildCollider();
            }
        }
    }
}
