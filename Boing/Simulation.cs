#region License

//  Copyright 2015-2017 Drew Noakes, Krzysztof Dul
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

using System;
using System.Collections.Generic;

namespace Boing
{
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

        public float GetTotalKineticEnergy()
        {
            float sum = 0;
            foreach (var pointMass in _pointMasses)
            {
                // 1/2 m v^2
                var speed = pointMass.Velocity.Norm();
                sum += 0.5f*pointMass.Mass*speed*speed;
            }
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
            _pointMasses.Remove(pointMass);
        }

        public void Add(ILocalForce localForce)
        {
            if (!_localForces.Add(localForce))
                throw new ArgumentException("Already exists.", nameof(localForce));
        }

        public void Remove(ILocalForce localForce)
        {
            _localForces.Remove(localForce);
        }
    }
}