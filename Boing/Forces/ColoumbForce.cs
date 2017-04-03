#region License

//  Copyright 2015-2017 Drew Noakes, Krzysztof Dul
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
    public sealed class ColoumbForce : IForce
    {
        public ColoumbForce(float repulsion = 20000, float maxDistance = float.MaxValue)
        {
            Repulsion = repulsion;
            MaxDistance = maxDistance;
        }

        public float Repulsion { get; set; }
        public float MaxDistance { get; set; }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var pointMass1 in simulation.PointMasses)
            {
                foreach (var pointMass2 in simulation.PointMasses)
                {
                    if (ReferenceEquals(pointMass1, pointMass2) || pointMass2.IsPinned)
                        continue;

                    var delta = pointMass2.Position - pointMass1.Position;
                    var distance = delta.Norm();

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (distance == 0.0f || distance > MaxDistance)
                        continue;

                    pointMass2.ApplyForce(delta*Repulsion/(distance*distance));
                }
            }
        }
    }
}