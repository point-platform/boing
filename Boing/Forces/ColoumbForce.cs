namespace Boing
{
    public sealed class ColoumbForce : IGlobalForce
    {
        public ColoumbForce(float repulsion = 20000, float maxDistance = float.MaxValue)
        {
            Repulsion = repulsion;
            MaxDistance = maxDistance;
        }

        public float Repulsion { get; set; }
        public float MaxDistance { get; set; }

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

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (distance == 0.0f || distance > MaxDistance)
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