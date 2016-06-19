using System;

namespace Boing.Forces
{
    public sealed class KeepWithinBoundsForce : IGlobalForce
    {
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float Magnitude { get; set; }
        public float MaximumForce { get; set; }

        public KeepWithinBoundsForce(float magnitude = 3.0f, float maximumForce = 1000.0f)
        {
            Magnitude = magnitude;
            MaximumForce = maximumForce;
        }

        public void ApplyTo(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                if (node.IsPinned)
                    continue;

                if (node.Position.X > MaxX)
                {
                    var force = (float)Math.Pow(node.Position.X - MaxX, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    node.ApplyForce(new Vector2f(-force, 0));
                }
                else if (node.Position.X < -MaxX)
                {
                    var force = (float)Math.Pow(-MaxX - node.Position.X, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    node.ApplyForce(new Vector2f(force, 0));
                }

                if (node.Position.Y > MaxY)
                {
                    var force = (float)Math.Pow(node.Position.Y - MaxY, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    node.ApplyForce(new Vector2f(0, -force));
                }
                else if (node.Position.Y < -MaxY)
                {
                    var force = (float)Math.Pow(-MaxY - node.Position.Y, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    node.ApplyForce(new Vector2f(0, force));
                }
            }
        }
    }
}