namespace Boing.Forces
{
    public sealed class FlowDownwardForce : IForce
    {
        public float Magnitude { get; set; }

        public FlowDownwardForce(float magnitude = 10.0f)
        {
            Magnitude = magnitude;
        }

        public void ApplyTo(Graph graph)
        {
            foreach (var edge in graph.Edges)
            {
                if (edge.Target.IsPinned)
                    continue;

                edge.Target.ApplyForce(new Vector2f(0, Magnitude));
            }
        }
    }
}