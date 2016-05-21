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
            foreach (var node in graph.Nodes)
            {
                if (node.IsPinned)
                    continue;

                node.ApplyForce(new Vector2f(0, Magnitude));
            }
        }
    }
}