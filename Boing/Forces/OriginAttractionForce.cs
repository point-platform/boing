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

using System.Numerics;

namespace Boing
{
    /// <summary>
    /// A force that attracts all non-pinned point masses towards the origin of the coordinate system.
    /// </summary>
    public sealed class OriginAttractionForce : IForce<Vector2>, IForce<Vector3>
    {
        /// <summary>
        /// Gets and sets the magnitude of the force.
        /// </summary>
        /// <remarks>
        /// Positive values cause attraction to the origin, while negative values
        /// cause repulsion from it. Larger values cause larger forces, and a zero
        /// value effectively disables this force.
        /// </remarks>
        public float Stiffness { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="OriginAttractionForce"/>.
        /// </summary>
        /// <param name="stiffness"></param>
        public OriginAttractionForce(float stiffness = 40)
        {
            Stiffness = stiffness;
        }

        /// <inheritdoc />
        void IForce<Vector2>.ApplyTo(Simulation<Vector2> simulation)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (Stiffness == 0)
                return;

            var f = -Stiffness;

            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                pointMass.ApplyForce(f*pointMass.Position);
            }
        }

        /// <inheritdoc />
        void IForce<Vector3>.ApplyTo(Simulation<Vector3> simulation)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (Stiffness == 0)
                return;

            var f = -Stiffness;

            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                pointMass.ApplyForce(f*pointMass.Position);
            }
        }
    }
}