using System;

namespace Boing
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

        public void ApplyTo(Simulation simulation)
        {
            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                if (pointMass.Position.X > MaxX)
                {
                    var force = (float)Math.Pow(pointMass.Position.X - MaxX, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    pointMass.ApplyForce(new Vector2f(-force, 0));
                }
                else if (pointMass.Position.X < -MaxX)
                {
                    var force = (float)Math.Pow(-MaxX - pointMass.Position.X, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    pointMass.ApplyForce(new Vector2f(force, 0));
                }

                if (pointMass.Position.Y > MaxY)
                {
                    var force = (float)Math.Pow(pointMass.Position.Y - MaxY, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    pointMass.ApplyForce(new Vector2f(0, -force));
                }
                else if (pointMass.Position.Y < -MaxY)
                {
                    var force = (float)Math.Pow(-MaxY - pointMass.Position.Y, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    pointMass.ApplyForce(new Vector2f(0, force));
                }
            }
        }
    }
}