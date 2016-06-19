namespace Boing.Forces
{
    public sealed class HookeForce : IGlobalForce
    {
        public void ApplyTo(Simulation simulation)
        {
            foreach (var edge in simulation.Edges)
            {
                var source = edge.Source;
                var target = edge.Target;

                var delta = target.Position - source.Position;
                var displacement = edge.Length - delta.Norm();
                var direction = delta.Normalized();

                if (!source.IsPinned && !target.IsPinned)
                {
                    source.ApplyForce(direction*(edge.K*displacement*-0.5f));
                    target.ApplyForce(direction*(edge.K*displacement*0.5f));
                }
                else if (source.IsPinned && !target.IsPinned)
                {
                    target.ApplyForce(direction*(edge.K*displacement));
                }
                else if (!source.IsPinned && target.IsPinned)
                {
                    source.ApplyForce(direction*(edge.K*-displacement));
                }
            }
        }
    }
}