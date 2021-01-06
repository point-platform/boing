#region License

//  Copyright 2015-2021 Drew Noakes, Krzysztof Dul
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

using System.Numerics;
using Xunit;

namespace Boing.Tests
{
    public sealed class Rectangle2fTest
    {
        [Fact]
        public void Corners()
        {
            var rect = new Rectangle2f(9, 9, 2, 2);

            Assert.Equal(new Vector2(9, 9), rect.Min);
            Assert.Equal(new Vector2(11, 11), rect.Max);

            Assert.Equal(new Vector2(9, 9), rect.TopLeft);
            Assert.Equal(new Vector2(11, 9), rect.TopRight);
            Assert.Equal(new Vector2(9, 11), rect.BottomLeft);
            Assert.Equal(new Vector2(11, 11), rect.BottomRight);
        }

        [Fact]
        public void Intersect()
        {
            var rect = new Rectangle2f(9, 9, 2, 2);

            var centre = new Vector2(10, 10);
            var below = new Vector2(10, 0);
            var above = new Vector2(10, 20);
            var left = new Vector2(0, 10);
            var right = new Vector2(20, 10);

            Vector2 intersectionPoint;
            float t;

            Assert.True(rect.TryIntersect(new LineSegment2f(below, centre), out intersectionPoint, out t));
            Assert.Equal(new Vector2(10, 9), intersectionPoint);
            Assert.Equal(0.9, t, precision: 4);

            Assert.True(rect.TryIntersect(new LineSegment2f(above, centre), out intersectionPoint, out t));
            Assert.Equal(new Vector2(10, 11), intersectionPoint);
            Assert.Equal(0.9, t, precision: 4);

            Assert.True(rect.TryIntersect(new LineSegment2f(left, centre), out intersectionPoint, out t));
            Assert.Equal(new Vector2(9, 10), intersectionPoint);
            Assert.Equal(0.9, t, precision: 4);

            Assert.True(rect.TryIntersect(new LineSegment2f(right, centre), out intersectionPoint, out t));
            Assert.Equal(new Vector2(11, 10), intersectionPoint);
            Assert.Equal(0.9, t, precision: 4);
        }
    }
}
