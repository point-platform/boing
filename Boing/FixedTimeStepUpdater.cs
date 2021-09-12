#region License

//  Copyright 2015-2021 Drew Noakes, Krzysztof Dul
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
using System.Diagnostics;

namespace Boing
{
    /// <summary>
    /// Updates a simulation according to a time scale of fixed repeating interval,
    /// when compared with a stopwatch (wall time).
    /// <para />
    /// Each call to <see cref="Update"/> may update the simulation zero or more times,
    /// </summary>
    public sealed class FixedTimeStepUpdater<TVec>
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly float _timeStepSeconds;
        private readonly long _timeStepTicks;
        private readonly Simulation<TVec> _simulation;
        private long _leftOverTicks;
        private long _lastTicks;

        /// <summary>
        /// Initialises a <see cref="FixedTimeStepUpdater{TVec}"/>.
        /// </summary>
        /// <param name="simulation">The simulation to apply to.</param>
        /// <param name="timeStepSeconds">The number of seconds to </param>
        public FixedTimeStepUpdater(Simulation<TVec> simulation, float timeStepSeconds)
        {
            _simulation = simulation;
            _timeStepSeconds = timeStepSeconds;
            _timeStepTicks = TimeSpan.FromSeconds(timeStepSeconds).Ticks;
        }

        /// <summary>
        /// Reset the current time
        /// </summary>
        public void Reset()
        {
            _leftOverTicks = 0;
            _lastTicks = 0;
            // Note we can't use Stopwatch.Restart here as we target netstandard1.0
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        /// <summary>
        /// Updates the simulations enough times to bring it up to date.
        /// </summary>
        /// <remarks>
        /// If <see cref="Update"/> was called recently enough, it may not actually
        /// update the simulation. It may also call it very many times if it has
        /// been a long time since it last ran.
        /// </remarks>
        public void Update()
        {
            var ticks = _stopwatch.Elapsed.Ticks;
            var delta = ticks - _lastTicks;
            _lastTicks = ticks;

            delta += _leftOverTicks;

            var stepsThisUpdate = delta/_timeStepTicks;

            _leftOverTicks = delta%_timeStepTicks;

            for (var i = 0; i < stepsThisUpdate; i++)
                _simulation.Update(_timeStepSeconds);
        }
    }
}