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