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
using System.Diagnostics;

namespace Boing
{
    public sealed class FixedTimeStepUpdater
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly float _timeStepSeconds;
        private readonly long _timeStepTicks;
        private readonly Simulation _simulation;
        private long _leftOverTicks;
        private long _lastTicks;

        public FixedTimeStepUpdater(Simulation simulation, float timeStepSeconds)
        {
            _simulation = simulation;
            _timeStepSeconds = timeStepSeconds;
            _timeStepTicks = TimeSpan.FromSeconds(timeStepSeconds).Ticks;
        }

        public void Reset()
        {
            _leftOverTicks = 0;
            _lastTicks = 0;
            _stopwatch.Reset();
            _stopwatch.Start();
        }

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