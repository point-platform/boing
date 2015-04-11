namespace Boing
{
    public sealed class Edge
    {
        public string Id { get; private set; }
        public Node Source { get; private set; }
        public Node Target { get; private set; }
        public float Length { get; private set; }
        public float K { get; private set; }

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