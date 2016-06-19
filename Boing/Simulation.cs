using System;
using System.Collections.Generic;

namespace Boing
{
    // TODO axis-align force for pairs of nodes
    // TODO hysteresis spring force

    public sealed class Simulation
    {
        private readonly List<IGlobalForce> _globalForces = new List<IGlobalForce>();
        private readonly Dictionary<string, Spring> _springById = new Dictionary<string, Spring>();
        private readonly Dictionary<string, Node> _nodeById = new Dictionary<string, Node>();

        public IEnumerable<Node> Nodes => _nodeById.Values;

        public IEnumerable<Spring> Springs => _springById.Values;

        public void Add(IGlobalForce force)
        {
            _globalForces.Add(force);
        }

        public void Update(float dt)
        {
            foreach (var force in _globalForces)
                force.ApplyTo(this);

            foreach (var node in _nodeById.Values)
                node.Update(dt);
        }

        public float GetTotalEnergy()
        {
            float sum = 0;
            foreach (var node in _nodeById.Values)
                sum += 0.5f * node.Mass * (float)Math.Pow(node.Velocity.Norm(), 2);
            return sum;
        }

        public void Clear()
        {
            _nodeById.Clear();
            _springById.Clear();
        }

        public void Add(Node node)
        {
            if (_nodeById.ContainsKey(node.Id))
                throw new ArgumentException("Node with specified ID already exists.", nameof(node));

            _nodeById[node.Id] = node;
        }

        public void Add(Spring spring)
        {
            if (_springById.ContainsKey(spring.Id))
                throw new ArgumentException("Spring with specified ID already exists.", nameof(spring));

            _springById[spring.Id] = spring;
        }

        public void Remove(Node node)
        {
            _nodeById.Remove(node.Id);
            foreach (var spring in new List<Spring>(Springs))
            {
                if (spring.Source.Id == node.Id || spring.Target.Id == node.Id)
                    Remove(spring);
            }
        }

        public void Remove(Spring spring)
        {
            _springById.Remove(spring.Id);
        }

        public Node GetNodeById(string id)
        {
            Node node;
            return _nodeById.TryGetValue(id, out node) ? node : null;
        }

        public Spring GetSpringById(string id)
        {
            Spring spring;
            return _springById.TryGetValue(id, out spring) ? spring : null;
        }
    }
}