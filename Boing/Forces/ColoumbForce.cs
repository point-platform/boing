// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Boing.Forces
{
    public sealed class ColoumbForce : IGlobalForce
    {
        public ColoumbForce(float repulsion = 20000)
        {
            Repulsion = repulsion;
        }

        public float Repulsion { get; set; }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var pointMass1 in simulation.PointMasses)
            {
                foreach (var pointMass2 in simulation.PointMasses)
                {
                    if (ReferenceEquals(pointMass1, pointMass2))
                        continue;

                    var delta = pointMass1.Position - pointMass2.Position;
                    var distance = delta.Norm();

                    if (distance == 0.0f)
                        continue;

                    var direction = delta.Normalized();

                    if (!pointMass1.IsPinned && !pointMass2.IsPinned)
                    {
                        var force = direction*Repulsion/(distance*0.5f);
                        pointMass1.ApplyForce(force);
                        pointMass2.ApplyForce(force*-1);
                    }
                    else if (pointMass1.IsPinned && !pointMass2.IsPinned)
                    {
                        pointMass2.ApplyForce(direction*Repulsion/-distance);
                    }
                    else if (!pointMass1.IsPinned && pointMass2.IsPinned)
                    {
                        pointMass1.ApplyForce(direction*Repulsion/distance);
                    }
                }
            }
        }
    }
}