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

        public Vector3 LocalEulerAngle
        {
            get { return ToEuler(localRotation); }
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
            Vector3 localEulerAngles = LocalEulerAngle;

            // Rotation
            Matrix rotation = Matrix.CreateFromYawPitchRoll(localEulerAngles.Y, localEulerAngles.X, localEulerAngles.Z);

            // Scale
            Matrix scale = Matrix.CreateScale(localScale);

            // Create local to world TRS
            localToWorldMatrix = translate * rotation * scale;

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

        internal static Vector3 ToEuler(in Quaternion q)
        {
            Vector3 euler = default;

            // if the input quaternion is normalized, this is exactly one. Otherwise, this acts as a correction factor for the quaternion's not-normalizedness
            float unit = (q.X * q.X) + (q.Y * q.Y) + (q.Z * q.Z) + (q.W * q.W);

            // this will have a magnitude of 0.5 or greater if and only if this is a singularity case
            float test = q.X * q.W - q.Y * q.Z;

            if (test > 0.4995f * unit) // singularity at north pole
            {
                euler.X = MathHelper.Pi / 2;
                euler.Y = 2f * MathF.Atan2(q.Y, q.X);
                euler.Z = 0;
            }
            else if (test < -0.4995f * unit) // singularity at south pole
            {
                euler.X = -MathHelper.Pi / 2;
                euler.Y = -2f * MathF.Atan2(q.Y, q.X);
                euler.Z = 0;
            }
            else // no singularity - this is the majority of cases
            {
                euler.X = MathF.Asin(2f * (q.W * q.X - q.Y * q.Z));
                euler.Y = MathF.Atan2(2f * q.W * q.Y + 2f * q.Z * q.X, 1 - 2f * (q.X * q.X + q.Y * q.Y));
                euler.Z = MathF.Atan2(2f * q.W * q.Z + 2f * q.X * q.Y, 1 - 2f * (q.Z * q.Z + q.X * q.X));
            }

            // all the math so far has been done in radians. Before returning, we convert to degrees...
            euler.X = MathHelper.ToDegrees(euler.X);
            euler.Y = MathHelper.ToDegrees(euler.Y);
            euler.Z = MathHelper.ToDegrees(euler.Z);

            //...and then ensure the degree values are between 0 and 360
            euler.X %= 360;
            euler.Y %= 360;
            euler.Z %= 360;

            return euler;
        }
    }
}
