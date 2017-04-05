#region License

//  Copyright 2015-2017 Drew Noakes, Krzysztof Dul
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
    public struct Vector2f
    {
        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public readonly float X;
        public readonly float Y;

        public float Norm() => (float)Math.Sqrt(X*X + Y*Y);

        public Vector2f Normalized() => this/Norm();

        public Vector2f Normalized(out float norm)
        {
            norm = Norm();
            return this/norm;
        }

        public bool HasNaN => float.IsNaN(X) || float.IsNaN(Y);

        public bool HasInfinity => float.IsInfinity(X) || float.IsInfinity(Y);

        #region Factories

        private static readonly Random _random = new Random();

        public static Vector2f Random(float valueRange = 1.0f)
        {
            return new Vector2f(
                valueRange*((float)_random.NextDouble() - 0.5f),
                valueRange*((float)_random.NextDouble() - 0.5f));
        }

        public static Vector2f Zero => default(Vector2f);

        #endregion

        #region Equality / Hashing

        public bool Equals(Vector2f other) => X.Equals(other.X) && Y.Equals(other.Y);

        public override bool Equals(object obj) => obj is Vector2f v && Equals(v);

        public override int GetHashCode() => unchecked((X.GetHashCode()*397) ^ Y.GetHashCode());

        public static bool operator ==(Vector2f a, Vector2f b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2f a, Vector2f b) => a.X != b.X || a.Y != b.Y;

        #endregion

        #region Arithmetic operators

        public static Vector2f operator +(Vector2f a, Vector2f b) => new Vector2f(a.X + b.X, a.Y + b.Y);
        public static Vector2f operator -(Vector2f a, Vector2f b) => new Vector2f(a.X - b.X, a.Y - b.Y);
        public static Vector2f operator *(Vector2f a, float b) => new Vector2f(a.X*b, a.Y*b);
        public static Vector2f operator *(float a, Vector2f b) => new Vector2f(b.X*a, b.Y*a);
        public static Vector2f operator /(Vector2f a, float b) => b == 0.0f ? Zero : new Vector2f(a.X/b, a.Y/b);
        public static Vector2f operator -(Vector2f v) => new Vector2f(-v.X, -v.Y);

        #endregion

        public override string ToString() => $"X: {X}, Y: {Y}";
    }
}