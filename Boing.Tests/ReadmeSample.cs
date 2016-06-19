using System;
using Boing.Forces;

namespace Boing.Tests
{
    public class ReadmeSample
    {
        public void Code()
        {
            // construct a graph (this is a very simple one)
            var node1 = new Node(mass: 1.0f);
            var node2 = new Node(mass: 2.0f);

            var simulation = new Simulation();
            simulation.Add(node1);
            simulation.Add(node2);
            simulation.Add(new Spring(node1, node2));

            // add various global forces to the simulation
            simulation.Add(new ColoumbForce());                       // nodes are attracted to one another
            simulation.Add(new OriginAttractionForce(stiffness: 10)); // nodes move towards the origin
            simulation.Add(new FlowDownwardForce(magnitude: 100));    // gravity

            // set up a loop
            while (true)
            {
                // compute a time step
                simulation.Update(dt: 0.01f);

                // use the resulting positions somehow
                Console.WriteLine($"Node1 at {node1.Position}, Node2 at {node2.Position}");
            }
        }
    }
}