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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Boing
{
    /// <summary>
    /// Top-level object for a Boing physical simulation.
    /// Contains instances of <see cref="IPointMass{TVec}"/> and <see cref="IForce{TVec}"/>,
    /// and can update them one time step at a time.
    /// </summary>
    public sealed class Simulation<TVec> : IEnumerable
    {
        private readonly HashSet<IPointMass<TVec>> _pointMasses = new();
        private readonly HashSet<IForce<TVec>> _forces = new();

        /// <summary>
        /// Gets the set of point masses within this simulation.
        /// </summary>
        public IEnumerable<IPointMass<TVec>> PointMasses => _pointMasses;

        /// <summary>
        /// Gets the set of forces within this simulation.
        /// </summary>
        public IEnumerable<IForce<TVec>> Forces => _forces;

        /// <summary>
        /// Progresses the simulation one time step, updating the <see cref="IPointMass{TVec}.Velocity"/> and
        /// <see cref="IPointMass{TVec}.Position"/> of all point masses within the simulation.
        /// </summary>
        /// <param name="dt">Elapsed time since the last update, in seconds.</param>
        public void Update(float dt)
        {
            foreach (var force in _forces)
                force.ApplyTo(this);

            foreach (var pointMass in _pointMasses)
                pointMass.Update(dt);
        }

        /// <summary>
        /// Aggregates the kinetic energy of all point masses in the simulation, and
        /// returns the sum.
        /// </summary>
        /// <remarks>
        /// Kinetic energy is computed based on point mass velocities.
        /// It can be useful in determining whether a simulation has come to rest,
        /// although a moving average should be applied to prevent incorrectly taking
        /// a transiently near-static moment to mean equilibrium is reached.
        /// </remarks>
        /// <returns></returns>
        public float GetTotalKineticEnergy()
        {
            // Ek = 1/2 m v^2

            float sum = 0;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var pointMass in _pointMasses)
            {
                var speed = pointMass.Speed;
                sum += pointMass.Mass*speed*speed;
            }

            return sum / 2;
        }

        /// <summary>
        /// Removes all <see cref="IPointMass{TVec}"/> and <see cref="IForce{TVec}"/> objects from the simulation.
        /// </summary>
        public void Clear()
        {
            _pointMasses.Clear();
            _forces.Clear();
        }

        /// <summary>
        /// Adds <paramref name="force"/> to the simulation.
        /// </summary>
        /// <param name="force">The <see cref="IForce{TVec}"/> to add to the simulation.</param>
        /// <exception cref="ArgumentException"><paramref name="force"/> already exists.</exception>
        public void Add(IForce<TVec> force)
        {
            if (!_forces.Add(force))
                throw new ArgumentException("Already exists.", nameof(force));
        }

        /// <summary>
        /// Removes <paramref name="force"/> from the simulation.
        /// </summary>
        /// <param name="force">The <see cref="IForce{TVec}"/> to remove from the simulation.</param>
        public bool Remove(IForce<TVec> force) => _forces.Remove(force);

        /// <summary>
        /// Adds <paramref name="pointMass"/> to the simulation.
        /// </summary>
        /// <param name="pointMass">The <see cref="IPointMass{TVec}"/> to add to the simulation.</param>
        /// <exception cref="ArgumentException"><paramref name="pointMass"/> already exists.</exception>
        public void Add(IPointMass<TVec> pointMass)
        {
            if (!_pointMasses.Add(pointMass))
                throw new ArgumentException("Already exists.", nameof(pointMass));
        }

        /// <summary>
        /// Removes <paramref name="pointMass"/> from the simulation.
        /// </summary>
        /// <param name="pointMass">The <see cref="IPointMass{TVec}"/> to remove from the simulation.</param>
        public bool Remove(IPointMass<TVec> pointMass) => _pointMasses.Remove(pointMass);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException($"{nameof(Simulation<TVec>)} only implements {nameof(IEnumerable)} to enable C# object initialisers.");
        }
    }
}