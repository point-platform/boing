using System.Collections.Generic;

namespace Boing
{
    public interface ILocalForce
    {
        IEnumerable<PointMass> AppliesToPointMasses { get; }

        void Apply();
    }
}