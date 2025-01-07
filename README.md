This asset recreates the system.random class inside of VRChat with added syncing.

To use this you must make a reference to UdonRandom and assign the Prefab or object with the script.
Inside of a UdonSharpBehaviour you can then call one of the following:

SetSeed - set a new seed for the random to use
GetSeed - returns you the current seed
Next - Returns a non-negative random integer or Returns a random integer that is within a specified range or Returns a non-negative random integer that is less than the specified maximum.
NextDouble - Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.

Why using the system.random class can be useful. Using system.random will let you set a seed for the random numbers, this means if you use the same seed you will get the same random numbers each time
