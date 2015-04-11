namespace Boing.Forces
{
    // TODO should apply near border to prevent objects from bouncing on edges

    public sealed class KeepWithinBoundsForce : IForce
    {
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float Magnitude { get; set; }

        public KeepWithinBoundsForce(float magnitude = 10.0f)
        {
            Magnitude = magnitude;
        }

        public void ApplyTo(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                if (node.IsPinned)
                    continue;

                if (node.Position.X > MaxX)
                {
                    node.ApplyForce(new Vector2f(-Magnitude*(node.Position.X - MaxX), 0));
                }
                else if (node.Position.X < -MaxX)
                {
                    node.ApplyForce(new Vector2f(Magnitude*(MaxX - node.Position.X), 0));
                }

                if (node.Position.Y > MaxY)
                {
                    node.ApplyForce(new Vector2f(0, -Magnitude*(node.Position.Y - MaxY)));
                }
                else if (node.Position.Y < -MaxY)
                {
                    node.ApplyForce(new Vector2f(0, Magnitude*(MaxY - node.Position.Y)));
                }
            }
        }
    }
}