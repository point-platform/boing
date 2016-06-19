namespace Boing.Forces
{
    public sealed class FlowDownwardForce : IGlobalForce
    {
        public float Magnitude { get; set; }

        public FlowDownwardForce(float magnitude = 10.0f)
        {
            Magnitude = magnitude;
        }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var node in simulation.Nodes)
            {
                if (node.IsPinned)
                    continue;

                node.ApplyForce(new Vector2f(0, Magnitude));
            }
        }
    }
}