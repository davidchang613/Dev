using System;
using System.Windows;

namespace Bornander.UI.Extensions
{
    public static class VectorExtensions
    {
        public static Vector Normalize(Vector v)
        {
            Vector result = v;
            result.Normalize();
            return result;
        }

        public static Vector Project(this Vector a, Vector b)
        {
            double angle = Vector.AngleBetween(a, b).ToRadians();
            return Normalize(b) * a.Length * Math.Cos(angle); 
        }
    }
}
