[![Boing NuGet version](https://img.shields.io/nuget/v/Boing.svg)](https://www.nuget.org/packages/Boing/)

A simple library for 2D physics simulations in .NET.

## Installation

The easiest way to use this library is via its [NuGet package](https://www.nuget.org/packages/Boing/):

    PM> Install-Package Boing

## Usage

Build a `Simulation` comprising `Node`s (point masses) connected via `Spring`s and forces.

Periodically update the simulation with a time step.

## Example

```csharp
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
```

## License

Copyright 2015-2016 Drew Noakes, Krzysztof Dul

> Licensed under the Apache License, Version 2.0 (the "License");
> you may not use this file except in compliance with the License.
> You may obtain a copy of the License at
>
>     http://www.apache.org/licenses/LICENSE-2.0
>
> Unless required by applicable law or agreed to in writing, software
> distributed under the License is distributed on an "AS IS" BASIS,
> WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
> See the License for the specific language governing permissions and
> limitations under the License.
