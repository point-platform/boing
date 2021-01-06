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
using System.Diagnostics;
using System.Numerics;

namespace Boing
{
    /// <summary>
    /// A force that models a linear spring between two point masses according to Hooke's law.
    /// </summary>
    /// <remarks>
    /// Unlike many other <see cref="IForce"/> implementations, a single <see cref="Spring"/>
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
    public sealed class Spring : IForce
    {
        /// <summary>
        /// Gets one of the two point masses connected by this spring.
        /// </summary>
        /// <remarks>
        /// The orientation of the spring has no effect. <see cref="Source"/> and <see cref="Target"/>
        /// could be swapped and the outcome would be identical.
        /// </remarks>
        public PointMass Source { get; }

        /// <summary>
        /// Gets one of the two point masses connected by this spring.
        /// </summary>
        /// <remarks>
        /// The orientation of the spring has no effect. <see cref="Source"/> and <see cref="Target"/>
        /// could be swapped and the outcome would be identical.
        /// </remarks>
        public PointMass Target { get; }

        /// <summary>
        /// Gets and sets the resting length of the spring.
        /// </summary>
        public float Length { get; set; }

        /// <summary>
        /// Gets and sets the spring constant.
        /// </summary>
        public float K { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="Spring"/>.
        /// </summary>
        /// <param name="source">One of the two point masses.</param>
        /// <param name="target">One of the two point masses.</param>
        /// <param name="length">The resting length of the spring. The default value is 100 units.</param>
        /// <param name="k">The string constant. The default value is 80.</param>
        public Spring(PointMass source, PointMass target, float length = 100.0f, float k = 80.0f)
        {
            Source = source;
            Target = target;
            Length = length;
            K = k;
        }

        /// <summary>
        /// Gets a 2D line segment whose start and end points match those of
        /// the <see cref="Source"/> and <see cref="Target"/> point mass positions.
        /// </summary>
        public LineSegment2f LineSegment => new LineSegment2f(Source.Position, Target.Position);

        /// <summary>
        /// Gets the 2D axis-aligned bounding box that encloses the <see cref="Source"/> and
        /// <see cref="Target"/> point mass positions.
        /// </summary>
        public Rectangle2f Bounds => new Rectangle2f(
            Math.Min(Source.Position.X, Target.Position.X),
            Math.Min(Source.Position.Y, Target.Position.Y),
            Math.Abs(Source.Position.X - Target.Position.X),
            Math.Abs(Source.Position.Y - Target.Position.Y));

        /// <inheritdoc />
        void IForce.ApplyTo(Simulation simulation)
        {
            var source = Source;
            var target = Target;

            var delta = target.Position - source.Position;
            var direction = Vector2.Normalize(delta);
            var deltaNorm = direction.Length();
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