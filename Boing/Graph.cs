using System;
using System.Collections.Generic;

namespace Boing
{
    public sealed class Graph
    {
        private readonly Dictionary<string, Edge> _edgeById = new Dictionary<string, Edge>();
        private readonly Dictionary<string, Node> _nodeById = new Dictionary<string, Node>();

        public IEnumerable<Node> Nodes
        {
            get { return _nodeById.Values; }
        }

        public IEnumerable<Edge> Edges
        {
            get { return _edgeById.Values; }
        }

        public void Clear()
        {
            _nodeById.Clear();
            _edgeById.Clear();
        }

        public void AddNode(Node node)
        {
            if (_nodeById.ContainsKey(node.Id))
                throw new ArgumentException("Node with specified ID already exists.", "node");

            _nodeById[node.Id] = node;
        }

        public void AddEdge(Edge edge)
        {
            if (_edgeById.ContainsKey(edge.Id))
                throw new ArgumentException("Edge with specified ID already exists.", "edge");

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