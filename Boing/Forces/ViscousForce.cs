namespace Boing
{
    public sealed class ViscousForce : IGlobalForce
    {
        public float Coefficient { get; set; }

        public ViscousForce(float coefficient)
        {
            Coefficient = coefficient;
        }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                pointMass.ApplyForce(pointMass.Velocity*-Coefficient);
            }
        }
    }
}