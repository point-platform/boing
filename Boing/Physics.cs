using System;
using System.Collections.Generic;
using System.Linq;

namespace Boing
{
    // TODO axis-align force for edges
    // TODO hysterisis spring force for edges
    // TODO within bounds force for nodes

    public sealed class Physics
    {
        private readonly List<IForce> _forces = new List<IForce>();
        private readonly Graph _graph;

        public Physics(Graph graph)
        {
            _graph = graph;
        }

        public void Add(IForce force)
        {
            _forces.Add(force);
        }

        public void Update(float dt)
        {
            foreach (var force in _forces)
                force.ApplyTo(_graph);

            foreach (var node in _graph.Nodes)
                node.Update(dt);
        }

        public float GetTotalEnergy()
        {
            return _graph.Nodes.Sum(node => 0.5f*node.Mass*(float)Math.Pow(node.Velocity.Norm(), 2));
        }
    }
}