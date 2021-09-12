using System;
using System.Diagnostics;
using System.Numerics;

namespace Boing
{
    /// <summary>
    /// A force that models a linear spring between two point masses according to Hooke's law.
    /// </summary>
    /// <remarks>
    /// Unlike many other <see cref="IForce{TVec}"/> implementations, a single <see cref="Spring3"/>
    /// instance does not apply to all point masses in the simulation. It applies to two, only.
    /// <para />
    /// The spring is defined by its two point masses, a length and a spring constant.
    /// At each time step, the distance between the point masses is computed and compared
    /// against the spring's length.
    /// <para />
    /// If the distance matches the length, no force is applied.
    /// When the distance diverges from the spring's length, the spring undergoes compression
    /// or elongation and exerts a restorative force against both point masses.
    /// <para />
    /// Note that if one point mass is pinned, the force applies doubly to the unpinned end.
    /// If both point masses are pinned, the spring has no effect.
    /// <para />
    /// See https://en.wikipedia.org/wiki/Hooke%27s_law for more information.
    /// </remarks>
    public sealed class Spring3 : IForce<Vector3>
    {
        /// <summary>
        /// Gets one of the two point masses connected by this spring.
        /// </summary>
        /// <remarks>
        /// The orientation of the spring has no effect. <see cref="Source"/> and <see cref="Target"/>
        /// could be swapped and the outcome would be identical.
        /// </remarks>
        public PointMass3 Source { get; }

        /// <summary>
        /// Gets one of the two point masses connected by this spring.
        /// </summary>
        /// <remarks>
        /// The orientation of the spring has no effect. <see cref="Source"/> and <see cref="Target"/>
        /// could be swapped and the outcome would be identical.
        /// </remarks>
        public PointMass3 Target { get; }

        /// <summary>
        /// Gets and sets the resting length of the spring.
        /// </summary>
        public float Length { get; set; }

        /// <summary>
        /// Gets and sets the spring constant.
        /// </summary>
        public float K { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="Spring3"/>.
        /// </summary>
        /// <param name="source">One of the two point masses.</param>
        /// <param name="target">One of the two point masses.</param>
        /// <param name="length">The resting length of the spring. The default value is 100 units.</param>
        /// <param name="k">The string constant. The default value is 80.</param>
        public Spring3(PointMass3 source, PointMass3 target, float length = 100.0f, float k = 80.0f)
        {
            Source = source;
            Target = target;
            Length = length;
            K = k;
        }

        /// <summary>
        /// Gets a 3D line segment whose start and end points match those of
        /// the <see cref="Source"/> and <see cref="Target"/> point mass positions.
        /// </summary>
        public LineSegment3f LineSegment => new(Source.Position, Target.Position);

        /// <summary>
        /// Gets the 3D axis-aligned bounding box that encloses the <see cref="Source"/> and
        /// <see cref="Target"/> point mass positions.
        /// </summary>
        public Rectangle3f Bounds => new(
            Math.Min(Source.Position.X, Target.Position.X),
            Math.Min(Source.Position.Y, Target.Position.Y),
            Math.Min(Source.Position.Z, Target.Position.Z),
            Math.Abs(Source.Position.X - Target.Position.X),
            Math.Abs(Source.Position.Y - Target.Position.Y),
            Math.Abs(Source.Position.Z - Target.Position.Z));

        /// <inheritdoc />
        void IForce<Vector3>.ApplyTo(Simulation<Vector3> simulation)
        {
            var source = Source;
            var target = Target;

            var delta = target.Position - source.Position;
            var deltaNorm = delta.Length();
            var direction = delta / deltaNorm;
            var displacement = Length - deltaNorm;

            Debug.Assert(!float.IsNaN(displacement), "!float.IsNaN(displacement)");

            if (!source.IsPinned && !target.IsPinned)
            {
                source.ApplyForce(direction*(K*displacement*-0.5f));
                target.ApplyForce(direction*(K*displacement*0.5f));
            }
            else if (source.IsPinned && !target.IsPinned)
            {
                target.ApplyForce(direction*(K*displacement));
            }
            else if (!source.IsPinned && target.IsPinned)
            {
                source.ApplyForce(direction*(K*-displacement));
            }
        }
    }
}