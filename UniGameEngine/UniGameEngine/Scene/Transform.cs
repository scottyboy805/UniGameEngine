using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UniGameEngine
{
    public sealed class Transform : Component
    {
        // Private
        [DataMember(Name = "LocalPosition")]
        private Vector3 localPosition = Vector3.Zero;
        [DataMember(Name = "LocalRotation")]
        private Quaternion localRotation = Quaternion.Identity;
        [DataMember(Name = "LocalScale")]
        private Vector3 localScale = Vector3.One;

        // Internal
        internal Matrix localToWorldMatrix = Matrix.Identity;
        internal Matrix worldToLocalMatrix = Matrix.Identity;
        internal Transform parent = null;
        internal Transform root = null;
        internal List<Transform> children = null;

        // Properties        
        public Transform Parent
        {
            get { return parent; }
        }

        public Transform Root
        {
            get { return root; }
        }

        public IReadOnlyList<Transform> Children
        {
            get { return children; }
        }

        public bool IsRoot
        {
            get { return parent != null; }
        }

        public int Depth
        {
            get
            {
                int depth = 0;
                Transform current = this;

                // Move up the hierarchy
                while (current.parent != null)
                {
                    current = current.parent;
                    depth++;
                }
                return depth;
            }
        }

        public bool HasChildren
        {
            get { return children != null && children.Count > 0; }
        }

        public Vector3 LocalPosition
        {
            get { return localPosition; }
            set
            {
                localPosition = value;
                RebuildTransform();
            }
        }

        public Quaternion LocalRotation
        {
            get { return localRotation; }
            set
            {
                localRotation = value;
                RebuildTransform();
            }
        }

        public Vector3 LocalEulerAngles
        {
            get { return localRotation.ToEuler(); }
            set
            {
                localRotation = Quaternion.CreateFromYawPitchRoll(value.Y, value.X, value.Z);
                RebuildTransform();
            }
        }

        public Vector3 LocalScale
        {
            get { return localScale; }
            set
            {
                localScale = value;
                RebuildTransform();
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                return parent != null
                    ? Vector3.Transform(localPosition, parent.localToWorldMatrix)
                    : localPosition;
            }
            set
            {
                localPosition = parent != null
                    ? parent.InverseTransformPoint(value)
                    : value;

                // Update matrices
                RebuildTransform();
            }
        }

        public Quaternion WorldRotation
        {
            get
            {
                Quaternion rot = localRotation;
                Transform current = parent;

                while (current != null)
                {
                    rot = current.localRotation * rot;
                    current = current.parent;
                }
                return rot;
            }
            set
            {
                localRotation = parent != null
                    ? Quaternion.Inverse(parent.WorldRotation) * value
                    : value;

                // Update matrices
                RebuildTransform();
            }
        }

        public Vector3 WorldEulerAngles
        {
            get { return WorldRotation.ToEuler(); }
            set
            {
                WorldRotation = Quaternion.CreateFromYawPitchRoll(value.Y, value.X, value.Z);
                RebuildTransform();
            }
        }

        public Vector3 Forward
        {
            get { return localToWorldMatrix.Forward; }
        }

        public Vector3 Up
        {
            get { return localToWorldMatrix.Up; }
        }

        public Vector3 Right
        {
            get { return localToWorldMatrix.Right; }
        }

        public Matrix WorldToLocalMatrix
        {
            get { return worldToLocalMatrix; }
        }

        public Matrix LocalToWorldMatrix
        {
            get { return localToWorldMatrix; }
        }

        // Constructor
        internal Transform(GameObject parent)
            : base(parent)
        {
            RebuildTransform();
        }

        // Methods
        public void Translate(Vector3 translation, Transform relativeTo = null)
        {
            if (relativeTo != null)
            {
                WorldPosition += relativeTo.TransformDirection(translation);
            }
            else
            {
                WorldPosition += translation;
            }
        }

        public void LookAt(Vector3 worldPosition, Vector3 worldUp)
        {
            // Create lookat
            //Matrix4 mat = Matrix4.LookAt(this.WorldPosition, worldPosition, worldUp);
            //LocalRotation = mat.rot
        }

        public Vector3 TransformPoint(Vector3 position)
        {
            return Vector3.Transform(position, localToWorldMatrix);
        }

        public Vector3 InverseTransformPoint(Vector3 position)
        {
            return Vector3.Transform(position, worldToLocalMatrix);
        }

        public Vector3 TransformDirection(Vector3 direction)
        {            
            return Vector3.TransformNormal(direction, localToWorldMatrix);
        }

        public Vector3 InverseTransformDirection(Vector3 direction)
        {
            return Vector3.TransformNormal(direction, worldToLocalMatrix);
        }

        public Quaternion TransformRotation(Quaternion rotation)
        {
            Quaternion rot = rotation;
            Transform current = this;

            while (current != null)
            {
                rot = current.localRotation * rot;
                current = current.parent;
            }
            return rot;
        }

        private void RebuildTransform()
        {
            // Check for static
            if (GameObject.IsStatic == true)
                return;

            // Translation
            Matrix translate = Matrix.CreateTranslation(localPosition);

            // Get euler
            Vector3 localEulerAngles = LocalEulerAngles;

            // Rotation
            Matrix rotation = Matrix.CreateFromYawPitchRoll(localEulerAngles.Y, localEulerAngles.X, localEulerAngles.Z);

            // Scale
            Matrix scale = Matrix.CreateScale(localScale);

            // Create local to world TRS
            localToWorldMatrix = scale * rotation * translate;// * scale;

            // Check for parent
            if (parent != null)
                localToWorldMatrix = localToWorldMatrix * parent.localToWorldMatrix;

            // Rebuild children
            if (children != null && children.Count > 0)
            {
                for (int i = 0; i < children.Count; i++)
                    children[i].RebuildTransform();
            }

            // Create inverse world to local
            worldToLocalMatrix = Matrix.Invert(localToWorldMatrix);
        }
    }
}
