# DVKeylessEntry

This mod for Derail Valley adds keyless entry to the two diesel powered locomotives.

## Turning on a train

The engine is turned on as soon as you enter a train by any means,
be it teleporting, climbing, or falling down from the sky onto the cabin

## Turning off a train

A train is turned off as soon as you leave it.
Using ~~a bunch of `if` statements~~ advanced AI,
it figures out if it should turn off the engine or not.

Currently the engine is **not** turned off if **any** of the following conditions are met:

- There are cars coupled to this loco
- There are cars in coupling range
- The loco is moving with at least 1 Km/H
- The independent brake is not at least 90% applied
- The loco is connected to a remote controller
