![boing logo](https://cdn.rawgit.com/drewnoakes/boing/master/Resources/logo.svg)


[![Build status](https://ci.appveyor.com/api/projects/status/xsovru9f2mmib616?svg=true)](https://ci.appveyor.com/project/drewnoakes/boing)
[![Boing NuGet version](https://img.shields.io/nuget/v/Boing.svg)](https://www.nuget.org/packages/Boing/)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

A simple library for 2D physics simulations in .NET.

## Installation

The easiest way to use this library is via its [NuGet package](https://www.nuget.org/packages/Boing/):

    PM> Install-Package Boing

Boing supports `net35` (.NET Framework 3.5 and above) and `netstandard1.0` (.NET Standard 1.0 and above) for .NET Core and other platforms.

## Usage

Build a `Simulation` comprising:

- `PointMass` objects
- Forces such as springs, gravity, Coloumb, viscosity

Periodically update the simulation with a time step.

## Example

```csharp
// create some point masses
var pointMass1 = new PointMass(mass: 1.0f);
var pointMass2 = new PointMass(mass: 2.0f);

// create a new simulation
var simulation = new Simulation
{
    // add the point masses
    pointMass1,
    pointMass2,

    // create a spring between these point masses
    new Spring(pointMass1, pointMass2, length: 20),

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

        // TODO use the resulting positions somehow
        Console.WriteLine($"PointMass1 at {pointMass1.Position}, PointMass2 at {pointMass2.Position}");
    }
}

async Task RunAtEvenRateAsync(CancellationToken token)
{
    var updater = new FixedTimeStepUpdater(simulation, timeStepSeconds: 1f/200);

    while (!token.IsCancellationRequested)
    {
        updater.Update();

        // TODO use the resulting positions somehow
        Console.WriteLine($"PointMass1 at {pointMass1.Position}, PointMass2 at {pointMass2.Position}");

        await Task.Delay(millisecondsDelay: 1/60, cancellationToken: token);
    }
}

```
