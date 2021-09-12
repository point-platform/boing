using System.Numerics;

namespace Boing
{
    /// <summary>
    /// A line segment in a two dimensional coordinate system.
    /// </summary>
    public readonly struct LineSegment3f
    {
        /// <summary>Gets the starting point of the line segment.</summary>
        public Vector3 From { get; }
        /// <summary>Gets the ending point of the line segment.</summary>
        public Vector3 To { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="LineSegment3f"/>.
        /// </summary>
        /// <param name="from">The starting point of the line segment</param>
        /// <param name="to">The ending point of the line segment</param>
        public LineSegment3f(Vector3 from, Vector3 to)
        {
            From = @from;
            To = to;
        }

        /// <summary>Gets a vector representing the difference of the line segment's endpoints.</summary>
        /// <remarks>Computed as <see cref="To"/> minus <see cref="From"/>.</remarks>
        public Vector3 Delta => To - From;

        // /// <summary>
        // /// Attempt to intersect two line segments.
        // /// </summary>
        // /// <remarks>
        // /// Even if the line segments do not intersect, <paramref name="t"/> and <paramref name="u"/> will be set.
        // /// If the lines are parallel, <paramref name="t"/> and <paramref name="u"/> are set to <see cref="float.NaN"/>.
        // /// </remarks>
        // /// <param name="other">The line to attempt intersection of this line with.</param>
        // /// <param name="intersectionPoint">The point of intersection if within the line segments, or empty..</param>
        // /// <param name="t">The distance along this line at which intersection would occur, or NaN if lines are collinear/parallel.</param>
        // /// <param name="u">The distance along the other line at which intersection would occur, or NaN if lines are collinear/parallel.</param>
        // /// <returns><c>true</c> if the line segments intersect, otherwise <c>false</c>.</returns>
        // public bool TryIntersect(LineSegment3f other, out Vector3 intersectionPoint, out float t, out float u)
        // {
        //     static float Fake2DCross(Vector3 a, Vector3 b) => a.X * b.Y - a.Y * b.X;
        //
        //     var p = From;
        //     var q = other.From;
        //     var r = Delta;
        //     var s = other.Delta;
        //
        //     // t = (q − p) × s / (r × s)
        //     // u = (q − p) × r / (r × s)
        //
        //     var denom = Fake2DCross(r, s);
        //
        //     if (denom == 0)
        //     {
        //         // lines are collinear or parallel
        //         t = float.NaN;
        //         u = float.NaN;
        //         intersectionPoint = default;
        //         return false;
        //     }
        //
        //     var tNumer = Fake2DCross(q - p, s);
        //     var uNumer = Fake2DCross(q - p, r);
        //
        //     t = tNumer / denom;
        //     u = uNumer / denom;
        //
        //     if (t < 0 || t > 1 || u < 0 || u > 1)
        //     {
        //         // line segments do not intersect within their ranges
        //         intersectionPoint = default;
        //         return false;
        //     }
        //
        //     intersectionPoint = p + r * t;
        //     return true;
        // }
    }
}