using System;
using System.Diagnostics;

namespace Boing
{
    public sealed class FixedTimeStepUpdater
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly float _timeStepSeconds;
        private readonly Simulation _simulation;
        private float _leftOverTimeSeconds;
        private float _lastTimeSeconds;

        public FixedTimeStepUpdater(Simulation simulation, float timeStepSeconds)
        {
            _simulation = simulation;
            _timeStepSeconds = timeStepSeconds;
        }

        public void Reset()
        {
            _leftOverTimeSeconds = 0;
            _lastTimeSeconds = 0;
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void Update()
        {
            var time = (float)_stopwatch.Elapsed.TotalSeconds;
            var delta = time - _lastTimeSeconds;
            _lastTimeSeconds = time;

            delta += _leftOverTimeSeconds;

            var stepsThisUpdate = Math.Floor(delta/_timeStepSeconds);

            _leftOverTimeSeconds = delta%_timeStepSeconds;

            for (var i = 0; i < stepsThisUpdate; i++)
                _simulation.Update(_timeStepSeconds);
        }
    }
}