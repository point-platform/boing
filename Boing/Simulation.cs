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
using System.Collections;
using System.Collections.Generic;

namespace Boing
{
    public sealed class Simulation : IEnumerable
    {
        private readonly HashSet<PointMass> _pointMasses = new HashSet<PointMass>();
        private readonly HashSet<IForce> _forces = new HashSet<IForce>();

        public IEnumerable<PointMass> PointMasses => _pointMasses;
        public IEnumerable<IForce> Forces => _forces;

        public void Update(float dt)
        {
            foreach (var force in _forces)
                force.ApplyTo(this);

            foreach (var pointMass in _pointMasses)
                pointMass.Update(dt);
        }

        public float GetTotalKineticEnergy()
        {
            float sum = 0;
            // ReSharper disable once LoopCanBeConvertedToQuery
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
            _forces.Clear();
        }

        public void Add(IForce force)
        {
            if (!_forces.Add(force))
                throw new ArgumentException("Already exists.", nameof(force));
        }

        public void Remove(IForce force)
        {
            _forces.Remove(force);
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException($"{nameof(Simulation)} only implements {nameof(IEnumerable)} to enable C# object initialisers.");
        }
    }
}