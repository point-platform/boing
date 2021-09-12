using System;
using System.Numerics;

namespace Boing
{
    /// <summary>
    /// An axis-aligned rectangular area in a two dimensional coordinate system.
    /// </summary>
    public readonly struct Rectangle3f
    {
        /// <summary>
        /// The minimum point in the rectangle, which is also the top left corner.
        /// </summary>
        public Vector3 Min { get; }

        /// <summary>
        /// The maximum point in the rectangle, which is also the bottom right corner.
        /// </summary>
        public Vector3 Max { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="Rectangle3f"/> from the pair of its minimum and maximum points.
        /// </summary>
        /// <param name="min">The minimum point, which is also the top left corner.</param>
        /// <param name="max">The maximum point, which is also the bottom right corner.</param>
        /// <exception cref="ArgumentException"></exception>
        public Rectangle3f(Vector3 min, Vector3 max)
        {
            if (min.X > max.X)
                throw new ArgumentException("min.X is greater than max.X");
            if (min.Y > max.Y)
                throw new ArgumentException("min.Y is greater than max.Y");
            if (min.Z > max.Z)
                throw new ArgumentException("min.Z is greater than max.Z");

            Min = min;
            Max = max;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Rectangle3f"/> from its minimum point, and a width and height.
        /// </summary>
        /// <param name="x">The x value of the minimum point.</param>
        /// <param name="y">The y value of the minimum point.</param>
        /// <param name="z">The z value of the minimum point.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="depth">The depth of the rectangle.</param>
        public Rectangle3f(float x, float y, float z, float width, float height, float depth)
            : this(new Vector3(x, y, z), new Vector3(x + width, y + height, z + depth))
        {}
    }
}