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
    // TODO new local force: axis-align force for pairs of point masses
    // TODO new local force: hysteresis spring force

    /// <summary>
    /// A force that potentially applies to every <see cref="IPointMass{TVec}"/> in the <see cref="Simulation{TVec}"/>.
    /// </summary>
    public interface IForce<TVec>
    {
        /// <summary>
        /// Evaluates the impact of this force on the simulation at the current point in time,
        /// and applies forces to point masses as required.
        /// </summary>
        /// <remarks>
        /// Implementations should call <see cref="IPointMass{TVec}.ApplyForce"/> on point masses.
        /// They can update any number of point masses, or none at all. The force may be identical across
        /// all point masses, but is more likely to vary from point to point.
        /// </remarks>
        /// <param name="simulation">The simulation to apply this force to.</param>
        void ApplyTo(Simulation<TVec> simulation);
    }
}