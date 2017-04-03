#region License

//  Copyright 2015-2017 Drew Noakes, Krzysztof Dul
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

using System;
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