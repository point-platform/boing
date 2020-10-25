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
    /// A force that acts upon point masses as a viscous environment would, exerting a force
    /// that inhibits movement and is proportional to point mass velocity.
    /// </summary>
    public sealed class ViscousForce : IForce
    {
        /// <summary>
        /// Gets and sets the coefficient of viscousity.
        /// </summary>
        public float Coefficient { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="ViscousForce"/>.
        /// </summary>
        /// <param name="coefficient">The initial coefficient of viscousity.</param>
        public ViscousForce(float coefficient)
        {
            Coefficient = coefficient;
        }

        /// <inheritdoc />
        void IForce.ApplyTo(Simulation simulation)
        {
            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                pointMass.ApplyForce(pointMass.Velocity*-Coefficient);
            }
        }
    }
}