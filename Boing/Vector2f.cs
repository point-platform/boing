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

        public bool Equals(Vector2f other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Vector2f && Equals((Vector2f)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Vector2f a, Vector2f b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2f a, Vector2f b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        #endregion

        #region Arithmetic operators

        public static Vector2f operator +(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2f operator -(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2f operator *(Vector2f a, float b)
        {
            return new Vector2f(a.X*b, a.Y*b);
        }

        public static Vector2f operator *(float a, Vector2f b)
        {
            return new Vector2f(b.X*a, b.Y*a);
        }

        public static Vector2f operator /(Vector2f a, float b)
        {
            return b == 0.0f
                ? Zero
                : new Vector2f(a.X/b, a.Y/b);
        }

        #endregion
    }
}