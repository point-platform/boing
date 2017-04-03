![boing logo](https://cdn.rawgit.com/drewnoakes/boing/master/Resources/logo.svg)


[![Build status](https://ci.appveyor.com/api/projects/status/xsovru9f2mmib616?svg=true)](https://ci.appveyor.com/project/drewnoakes/boing)
[![Boing NuGet version](https://img.shields.io/nuget/v/Boing.svg)](https://www.nuget.org/packages/Boing/)
[![Boing NuGet prerelease version](https://img.shields.io/nuget/vpre/Boing.svg)](https://www.nuget.org/packages/Boing/)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

A simple library for 2D physics simulations in .NET.

## Installation

The easiest way to use this library is via its [NuGet package](https://www.nuget.org/packages/Boing/):

    PM> Install-Package Boing

Boing supports `net35` (.NET Framework 3.5 and above) and `netstandard1.0` (.NET Standard 1.0 and above) for .NET Core and other platforms.

## Usage

Build a `Simulation` comprising:

- `PointMass` objects
- Local forces (such as `Spring`s) 
- Global forces (such as gravity or Coloumb forces)

Periodically update the simulation with a time step.

## Example

```csharp
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
simulation.Add(new ColoumbForce());                       // point masses repel one another
simulation.Add(new OriginAttractionForce(stiffness: 10)); // point masses move towards the origin
simulation.Add(new FlowDownwardForce(magnitude: 100));    // gravity
simulation.Add(new ViscousForce());                       // some resistance to motion

// set up a loop
while (true)
{
    // compute a time step
    simulation.Update(dt: 0.01f);

    // use the resulting positions somehow
    Console.WriteLine($"PointMass1 at {pointMass1.Position}, PointMass2 at {pointMass2.Position}");
}
```
