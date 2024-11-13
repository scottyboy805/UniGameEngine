using Jitter2.Collision.Shapes;
using Jitter2.LinearMath;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace UniGameEngine.Physics
{
    [DataContract]
    public struct PhysicsTriangle
    {
        // Public
        [DataMember]
        public Vector3 Point0;
        [DataMember]
        public Vector3 Point1;
        [DataMember]
        public Vector3 Point2;
    }

    [DataContract]
    public sealed class PhysicsMesh : GameElement
    {
        // Private
        [DataMember(Name = "Triangles")]
        private PhysicsTriangle[] triangles = null;

        // Internal
        internal TriangleMesh physicsMesh = null;

        // Properties
        public IReadOnlyList<PhysicsTriangle> Triangles
        {
            get { return triangles; }
        }

        // Constructor
        public PhysicsMesh(IEnumerable<PhysicsTriangle> points)
        {
            // Update triangles
            triangles = points.ToArray();

            // Initialize mesh
            InitializeMesh();
        }

        // Methods
        protected internal override void OnLoaded()
        {
            // Create physics mesh
            if (physicsMesh == null)
                InitializeMesh();
        }

        private void InitializeMesh()
        {
            // Create the interop array
            List<JTriangle> triMesh = new List<JTriangle>(triangles.Length);

            // Copy elements
            for(int i = 0; i < triangles.Length; i++)
                triMesh.Add(Unsafe.As<PhysicsTriangle, JTriangle>(ref triangles[i]));

            // Create physics mesh
            physicsMesh = new TriangleMesh(triMesh);
        }
    }
}
