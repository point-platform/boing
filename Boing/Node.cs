namespace Boing
{
    public sealed class Node
    {
        private Vector2f _force;

        public string Id { get; }

        public float Mass { get; set; }
        public float Damping { get; set; }
        public bool IsPinned { get; set; }
        public object Tag { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; private set; }

        public Node(string id, float mass = 1.0f, float damping = 0.5f, Vector2f? position = null)
        {
            Id = id;
            Mass = mass;
            Damping = damping;
            Position = position ?? Vector2f.Random();
        }

        public void ApplyForce(Vector2f force)
        {
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

            // Clear acceleration
            _force = Vector2f.Zero;
        }
    }
}