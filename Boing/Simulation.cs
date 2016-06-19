using System;
using System.Collections.Generic;

namespace Boing
{
    // TODO axis-align force for pairs of point masses
    // TODO hysteresis spring force

    public sealed class Simulation
    {
        private readonly HashSet<PointMass> _pointMasses = new HashSet<PointMass>();
        private readonly HashSet<ILocalForce> _localForces = new HashSet<ILocalForce>();
        private readonly List<IGlobalForce> _globalForces = new List<IGlobalForce>();

        public IEnumerable<PointMass> PointMasses => _pointMasses;
        public IEnumerable<ILocalForce> LocalForces => _localForces;
        public IEnumerable<IGlobalForce> GlobalForces => _globalForces;

        public void Update(float dt)
        {
            foreach (var force in _globalForces)
                force.ApplyTo(this);

            foreach (var force in _localForces)
                force.Apply();

            foreach (var pointMass in _pointMasses)
                pointMass.Update(dt);
        }

        public float GetTotalEnergy()
        {
            float sum = 0;
            foreach (var pointMass in _pointMasses)
                sum += 0.5f * pointMass.Mass * (float)Math.Pow(pointMass.Velocity.Norm(), 2);
            return sum;
        }

        public void Clear()
        {
            _pointMasses.Clear();
            _localForces.Clear();
            _globalForces.Clear();
        }

        public void Add(IGlobalForce force)
        {
            _globalForces.Add(force);
        }

        public void Add(PointMass pointMass)
        {
            if (!_pointMasses.Add(pointMass))
                throw new ArgumentException("Already exists.", nameof(pointMass));
        }

        public void Remove(PointMass pointMass)
        {
            if (!_pointMasses.Remove(pointMass))
                throw new ArgumentException("Not found.", nameof(pointMass));

            foreach (var localForce in pointMass.LocalForces)
                _localForces.Remove(localForce);
        }

        public void Add(ILocalForce localForce)
        {
            if (!_localForces.Add(localForce))
                throw new ArgumentException("Already exists.", nameof(localForce));

            foreach (var pointMass in localForce.AppliesToPointMasses)
                pointMass.LocalForces.Add(localForce);
        }

        public void Remove(ILocalForce localForce)
        {
            if (!_localForces.Remove(localForce))
                throw new ArgumentException("Not found.", nameof(localForce));

            foreach (var pointMass in localForce.AppliesToPointMasses)
                pointMass.LocalForces.Remove(localForce);
        }
    }
}