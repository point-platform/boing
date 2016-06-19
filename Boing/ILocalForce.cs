using System.Collections.Generic;

namespace Boing
{
    // TODO new local force: axis-align force for pairs of point masses
    // TODO new local force: hysteresis spring force

    public interface ILocalForce
    {
        IEnumerable<PointMass> AppliesToPointMasses { get; }

        void Apply();
    }
}