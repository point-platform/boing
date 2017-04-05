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
            if (min.Y > max.Y)
                throw new ArgumentException("min.Y is greater than max.Y");

            Min = min;
            Max = max;
        }

        public Rectangle2f(float x, float y, float width, float height)
            : this(new Vector2f(x, y), new Vector2f(x + width, y + height))
        {}

        public float Left => Min.X;
        public float Right => Max.X;
        public float Top => Min.Y;
        public float Bottom => Max.Y;

        public Vector2f TopLeft => Min;
        public Vector2f TopRight => new Vector2f(Max.X, Min.Y);
        public Vector2f BottomLeft => new Vector2f(Min.X, Max.Y);
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
                float lineT;
                float edgeT;
                Vector2f point;
                if (edge.TryIntersect(line, out point, out edgeT, out lineT))
                {
                    if (lineT > minT)
                        continue;
                    found = true;
                    minT = lineT;
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