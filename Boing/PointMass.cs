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

using System.Diagnostics;

namespace Boing
{
    public sealed class PointMass
    {
        private Vector2f _force;

        public float Mass { get; set; }
        public bool IsPinned { get; set; }
        public object Tag { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; set; }

        public PointMass(float mass = 1.0f, Vector2f? position = null)
        {
            Mass = mass;
            Position = position ?? Vector2f.Random();
        }

        public void ApplyForce(Vector2f force)
        {
            Debug.Assert(!float.IsNaN(force.X) && !float.IsNaN(force.Y), "!float.IsNaN(force.X) && !float.IsNaN(force.Y)");
            Debug.Assert(!float.IsInfinity(force.X) && !float.IsInfinity(force.Y), "!float.IsInfinity(force.X) && !float.IsInfinity(force.Y)");

            // Accumulate force
            _force += force;
        }

        public void ApplyImpulse(Vector2f impulse)
        {
            // Update velocity
            Velocity += impulse/Mass;
        }

        public void Update(float dt)
        {
            // Update velocity
            Velocity += _force/Mass*dt;

            // Update position
            Position += Velocity*dt;

            Debug.Assert(!float.IsNaN(Position.X) && !float.IsNaN(Position.Y), "!float.IsNaN(Position.X) && !float.IsNaN(Position.Y)");
            Debug.Assert(!float.IsInfinity(Position.X) && !float.IsInfinity(Position.Y), "!float.IsInfinity(Position.X) && !float.IsInfinity(Position.Y)");

            // Clear force
            _force = Vector2f.Zero;
        }
    }
}