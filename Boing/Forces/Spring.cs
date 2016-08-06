﻿using System;
using System.Diagnostics;

namespace Boing
{
    /// <summary>
    /// Models a linear spring between two <see cref="PointMass"/> instances according to Hooke's law.
    /// </summary>
    public sealed class Spring : ILocalForce
    {
        public PointMass Source { get; }
        public PointMass Target { get; }
        public float Length { get; set; }
        public float K { get; set; }

        public Spring(PointMass source, PointMass target, float length = 100.0f, float k = 80.0f)
        {
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

        public void Apply()
        {
            var source = Source;
            var target = Target;

            var delta = target.Position - source.Position;
            var displacement = Length - delta.Norm();
            var direction = delta.Normalized();

            Debug.Assert(!float.IsNaN(displacement), "!float.IsNaN(displacement)");

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