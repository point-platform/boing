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

namespace Boing
{
    /// <summary>
    /// Models an idealised physical body for the purposes of simulation, having mass and infinitesimal volume and dimension.
    /// </summary>
    /// <remarks>
    /// Point masses may be pinned, in which case forces and impulses should not be applied.
    /// It is up to implementations of <see cref="IForce{TVec}"/> to ensure this.
    /// <para />
    /// You may store custom data in the <see cref="Tag"/> property.
    /// <para />
    /// Whilst all properties of this type are mutable, in normal usage <see cref="Position"/> and <see cref="Velocity"/>
    /// are modified by <see cref="IForce{TVec}"/> implementations during simulation <see cref="Simulation{TVec}.Update"/>.
    /// <para />
    /// <see cref="Mass"/> and <see cref="IsPinned"/> may safely be modified at any time, having a direct effect on the simulation.
    /// </remarks>
    public interface IPointMass<TVec>
    {
        /// <summary>
        /// Gets and sets the mass of this point mass.
        /// </summary>
        /// <remarks>
        /// This value may be safely modified while the simulation is running.
        /// </remarks>
        float Mass { get; set; }

        /// <summary>
        /// Gets and sets whether this point mass is pinned. When pinned, it should not be moved.
        /// </summary>
        /// <remarks>
        /// This value may be safely modified while the simulation is running.
        /// </remarks>
        bool IsPinned { get; set; }

        /// <summary>
        /// Gets and sets an object of miscellaneous data to associate with this point mass.
        /// </summary>
        /// <remarks>
        /// This value is optional. It is provided for convenience, and is unused by the library.
        /// </remarks>
        object? Tag { get; set; }

        /// <summary>
        /// Gets and sets the position of this point mass.
        /// </summary>
        /// <remarks>
        /// In normal usage this property is modified by <see cref="IForce{TVec}"/> implementations
        /// during simulation <see cref="Simulation{TVec}.Update"/>.
        /// </remarks>
        TVec Position { get; set; }

        /// <summary>
        /// Gets and sets the position of this point mass.
        /// </summary>
        /// <remarks>
        /// In normal usage this property is modified by <see cref="IForce{TVec}"/> implementations
        /// during simulation <see cref="Simulation{TVec}.Update"/>.
        /// </remarks>
        TVec Velocity { get; set; }

        /// <summary>
        /// Gets the speed of the point mass. This is the length of the <see cref="Velocity"/> vector.
        /// </summary>
        float Speed { get; }

        /// <summary>
        /// Applies <paramref name="force"/> to the point mass.
        /// </summary>
        /// <remarks>
        /// Forces are accumulated in each time step, and this method may be called multiple times in a
        /// single time step. Only when <see cref="PointMass2.Update"/> is called on this point mass will the
        /// velocity and position be updated based upon any accumulated force.
        /// </remarks>
        /// <param name="force">The force to apply to the point mass.</param>
        void ApplyForce(TVec force);

        /// <summary>
        /// Applies <paramref name="impulse"/> to the point mass.
        /// </summary>
        /// <remarks>
        /// Impulses are applied directly to the point mass's <see cref="PointMass2.Velocity"/>, and as such their impact is
        /// unaffected by the simulation's step size. However <see cref="PointMass2.Position"/> will only be modified by the next call
        /// to <see cref="PointMass2.Update"/>.
        /// </remarks>
        /// <param name="impulse">The impulse to apply to the point mass.</param>
        void ApplyImpulse(TVec impulse);

        /// <summary>
        /// Updates <see cref="PointMass2.Velocity"/> according to any applied forces and the time delta, and then
        /// updates <see cref="PointMass2.Position"/>.
        /// </summary>
        /// <param name="dt">Elapsed time since the last update, in seconds.</param>
        void Update(float dt);
    }
}