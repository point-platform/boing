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

using System.Diagnostics;
using System.Numerics;

namespace Boing
{
    /// <summary>
    /// Models an idealised physical body for the purposes of simulation, having mass and infinitesimal volume and dimension.
    /// </summary>
    /// <remarks>
    /// Point masses may be pinned, in which case forces and impulses should not be applied.
    /// It is up to implementations of <see cref="IForce"/> to ensure this.
    /// <para />
    /// You may store custom data in the <see cref="Tag"/> property.
    /// <para />
    /// Whilst all properties of this type are mutable, in normal usage <see cref="Position"/> and <see cref="Velocity"/>
    /// are modified by <see cref="IForce"/> implementations during simulation <see cref="Simulation.Update"/>.
    /// <para />
    /// <see cref="Mass"/> and <see cref="IsPinned"/> may safely be modified at any time, having a direct effect on the simulation.
    /// </remarks>
    public sealed class PointMass
    {
        private Vector2 _force;

        /// <summary>
        /// Gets and sets the mass of this point mass.
        /// </summary>
        /// <remarks>
        /// This value may be safely modified while the simulation is running.
        /// </remarks>
        public float Mass { get; set; }

        /// <summary>
        /// Gets and sets whether this point mass is pinned. When pinned, it should not be moved.
        /// </summary>
        /// <remarks>
        /// This value may be safely modified while the simulation is running.
        /// </remarks>
        public bool IsPinned { get; set; }

        /// <summary>
        /// Gets and sets an object of miscellaneous data to associate with this point mass.
        /// </summary>
        /// <remarks>
        /// This value is optional. It is provided for convenience, and is unused by the library.
        /// </remarks>
        public object? Tag { get; set; }

        /// <summary>
        /// Gets and sets the position of this point mass.
        /// </summary>
        /// <remarks>
        /// In normal usage this property is modified by <see cref="IForce"/> implementations
        /// during simulation <see cref="Simulation.Update"/>.
        /// </remarks>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets and sets the position of this point mass.
        /// </summary>
        /// <remarks>
        /// In normal usage this property is modified by <see cref="IForce"/> implementations
        /// during simulation <see cref="Simulation.Update"/>.
        /// </remarks>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="PointMass"/>.
        /// </summary>
        /// <param name="mass">The initial mass. The default value is 1.</param>
        /// <param name="position">The initial position. The default value is (0,0).</param>
        public PointMass(float mass = 1.0f, Vector2 position = default)
        {
            Mass = mass;
            Position = position;
        }

        /// <summary>
        /// Applies <paramref name="force"/> to the point mass.
        /// </summary>
        /// <remarks>
        /// Forces are accumulated in each time step, and this method may be called multiple times in a
        /// single time step. Only when <see cref="Update"/> is called on this point mass will the
        /// velocity and position be updated based upon any accumulated force.
        /// </remarks>
        /// <param name="force">The force to apply to the point mass.</param>
        public void ApplyForce(Vector2 force)
        {
            Debug.Assert(!float.IsNaN(force.X) && !float.IsNaN(force.Y), "!float.IsNaN(force.X) && !float.IsNaN(force.Y)");
            Debug.Assert(!float.IsInfinity(force.X) && !float.IsInfinity(force.Y), "!float.IsInfinity(force.X) && !float.IsInfinity(force.Y)");

            // Accumulate force
            _force += force;
        }

        /// <summary>
        /// Applies <paramref name="impulse"/> to the point mass.
        /// </summary>
        /// <remarks>
        /// Impulses are applied directly to the point mass's <see cref="Velocity"/>, and as such their impact is
        /// unaffected by the simulation's step size. However <see cref="Position"/> will only be modified by the next call
        /// to <see cref="Update"/>.
        /// </remarks>
        /// <param name="impulse">The impulse to apply to the point mass.</param>
        public void ApplyImpulse(Vector2 impulse)
        {
            // Update velocity
            Velocity += impulse/Mass;
        }

        /// <summary>
        /// Updates <see cref="Velocity"/> according to any applied forces and the time delta, and then
        /// updates <see cref="Position"/>.
        /// </summary>
        /// <param name="dt">Elapsed time since the last update, in seconds.</param>
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