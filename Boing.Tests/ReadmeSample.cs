using System;
using Boing;

namespace Boing.Tests
{
    public class ReadmeSample
    {
        public void Code()
        {
            // create a new simulation
            var simulation = new Simulation();

            // create some point masses
            var pointMass1 = new PointMass(mass: 1.0f);
            var pointMass2 = new PointMass(mass: 2.0f);

            simulation.Add(pointMass1);
            simulation.Add(pointMass2);

            // create a spring between these point masses
            simulation.Add(new Spring(pointMass1, pointMass2));

            // add some global forces to the simulation
            simulation.Add(new ColoumbForce());                       // point masses are attracted to one another
            simulation.Add(new OriginAttractionForce(stiffness: 10)); // point masses move towards the origin
            simulation.Add(new FlowDownwardForce(magnitude: 100));    // gravity

            // set up a loop
            while (true)
            {
                // compute a time step
                simulation.Update(dt: 0.01f);

                // use the resulting positions somehow
                Console.WriteLine($"PointMass1 at {pointMass1.Position}, PointMass2 at {pointMass2.Position}");
            }
        }
    }
}