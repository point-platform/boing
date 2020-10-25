#region License

//  Copyright 2015-2020 Drew Noakes, Krzysztof Dul
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

using System;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Boing
{
    /// <summary>
    /// A point in a two dimensional coordinate system.
    /// </summary>
    public readonly struct Vector2f
    {
        /// <summary>
        /// Initialises a <see cref="Vector2f"/>.
        /// </summary>
        /// <param name="x">The x coordinate value.</param>
        /// <param name="y">The y coordinate value.</param>
        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Gets the x coordinate value.</summary>
        public readonly float X;
        /// <summary>Gets the y coordinate value.</summary>
        public readonly float Y;

        /// <summary>Gets the norm (length) of the vector.</summary>
        public float Norm() => (float)Math.Sqrt(X*X + Y*Y);

        /// <summary>
        /// Returns a normalized version of this vector.
        /// </summary>
        /// <remarks>
        /// The normalized vector has length one (it is a unit vector).
        /// </remarks>
        /// <returns>The normalized version of this vector.</returns>
        public Vector2f Normalized() => this/Norm();

        /// <summary>
        /// Returns a normalized version of this vector.
        /// </summary>
        /// <remarks>
        /// The normalized vector has length one (it is a unit vector).
        /// This overload is useful if you need to know the length of a vector,
        /// and need its normalized form, in which case it is both convenient and efficient.
        /// </remarks>
        /// <param name="norm">The length of the vector before normalization.</param>
        /// <returns>The normalized version of this vector.</returns>
        public Vector2f Normalized(out float norm)
        {
            norm = Norm();
            return this/norm;
        }

        /// <summary>
        /// Gets whether either the <see cref="X"/> or <see cref="Y"/> values are <see cref="float.NaN"/>.
        /// </summary>
        public bool HasNaN => float.IsNaN(X) || float.IsNaN(Y);

        /// Gets whether either the <see cref="X"/> or <see cref="Y"/> values are infinite.
        public bool HasInfinity => float.IsInfinity(X) || float.IsInfinity(Y);

        #region Factories

        private static readonly Random _random = new Random();

        /// <summary>
        /// Produces random points with <see cref="X"/> and <see cref="Y"/> values distributed evenly
        /// over a square of side length <paramref name="valueRange"/> that is centered at the origin.
        /// </summary>
        /// <param name="valueRange">The width and height of the square over which random points are distributed.</param>
        /// <returns>The next random point in the specified range.</returns>
        public static Vector2f Random(float valueRange = 1.0f)
        {
            return new Vector2f(
                valueRange*((float)_random.NextDouble() - 0.5f),
                valueRange*((float)_random.NextDouble() - 0.5f));
        }

        /// <summary>
        /// Gets a vector with both <see cref="X"/> and <see cref="Y"/> equalling zero.
        /// </summary>
        public static Vector2f Zero => default(Vector2f);

        #endregion

        #region Equality / Hashing

        /// <summary>
        /// Indicates whether this instance equals <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The vector to compare against.</param>
        /// <returns><c>true</c> if the vectors are equal, otherwise <c>false</c>.</returns>
        public bool Equals(Vector2f other) => X.Equals(other.X) && Y.Equals(other.Y);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Vector2f v && Equals(v);

        /// <inheritdoc />
        public override int GetHashCode() => unchecked((X.GetHashCode()*397) ^ Y.GetHashCode());

        #endregion

        #region Operators

#pragma warning disable 1591
        public static bool operator ==(Vector2f a, Vector2f b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2f a, Vector2f b) => a.X != b.X || a.Y != b.Y;

        public static Vector2f operator +(Vector2f a, Vector2f b) => new Vector2f(a.X + b.X, a.Y + b.Y);
        public static Vector2f operator -(Vector2f a, Vector2f b) => new Vector2f(a.X - b.X, a.Y - b.Y);
        public static Vector2f operator *(Vector2f a, float b) => new Vector2f(a.X*b, a.Y*b);
        public static Vector2f operator *(float a, Vector2f b) => new Vector2f(b.X*a, b.Y*a);
        public static Vector2f operator /(Vector2f a, float b) => b == 0.0f ? Zero : new Vector2f(a.X/b, a.Y/b);
        public static Vector2f operator -(Vector2f v) => new Vector2f(-v.X, -v.Y);
#pragma warning restore 1591

        #endregion

        /// <inheritdoc />
        public override string ToString() => $"X: {X}, Y: {Y}";
    }
}