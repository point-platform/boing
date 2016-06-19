namespace Boing.Forces
{
    public sealed class HookeForce : IGlobalForce
    {
        public void ApplyTo(Simulation simulation)
        {
            foreach (var spring in simulation.Springs)
            {
                var source = spring.Source;
                var target = spring.Target;

                var delta = target.Position - source.Position;
                var displacement = spring.Length - delta.Norm();
                var direction = delta.Normalized();

                if (!source.IsPinned && !target.IsPinned)
                {
                    source.ApplyForce(direction*(spring.K*displacement*-0.5f));
                    target.ApplyForce(direction*(spring.K*displacement*0.5f));
                }
                else if (source.IsPinned && !target.IsPinned)
                {
                    target.ApplyForce(direction*(spring.K*displacement));
                }
                else if (!source.IsPinned && target.IsPinned)
                {
                    source.ApplyForce(direction*(spring.K*-displacement));
                }
            }
        }
    }
}