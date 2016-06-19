using System;
using Boing.Forces;

namespace Boing.Tests
{
    public class ReadmeSample
    {
        public void Code()
        {
            // construct a graph (this is a very simple one)
            var node1 = new Node("Node1", mass: 1.0f);
            var node2 = new Node("Node2", mass: 2.0f);

            var simulation = new Simulation();
            simulation.Add(node1);
            simulation.Add(node2);
            simulation.Add(new Edge("Edge1", node1, node2));

            // add various forces to the universe
            simulation.Add(new ColoumbForce());                       // nodes are attracted
            simulation.Add(new HookeForce());                         // edges are springs
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