using System.Collections.Generic;
using System.Diagnostics;

namespace Boing
{
    public sealed class PointMass
    {
        private Vector2f _force;
        internal HashSet<ILocalForce> LocalForces { get; } = new HashSet<ILocalForce>();

        public float Mass { get; set; }
        public float Damping { get; set; }
        public bool IsPinned { get; set; }
        public object Tag { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; private set; }

        public PointMass(float mass = 1.0f, float damping = 0.5f, Vector2f? position = null)
        {
            Mass = mass;
            Damping = damping;
            Position = position ?? Vector2f.Random();
        }

        public void ApplyForce(Vector2f force)
        {
            Debug.Assert(!float.IsNaN(force.X) && !float.IsNaN(force.Y), "!float.IsNaN(force.X) && !float.IsNaN(force.Y)");
            Debug.Assert(!float.IsInfinity(force.X) && !float.IsInfinity(force.Y), "!float.IsInfinity(force.X) && !float.IsInfinity(force.Y)");

            // Accumulate force
            _force += force;
        }

        public void Update(float dt)
        {
            // Update velocity
            Velocity += _force/Mass*dt;
            Velocity *= Damping;

            // Update position
            Position += Velocity*dt;

            Debug.Assert(!float.IsNaN(Position.X) && !float.IsNaN(Position.Y), "!float.IsNaN(Position.X) && !float.IsNaN(Position.Y)");
            Debug.Assert(!float.IsInfinity(Position.X) && !float.IsInfinity(Position.Y), "!float.IsInfinity(Position.X) && !float.IsInfinity(Position.Y)");

            // Clear acceleration
            _force = Vector2f.Zero;
        }
    }
}