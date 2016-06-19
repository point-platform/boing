namespace Boing
{
    /// <summary>
    /// An attraction towards the system's origin.
    /// </summary>
    public sealed class OriginAttractionForce : IGlobalForce
    {
        public float Stiffness { get; set; }

        public OriginAttractionForce(float stiffness = 40)
        {
            Stiffness = stiffness;
        }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                var magnitude = -pointMass.Position.Norm()*Stiffness;

                pointMass.ApplyForce(pointMass.Position.Normalized()*magnitude);
            }
        }
    }
}