using System;

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

        public LineSegment2f LineSegment => new LineSegment2f(Source.Position, Target.Position);

        public Rectangle2f Bounds => new Rectangle2f(
            Math.Min(Source.Position.X, Target.Position.X),
            Math.Min(Source.Position.Y, Target.Position.Y),
            Math.Abs(Source.Position.X - Target.Position.X),
            Math.Abs(Source.Position.Y - Target.Position.Y));
    }
}