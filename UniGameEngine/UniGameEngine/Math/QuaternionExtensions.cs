using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Xna.Framework
{
    public static class QuaternionExtensions
    {
        // Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ToEuler(in this Quaternion quaternion)
        {
            Vector3 euler = default;

            // if the input quaternion is normalized, this is exactly one. Otherwise, this acts as a correction factor for the quaternion's not-normalizedness
            float unit = (quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.W * quaternion.W);

            // this will have a magnitude of 0.5 or greater if and only if this is a singularity case
            float test = quaternion.X * quaternion.W - quaternion.Y * quaternion.Z;

            if (test > 0.4995f * unit) // singularity at north pole
            {
                euler.X = MathHelper.Pi / 2;
                euler.Y = 2f * MathF.Atan2(quaternion.Y, quaternion.X);
                euler.Z = 0;
            }
            else if (test < -0.4995f * unit) // singularity at south pole
            {
                euler.X = -MathHelper.Pi / 2;
                euler.Y = -2f * MathF.Atan2(quaternion.Y, quaternion.X);
                euler.Z = 0;
            }
            else // no singularity - this is the majority of cases
            {
                euler.X = MathF.Asin(2f * (quaternion.W * quaternion.X - quaternion.Y * quaternion.Z));
                euler.Y = MathF.Atan2(2f * quaternion.W * quaternion.Y + 2f * quaternion.Z * quaternion.X, 1 - 2f * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y));
                euler.Z = MathF.Atan2(2f * quaternion.W * quaternion.Z + 2f * quaternion.X * quaternion.Y, 1 - 2f * (quaternion.Z * quaternion.Z + quaternion.X * quaternion.X));
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
