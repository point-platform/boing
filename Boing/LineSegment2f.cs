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

namespace Boing
{
    /// <summary>
    /// A line segment in a two dimensional coordinate system.
    /// </summary>
    public struct LineSegment2f
    {
        /// <summary>Gets the starting point of the line segment.</summary>
        public Vector2f From { get; }
        /// <summary>Gets the ending point of the line segment.</summary>
        public Vector2f To { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="LineSegment2f"/>.
        /// </summary>
        /// <param name="from">The starting point of the line segment</param>
        /// <param name="to">The ending point of the line segment</param>
        public LineSegment2f(Vector2f from, Vector2f to)
        {
            From = from;
            To = to;
        }

        /// <summary>Gets a vector representing the difference of the line segment's endpoints.</summary>
        /// <remarks>Computed as <see cref="To"/> minus <see cref="From"/>.</remarks>
        public Vector2f Delta => new Vector2f(To.X - From.X, To.Y - From.Y);

        /// <summary>
        /// Attempt to intersect two line segments.
        /// </summary>
        /// <remarks>
        /// Even if the line segments do not intersect, <paramref name="t"/> and <paramref name="u"/> will be set.
        /// If the lines are parallel, <paramref name="t"/> and <paramref name="u"/> are set to <see cref="float.NaN"/>.
        /// </remarks>
        /// <param name="other">The line to attempt intersection of this line with.</param>
        /// <param name="intersectionPoint">The point of intersection if within the line segments, or empty..</param>
        /// <param name="t">The distance along this line at which intersection would occur, or NaN if lines are collinear/parallel.</param>
        /// <param name="u">The distance along the other line at which intersection would occur, or NaN if lines are collinear/parallel.</param>
        /// <returns><c>true</c> if the line segments intersect, otherwise <c>false</c>.</returns>
        public bool TryIntersect(LineSegment2f other, out Vector2f intersectionPoint, out float t, out float u)
        {
            float Fake2DCross(Vector2f a, Vector2f b) => a.X * b.Y - a.Y * b.X;

            var p = From;
            var q = other.From;
            var r = Delta;
            var s = other.Delta;

            // t = (q − p) × s / (r × s)
            // u = (q − p) × r / (r × s)

            var denom = Fake2DCross(r, s);

            if (denom == 0)
            {
                // lines are collinear or parallel
                t = float.NaN;
                u = float.NaN;
                intersectionPoint = default(Vector2f);
                return false;
            }

            var tNumer = Fake2DCross(q - p, s);
            var uNumer = Fake2DCross(q - p, r);

            t = tNumer / denom;
            u = uNumer / denom;

            if (t < 0 || t > 1 || u < 0 || u > 1)
            {
                // line segments do not intersect within their ranges
                intersectionPoint = default(Vector2f);
                return false;
            }

            intersectionPoint = p + r * t;
            return true;
        }
    }
}