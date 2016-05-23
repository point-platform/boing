using Xunit;

namespace Boing.Tests
{
    public sealed class Rectangle2fTest
    {
        [Fact]
        public void Corners()
        {
            var rect = new Rectangle2f(9, 9, 2, 2);

            Assert.Equal(new Vector2f(9, 9), rect.Min);
            Assert.Equal(new Vector2f(11, 11), rect.Max);

            Assert.Equal(new Vector2f(9, 9), rect.TopLeft);
            Assert.Equal(new Vector2f(11, 9), rect.TopRight);
            Assert.Equal(new Vector2f(9, 11), rect.BottomLeft);
            Assert.Equal(new Vector2f(11, 11), rect.BottomRight);
        }

        [Fact]
        public void Intersect()
        {
            var rect = new Rectangle2f(9, 9, 2, 2);

            var centre = new Vector2f(10, 10);
            var below = new Vector2f(10, 0);
            var above = new Vector2f(10, 20);
            var left = new Vector2f(0, 10);
            var right = new Vector2f(20, 10);

            Vector2f intersectionPoint;
            float t;

            Assert.True(rect.TryIntersect(new LineSegment2f(below, centre), out intersectionPoint, out t));
            Assert.Equal(new Vector2f(10, 9), intersectionPoint);
            Assert.Equal(0.9, t, precision: 4);

            Assert.True(rect.TryIntersect(new LineSegment2f(above, centre), out intersectionPoint, out t));
            Assert.Equal(new Vector2f(10, 11), intersectionPoint);
            Assert.Equal(0.9, t, precision: 4);

            Assert.True(rect.TryIntersect(new LineSegment2f(left, centre), out intersectionPoint, out t));
            Assert.Equal(new Vector2f(9, 10), intersectionPoint);
            Assert.Equal(0.9, t, precision: 4);

            Assert.True(rect.TryIntersect(new LineSegment2f(right, centre), out intersectionPoint, out t));
            Assert.Equal(new Vector2f(11, 10), intersectionPoint);
            Assert.Equal(0.9, t, precision: 4);
        }
    }
}
