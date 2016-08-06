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
                    if (ReferenceEquals(pointMass1, pointMass2) || pointMass2.IsPinned)
                        continue;

                    var delta = pointMass2.Position - pointMass1.Position;
                    var distance = delta.Norm();

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (distance == 0.0f || distance > MaxDistance)
                        continue;

                    pointMass2.ApplyForce(delta*Repulsion/(distance*distance));
                }
            }
        }
    }
}