namespace Boing
{
    public sealed class Node
    {
        private Vector2f _acceleration;

        public string Id { get; private set; }
        public float Mass { get; set; }
        public float Damping { get; set; }
        public bool IsPinned { get; set; }
        public object Tag { get; set; }
        public Vector2f Position { get; private set; }
        public Vector2f Velocity { get; private set; }

        public Node(string id, float mass = 1.0f, float damping = 0.5f, Vector2f position = default(Vector2f))
        {
            Id = id;
            Mass = mass;
            Damping = damping;
            Position = position != Vector2f.Zero ? position : Vector2f.Random();
        }

        public void ApplyForce(Vector2f force)
        {
            // Accumulate acceleration
            _acceleration += force/Mass;
        }

        public void Update(float dt)
        {
            // Update velocity
            Velocity += _acceleration*dt;
            Velocity *= Damping;

            // Update position
            Position += Velocity*dt;

            // Clear acceleration
            _acceleration = Vector2f.Zero;
        }
    }
}