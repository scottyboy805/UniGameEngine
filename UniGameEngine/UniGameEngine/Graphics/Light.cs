using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UniGameEngine.Graphics
{
    public sealed class Light : Component
    {
        // Private
        private static readonly List<Light> allActiveLights = new List<Light>();

        [DataMember(Name = "Color")]
        private Color color = Color.White;
        [DataMember(Name = "Specular")]
        private Color specular = Color.White;
        
        // Properties
        public Vector3 Direction
        {
            get { return Transform.Forward; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Color Specular
        {
            get { return specular; }
            set { specular = value; }
        }

        // Properties
        public static IReadOnlyList<Light> AllActiveLights
        {
            get { return allActiveLights; }
        }

        public static Light MainLight
        {
            get { return allActiveLights.Count > 0 ? allActiveLights[0] : null; }
        }

        public static bool HasActiveLights
        {
            get { return allActiveLights.Count > 0; }
        }

        // Methods
        protected override void OnEnable()
        {
            allActiveLights.Add(this);
        }

        protected internal override void OnDestroy()
        {
            allActiveLights.Remove(this);
        }
    }
}
