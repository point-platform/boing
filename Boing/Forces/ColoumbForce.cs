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
    /// A force between point masses that follows an inverse square law.
    /// </summary>
    /// <remarks>
    /// All pairs of unpinned point masses in the simulation will
    /// attract or repel one another with equal and opposite force.
    /// <para />
    /// The force is proportional to the inverse square of the distance, meaning it
    /// is strongest for close particles, and the intensity diminishes quadratically
    /// as distance increases.
    /// <para />
    /// The intensity of this effect can be linearly scaled using <see cref="Scale"/>.
    /// <para />
    /// As an optimisation, you may set the <see cref="MaxDistance"/> property to
    /// prevent this force applying over some distance threshold. By default this is
    /// 20,000.
    /// <para />
    /// See https://en.wikipedia.org/wiki/Coulomb%27s_law for more information.
    /// </remarks>
    public sealed class ColoumbForce : IForce<Vector2>, IForce<Vector3>
    {
        /// <summary>
        /// Initialises a new instance of <see cref="ColoumbForce"/>.
        /// </summary>
        /// <param name="scale">The <see cref="Scale"/> to use. The default value is 20,000. Positive values cause repulsion, while negative values cause attraction.</param>
        /// <param name="maxDistance">The <see cref="MaxDistance"/> to use. The default value is <see cref="float.MaxValue"/>.</param>
        public ColoumbForce(float scale = 20_000, float maxDistance = float.MaxValue)
        {
            Scale = scale;
            MaxDistance = maxDistance;
        }

        /// <summary>
        /// Gets and sets a value that linearly scales the intensity of the force applied between pairs of point masses.
        /// </summary>
        /// <remarks>
        /// This value is equivalent to the force, in Newtons, applied to point masses one unit apart.
        /// <para />
        /// Positive values cause repulsion, while negative values cause attraction.
        /// This value may be safely modified while the simulation is running.
        /// </remarks>
        public float Scale { get; set; }

        /// <summary>
        /// Gets and sets the maximum distance over which the force applies.
        /// Point masses separated by distances greater than this threshold will not have a force applied.
        /// </summary>
        /// <remarks>
        /// A value less than or equal to zero effectively disables this force.
        /// This value may be safely modified while the simulation is running.
        /// </remarks>
        public float MaxDistance { get; set; }

        /// <inheritdoc />
        void IForce<Vector2>.ApplyTo(Simulation<Vector2> simulation)
        {
            if (MaxDistance <= 0)
                return;

            foreach (var pointMass1 in simulation.PointMasses)
            {
                foreach (var pointMass2 in simulation.PointMasses)
                {
                    if (ReferenceEquals(pointMass1, pointMass2) || pointMass2.IsPinned)
                        continue;

                    var delta = pointMass2.Position - pointMass1.Position;
                    var distance = delta.Length();

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (distance == 0.0f || distance > MaxDistance)
                        continue;

                    pointMass2.ApplyForce(delta*Scale/(distance*distance));
                }
            }
        }

        /// <inheritdoc />
        void IForce<Vector3>.ApplyTo(Simulation<Vector3> simulation)
        {
            if (MaxDistance <= 0)
                return;

            foreach (var pointMass1 in simulation.PointMasses)
            {
                foreach (var pointMass2 in simulation.PointMasses)
                {
                    if (ReferenceEquals(pointMass1, pointMass2) || pointMass2.IsPinned)
                        continue;

                    var delta = pointMass2.Position - pointMass1.Position;
                    var distance = delta.Length();

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (distance == 0.0f || distance > MaxDistance)
                        continue;

                    pointMass2.ApplyForce(delta*Scale/(distance*distance));
                }
            }
        }
    }
}