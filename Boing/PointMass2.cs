#region License

//  Copyright 2015-2021 Drew Noakes, Krzysztof Dul
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

using System.Diagnostics;
using System.Numerics;

namespace Boing
{
    /// <inheritdoc />
    public sealed class PointMass2 : IPointMass<Vector2>
    {
        private Vector2 _force;

        /// <inheritdoc />
        public float Mass { get; set; }

        /// <inheritdoc />
        public bool IsPinned { get; set; }

        /// <inheritdoc />
        public object? Tag { get; set; }

        /// <inheritdoc />
        public Vector2 Position { get; set; }

        /// <inheritdoc />
        public Vector2 Velocity { get; set; }

        /// <inheritdoc />
        public float Speed => Velocity.Length();

        /// <summary>
        /// Initialises a new instance of <see cref="PointMass2"/>.
        /// </summary>
        /// <param name="mass">The mass, in kilograms. Defaults to 1.</param>
        /// <param name="position">The position, in metres. Defaults to the origin.</param>
        public PointMass2(float mass = 1.0f, Vector2 position = default)
        {
            Mass = mass;
            Position = position;
        }

        /// <inheritdoc />
        public void ApplyForce(Vector2 force)
        {
            Debug.Assert(!float.IsNaN(force.X) && !float.IsNaN(force.Y), "!float.IsNaN(force.X) && !float.IsNaN(force.Y)");
            Debug.Assert(!float.IsInfinity(force.X) && !float.IsInfinity(force.Y), "!float.IsInfinity(force.X) && !float.IsInfinity(force.Y)");

            // Accumulate force
            _force += force;
        }

        /// <inheritdoc />
        public void ApplyImpulse(Vector2 impulse)
        {
            // Update velocity
            Velocity += impulse/Mass;
        }

        /// <inheritdoc />
        public void Update(float dt)
        {
            // Update velocity
            Velocity += _force/Mass*dt;

            // Update position
            Position += Velocity*dt;

            Debug.Assert(!float.IsNaN(Position.X) && !float.IsNaN(Position.Y), "!float.IsNaN(Position.X) && !float.IsNaN(Position.Y)");
            Debug.Assert(!float.IsInfinity(Position.X) && !float.IsInfinity(Position.Y), "!float.IsInfinity(Position.X) && !float.IsInfinity(Position.Y)");

            // Clear force
            _force = Vector2.Zero;
        }
    }
}