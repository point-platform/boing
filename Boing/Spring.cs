using System;
using System.Collections.Generic;

namespace Boing
{
    /// <summary>
    /// Models a linear spring between two <see cref="Node"/> instances according to Hooke's law.
    /// </summary>
    public sealed class Spring : ILocalForce
    {
        public Node Source { get; }
        public Node Target { get; }
        public float Length { get; }
        public float K { get; }

        public Spring(Node source, Node target, float length = 100.0f, float k = 80.0f)
        {
            Source = source;
            Target = target;
            Length = length;
            K = k;

            AppliesToNodes = new[] {source, target};
        }

        public LineSegment2f LineSegment => new LineSegment2f(Source.Position, Target.Position);

        public Rectangle2f Bounds => new Rectangle2f(
            Math.Min(Source.Position.X, Target.Position.X),
            Math.Min(Source.Position.Y, Target.Position.Y),
            Math.Abs(Source.Position.X - Target.Position.X),
            Math.Abs(Source.Position.Y - Target.Position.Y));

        public IEnumerable<Node> AppliesToNodes { get; }

        public void Apply()
        {
            var source = Source;
            var target = Target;

            var delta = target.Position - source.Position;
            var displacement = Length - delta.Norm();
            var direction = delta.Normalized();

            if (!source.IsPinned && !target.IsPinned)
            {
                source.ApplyForce(direction*(K*displacement*-0.5f));
                target.ApplyForce(direction*(K*displacement*0.5f));
            }
            else if (source.IsPinned && !target.IsPinned)
            {
                target.ApplyForce(direction*(K*displacement));
            }
            else if (!source.IsPinned && target.IsPinned)
            {
                source.ApplyForce(direction*(K*-displacement));
            }
        }
    }
}