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
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Boing.Tests
{
    public static class ReadmeSample
    {
        public static void Code()
        {
            // create some point masses
            var pointMass1 = new PointMass2(mass: 1.0f);
            var pointMass2 = new PointMass2(mass: 2.0f);

            // create a new simulation
            var simulation = new Simulation<Vector2>
            {
                // add the point masses
                pointMass1,
                pointMass2,

                // create a spring between these point masses
                new Spring2(pointMass1, pointMass2, length: 20),

                // point masses are attracted to one another
                new ColoumbForce(),

                // point masses move towards the origin
                new OriginAttractionForce(stiffness: 10),

                // gravity
                new FlowDownwardForce(magnitude: 100)
            };

            void RunAtMaxSpeed()
            {
                while (true)
                {
                    // update the simulation
                    simulation.Update(dt: 0.01f);

                    // use the resulting positions somehow
                    Console.WriteLine($"PointMass1 at {pointMass1.Position}, PointMass2 at {pointMass2.Position}");
                }
            }

            async Task RunAtFixedRateAsync(CancellationToken token)
            {
                var updater = new FixedTimeStepUpdater<Vector2>(simulation, timeStepSeconds: 1f/200);

                while (!token.IsCancellationRequested)
                {
                    updater.Update();

                    // TODO render frame

                    await Task.Delay(millisecondsDelay: 1/60, cancellationToken: token);
                }
            }

            // Reference methods to remove IDE warnings
            RunAtMaxSpeed();
            RunAtFixedRateAsync(CancellationToken.None).Wait();
        }
    }
}