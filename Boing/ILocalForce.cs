using System.Collections.Generic;

namespace Boing
{
    public interface ILocalForce
    {
        IEnumerable<Node> AppliesToNodes { get; }

        void Apply();
    }
}