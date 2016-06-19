using System;
using System.Collections.Generic;

namespace Boing
{
    // TODO axis-align force for pairs of nodes
    // TODO hysteresis spring force

    public sealed class Simulation
    {
        private readonly HashSet<Node> _nodes = new HashSet<Node>();
        private readonly HashSet<ILocalForce> _localForces = new HashSet<ILocalForce>();
        private readonly List<IGlobalForce> _globalForces = new List<IGlobalForce>();

        public IEnumerable<Node> Nodes => _nodes;
        public IEnumerable<ILocalForce> LocalForces => _localForces;
        public IEnumerable<IGlobalForce> GlobalForces => _globalForces;

        public void Update(float dt)
        {
            foreach (var force in _globalForces)
                force.ApplyTo(this);

            foreach (var force in _localForces)
                force.Apply();

            foreach (var node in _nodes)
                node.Update(dt);
        }

        public float GetTotalEnergy()
        {
            float sum = 0;
            foreach (var node in _nodes)
                sum += 0.5f * node.Mass * (float)Math.Pow(node.Velocity.Norm(), 2);
            return sum;
        }

        public void Clear()
        {
            _nodes.Clear();
            _localForces.Clear();
            _globalForces.Clear();
        }

        public void Add(IGlobalForce force)
        {
            _globalForces.Add(force);
        }

        public void Add(Node node)
        {
            if (!_nodes.Add(node))
                throw new ArgumentException("Already exists.", nameof(node));
        }

        public void Remove(Node node)
        {
            if (!_nodes.Remove(node))
                throw new ArgumentException("Not found.", nameof(node));

            foreach (var localForce in node.LocalForces)
                _localForces.Remove(localForce);
        }

        public void Add(ILocalForce localForce)
        {
            if (!_localForces.Add(localForce))
                throw new ArgumentException("Already exists.", nameof(localForce));

            foreach (var node in localForce.AppliesToNodes)
                node.LocalForces.Add(localForce);
        }

        public void Remove(ILocalForce localForce)
        {
            if (!_localForces.Remove(localForce))
                throw new ArgumentException("Not found.", nameof(localForce));

            foreach (var node in localForce.AppliesToNodes)
                node.LocalForces.Remove(localForce);
        }
    }
}