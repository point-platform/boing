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
using System.Collections.Generic;

namespace Boing
{
    /// <summary>
    /// An axis-aligned rectangular area in a two dimensional coordinate system.
    /// </summary>
    public struct Rectangle2f
    {
        /// <summary>
        /// The minimum point in the rectangle, which is also the top left corner.
        /// </summary>
        public Vector2f Min { get; }

        /// <summary>
        /// The maximum point in the rectangle, which is also the bottom right corner.
        /// </summary>
        public Vector2f Max { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="Rectangle2f"/> from the pair of its minimum and maximum points.
        /// </summary>
        /// <param name="min">The minimum point, which is also the top left corner.</param>
        /// <param name="max">The maximum point, which is also the bottom right corner.</param>
        /// <exception cref="ArgumentException"></exception>
        public Rectangle2f(Vector2f min, Vector2f max)
        {
            if (min.X > max.X)
                throw new ArgumentException("min.X is greater than max.X");
            if (min.Y > max.Y)
                throw new ArgumentException("min.Y is greater than max.Y");

            Min = min;
            Max = max;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Rectangle2f"/> from its minimum point, and a width and height.
        /// </summary>
        /// <param name="x">The x value of the minimum point.</param>
        /// <param name="y">The y value of the minimum point.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public Rectangle2f(float x, float y, float width, float height)
            : this(new Vector2f(x, y), new Vector2f(x + width, y + height))
        {}

        /// <summary>Gets the x value of the left edge.</summary>
        public float Left => Min.X;
        /// <summary>Gets the x value of the right edge.</summary>
        public float Right => Max.X;
        /// <summary>Gets the y value of the top edge.</summary>
        public float Top => Min.Y;
        /// <summary>Gets the y value of the bottom edge.</summary>
        public float Bottom => Max.Y;

        /// <summary>Gets the width of the rectangle.</summary>
        public float Width => Right - Left;
        /// <summary>Gets the height of the rectangle.</summary>
        public float Height => Bottom - Top;

        /// <summary> Gets the position of the top left corner. </summary>
        public Vector2f TopLeft => Min;
        /// <summary> Gets the position of the top right corner. </summary>
        public Vector2f TopRight => new Vector2f(Max.X, Min.Y);
        /// <summary> Gets the position of the bottom left corner. </summary>
        public Vector2f BottomLeft => new Vector2f(Min.X, Max.Y);
        /// <summary> Gets the position of the bottom right corner. </summary>
        public Vector2f BottomRight => Max;

        /// <summary>
        /// Enumerates the four edges of the rectangle.
        /// </summary>
        /// <remarks>
        /// Edges are produce in clockwise order: top, right, bottom, left.
        /// </remarks>
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
                if (edge.TryIntersect(line, out Vector2f point, out float edgeT, out float lineT))
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