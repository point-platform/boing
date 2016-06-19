namespace Boing.Forces
{
    /// <summary>
    /// An attraction towards the system's origin.
    /// </summary>
    public sealed class OriginAttractionForce : IGlobalForce
    {
        public float Stiffness { get; set; }

        public OriginAttractionForce(float stiffness = 40)
        {
            Stiffness = stiffness;
        }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var node in simulation.Nodes)
            {
                if (node.IsPinned)
                    continue;

                var magnitude = -node.Position.Norm()*Stiffness;

                node.ApplyForce(node.Position.Normalized()*magnitude);
            }
        }
    }
}