namespace Boing
{
    public sealed class FlowDownwardForce : IGlobalForce
    {
        /// <summary>
        /// The magnitude of the downward force, in Newtons.
        /// </summary>
        public float Magnitude { get; set; }

        /// <param name="magnitude">The magnitude of the downward force, in Newtons.</param>
        public FlowDownwardForce(float magnitude = 10.0f)
        {
            Magnitude = magnitude;
        }

        public void ApplyTo(Simulation simulation)
        {
            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                pointMass.ApplyForce(new Vector2f(0, Magnitude));
            }
        }
    }
}