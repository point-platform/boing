// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Boing.Forces
{
    public sealed class ColoumbForce : IGlobalForce
    {
        public ColoumbForce(float repulsion = 20000)
        {
            Repulsion = repulsion;
        }

        public float Repulsion { get; set; }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var node1 in simulation.Nodes)
            {
                foreach (var node2 in simulation.Nodes)
                {
                    if (ReferenceEquals(node1, node2))
                        continue;

                    var delta = node1.Position - node2.Position;
                    var distance = delta.Norm();

                    if (distance == 0.0f)
                        continue;

                    var direction = delta.Normalized();

                    if (!node1.IsPinned && !node2.IsPinned)
                    {
                        var force = direction*Repulsion/(distance*0.5f);
                        node1.ApplyForce(force);
                        node2.ApplyForce(force*-1);
                    }
                    else if (node1.IsPinned && !node2.IsPinned)
                    {
                        node2.ApplyForce(direction*Repulsion/-distance);
                    }
                    else if (!node1.IsPinned && node2.IsPinned)
                    {
                        node1.ApplyForce(direction*Repulsion/distance);
                    }
                }
            }
        }
    }
}