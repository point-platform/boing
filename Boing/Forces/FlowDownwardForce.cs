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
    public sealed class FlowDownwardForce : IGlobalForce
    {
        /// <summary>
        /// The magnitude of the downward force, in Newtons.
        /// </summary>
        public float Magnitude { get; set; }

        /// <param name="magnitude">The magnitude of the downward force, in Newtons.</param>
        public FlowDownwardForce(float magnitude = 10.0f)
        {
            Magnitude = magnitude;
        }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                pointMass.ApplyForce(new Vector2f(0, Magnitude));
            }
        }
    }
}