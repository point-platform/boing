namespace Boing
{
    public sealed class Edge
    {
        public string Id { get; }
        public Node Source { get; }
        public Node Target { get; }
        public float Length { get; }
        public float K { get; }

        public Edge(string id, Node source, Node target, float length = 100.0f, float k = 80.0f)
        {
            Id = id;
            Source = source;
            Target = target;
            Length = length;
            K = k;
        }
    }
}