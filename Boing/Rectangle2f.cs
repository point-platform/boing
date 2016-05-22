using System;
using System.Collections.Generic;

namespace Boing
{
    public struct Rectangle2f
    {
        public Vector2f Min { get; }
        public Vector2f Max { get; }

        public Rectangle2f(Vector2f min, Vector2f max)
        {
            if (min.X > max.X)
                throw new ArgumentException("min.X is greater than max.X");
            if (min.X > max.X)
                throw new ArgumentException("min.Y is greater than max.Y");

            Min = min;
            Max = max;
        }

        public Rectangle2f(float x, float y, float width, float height)
        {
            Min = new Vector2f(x, y);
            Max = new Vector2f(x + width, y + height);
        }

        public Vector2f TopLeft => Min;
        public Vector2f TopRight => new Vector2f(Min.Y, Max.X);
        public Vector2f BottomLeft => new Vector2f(Max.Y, Min.X);
        public Vector2f BottomRight => Max;

        public IEnumerable<LineSegment2f> Edges
        {
            get
            {
                yield return new LineSegment2f(TopLeft, TopRight);
                yield return new LineSegment2f(TopRight, BottomRight);
                yield return new LineSegment2f(BottomLeft, BottomRight);
                yield return new LineSegment2f(TopLeft, BottomLeft);
            }
        }

        /// <summary>
        /// Attempt to intersect a line segment with this rectangle.
        /// </summary>
        /// <param name="line">The line to intersect with this rectangle.</param>
        /// <param name="intersectionPoint">The point of intersection.</param>
        /// <param name="t">the distance along this line at which intersection would occur, or zero if no intersection occurs.</param>
        /// <returns><c>true</c> if an intersection exists, otherwise <c>false</c>.</returns>
        public bool TryIntersect(LineSegment2f line, out Vector2f intersectionPoint, out float t)
        {
            var minT = float.MaxValue;
            var found = false;

            intersectionPoint = default(Vector2f);

            foreach (var edge in Edges)
            {
                float edgeU;
                float edgeT;
                Vector2f point;
                if (edge.TryIntersect(line, out point, out edgeT, out edgeU))
                {
                    if (edgeT > minT)
                        continue;
                    found = true;
                    minT = edgeT;
                    intersectionPoint = point;
                }
            }

            if (found)
            {
                t = minT;
                return true;
            }

            t = 0;
            return false;
        }
    }
}