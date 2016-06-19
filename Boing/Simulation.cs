using System;
using System.Collections.Generic;

namespace Boing
{
    // TODO axis-align force for edges
    // TODO hysterisis spring force for edges

    public sealed class Simulation
    {
        private readonly List<IGlobalForce> _forces = new List<IGlobalForce>();
        private readonly Dictionary<string, Edge> _edgeById = new Dictionary<string, Edge>();
        private readonly Dictionary<string, Node> _nodeById = new Dictionary<string, Node>();

        public IEnumerable<Node> Nodes => _nodeById.Values;

        public IEnumerable<Edge> Edges => _edgeById.Values;

        public void Add(IGlobalForce force)
        {
            _forces.Add(force);
        }

        public void Update(float dt)
        {
            foreach (var force in _forces)
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
            _edgeById.Clear();
        }

        public void AddNode(Node node)
        {
            if (_nodeById.ContainsKey(node.Id))
                throw new ArgumentException("Node with specified ID already exists.", nameof(node));

            _nodeById[node.Id] = node;
        }

        public void AddEdge(Edge edge)
        {
            if (_edgeById.ContainsKey(edge.Id))
                throw new ArgumentException("Edge with specified ID already exists.", nameof(edge));

            _edgeById[edge.Id] = edge;
        }

        public void RemoveNode(Node node)
        {
            _nodeById.Remove(node.Id);
            foreach (var edge in new List<Edge>(Edges))
            {
                if (edge.Source.Id == node.Id || edge.Target.Id == node.Id)
                    RemoveEdge(edge);
            }
        }

        public void RemoveEdge(Edge edge)
        {
            _edgeById.Remove(edge.Id);
        }

        public Node GetNodeById(string id)
        {
            Node node;
            return _nodeById.TryGetValue(id, out node) ? node : null;
        }

        public Edge GetEdgeById(string id)
        {
            Edge edge;
            return _edgeById.TryGetValue(id, out edge) ? edge : null;
        }
    }
}