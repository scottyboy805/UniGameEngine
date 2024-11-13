using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Xna.Framework
{
    public static class MatrixExtensions
    {
        // Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Decompose(in this Matrix matrix, out Vector2 scale, out float zRotation, out Vector2 translation)
        {
            translation.X = matrix.M41;
            translation.Y = matrix.M42;
            float num = ((Math.Sign(matrix.M11 * matrix.M12 * matrix.M13 * matrix.M14) >= 0) ? 1 : (-1));
            float num2 = ((Math.Sign(matrix.M21 * matrix.M22 * matrix.M23 * matrix.M24) >= 0) ? 1 : (-1));
            float num3 = ((Math.Sign(matrix.M31 * matrix.M32 * matrix.M33 * matrix.M34) >= 0) ? 1 : (-1));
            scale.X = num * MathF.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13);
            scale.Y = num2 * MathF.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23);
            float scaZ = num3 * MathF.Sqrt(matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33);
            if ((double)scale.X == 0.0 || (double)scale.Y == 0.0)
            {
                zRotation = 0f;
                return false;
            }

            Matrix rMatrix = new Matrix(matrix.M11 / scale.X, matrix.M12 / scale.X, matrix.M13 / scale.X, 0f, matrix.M21 / scale.Y, matrix.M22 / scale.Y, matrix.M23 / scale.Y, 0f, matrix.M31 / scaZ, matrix.M32 / scaZ, matrix.M33 / scaZ, 0f, 0f, 0f, 0f, 1f);
            zRotation = MathHelper.ToRadians( Quaternion.CreateFromRotationMatrix(rMatrix).ToEuler().Z);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DecomposeScale(in this Matrix matrix, out Vector2 scale)
        {
            float xModifier = ((Math.Sign(matrix.M11 * matrix.M12 * matrix.M13 * matrix.M14) >= 0) ? 1 : (-1));
            float yModifier = ((Math.Sign(matrix.M21 * matrix.M22 * matrix.M23 * matrix.M24) >= 0) ? 1 : (-1));
            
            // Create scale
            scale.X = xModifier * MathF.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13);
            scale.Y = yModifier * MathF.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23);
            
            // Check for zero scale
            if ((double)scale.X == 0.0 || (double)scale.Y == 0.0)
                return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DecomposeScale(in this Matrix matrix, out Vector3 scale)
        {
            float xModifier = ((Math.Sign(matrix.M11 * matrix.M12 * matrix.M13 * matrix.M14) >= 0) ? 1 : (-1));
            float yModifier = ((Math.Sign(matrix.M21 * matrix.M22 * matrix.M23 * matrix.M24) >= 0) ? 1 : (-1));
            float zModifier = ((Math.Sign(matrix.M31 * matrix.M32 * matrix.M33 * matrix.M34) >= 0) ? 1 : (-1));

            // Create scale
            scale.X = xModifier * MathF.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13);
            scale.Y = yModifier * MathF.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23);
            scale.Z = zModifier * MathF.Sqrt(matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33);

            // Check for zero scale
            if ((double)scale.X == 0.0 || (double)scale.Y == 0.0 || (double)scale.Z == 0.0)
                return false;

            return true;
        }
    }
}
