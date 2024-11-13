using System.Runtime.CompilerServices;

namespace Microsoft.Xna.Framework
{
    public static class Vector3Extensions
    {
        // Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToVector2(in this Vector3 vector)
        {
            Vector2 result;
            result.X = vector.X;
            result.Y = vector.Y;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point ToPoint(in this Vector3 vector)
        {
            Point result;
            result.X = (int)vector.X;
            result.Y = (int)vector.Y;
            return result;
        }
    }
}
