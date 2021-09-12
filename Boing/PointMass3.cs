using System.Diagnostics;
using System.Numerics;

namespace Boing
{
    /// <inheritdoc />
    public sealed class PointMass3 : IPointMass<Vector3>
    {
        private Vector3 _force;

        /// <inheritdoc />
        public float Mass { get; set; }

        /// <inheritdoc />
        public bool IsPinned { get; set; }

        /// <inheritdoc />
        public object? Tag { get; set; }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public Vector3 Velocity { get; set; }

        /// <inheritdoc />
        public float Speed => Velocity.Length();

        /// <summary>
        /// Initialises a new instance of <see cref="PointMass3"/>.
        /// </summary>
        /// <param name="mass">The mass, in kilograms. Defaults to 1.</param>
        /// <param name="position">The position, in metres. Defaults to the origin.</param>
        public PointMass3(float mass = 1.0f, Vector3 position = default)
        {
            Mass = mass;
            Position = position;
        }

        /// <inheritdoc />
        public void ApplyForce(Vector3 force)
        {
            Debug.Assert(!float.IsNaN(force.X) && !float.IsNaN(force.Y) && !float.IsNaN(force.Z), "!float.IsNaN(force.X) && !float.IsNaN(force.Y) && !float.IsNaN(force.Z)");
            Debug.Assert(!float.IsInfinity(force.X) && !float.IsInfinity(force.Y) && !float.IsInfinity(force.Z), "!float.IsInfinity(force.X) && !float.IsInfinity(force.Y) && !float.IsInfinity(force.Z)");

            // Accumulate force
            _force += force;
        }

        /// <inheritdoc />
        public void ApplyImpulse(Vector3 impulse)
        {
            // Update velocity
            Velocity += impulse/Mass;
        }

        /// <inheritdoc />
        public void Update(float dt)
        {
            // Update velocity
            Velocity += _force/Mass*dt;

            // Update position
            Position += Velocity*dt;

            Debug.Assert(!float.IsNaN(Position.X) && !float.IsNaN(Position.Y) && !float.IsNaN(Position.Z), "!float.IsNaN(Position.X) && !float.IsNaN(Position.Y) && !float.IsNaN(Position.Z)");
            Debug.Assert(!float.IsInfinity(Position.X) && !float.IsInfinity(Position.Y) && !float.IsInfinity(Position.Z), "!float.IsInfinity(Position.X) && !float.IsInfinity(Position.Y) && !float.IsInfinity(Position.Z)");

            // Clear force
            _force = Vector3.Zero;
        }
    }
}