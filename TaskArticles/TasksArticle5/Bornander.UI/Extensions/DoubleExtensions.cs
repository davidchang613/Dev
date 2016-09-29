using System;

namespace Bornander.UI.Extensions
{
    public static class DoubleExtensions
    {
        private const double PiOverOneEighty = Math.PI / 180.0;

        public static double ToRadians(this double degrees)
        {
            return degrees * PiOverOneEighty;
        }
    }
}
