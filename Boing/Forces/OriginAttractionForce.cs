namespace Boing.Forces
{
    /// <summary>
    /// An attraction towards the system's origin.
    /// </summary>
    public sealed class OriginAttractionForce : IForce
    {
        public float Stiffness { get; set; }

        public OriginAttractionForce(float stiffness = 40)
        {
            Stiffness = stiffness;
        }

        public void ApplyTo(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                if (node.IsPinned)
                    continue;

                var magnitude = -node.Position.Norm()*Stiffness;

                node.ApplyForce(node.Position.Normalized()*magnitude);
            }
        }
    }
}