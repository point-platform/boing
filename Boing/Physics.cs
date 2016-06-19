using System;
using System.Collections.Generic;

namespace Boing
{
    // TODO axis-align force for edges
    // TODO hysterisis spring force for edges

    public sealed class Physics
    {
        private readonly List<IGlobalForce> _forces = new List<IGlobalForce>();
        private readonly Graph _graph;

        public Physics(Graph graph)
        {
            _graph = graph;
        }

        public void Add(IGlobalForce force)
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
            float sum = 0;
            foreach (var node in _graph.Nodes)
                sum += 0.5f*node.Mass*(float)Math.Pow(node.Velocity.Norm(), 2);
            return sum;
        }
    }
}