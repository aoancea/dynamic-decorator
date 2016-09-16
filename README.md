[![Build Status](https://travis-ci.org/aoancea/dynamic-decorator.svg?branch=master)](https://travis-ci.org/aoancea/dynamic-decorator)
[![Build status](https://ci.appveyor.com/api/projects/status/xstw78sygkscl1vk?svg=true)](https://ci.appveyor.com/project/aoancea/dynamic-decorator)

# dynamic-decorator

This is an attempt to simplify the work with decorators in such a way that you won't have to write the whole class implementation but have it generated for you. A decorator has almost 90% of the code identical to the decorated class, so it does make sense to not write it yourself.

For more information please read about the Decorator pattern [here](https://en.wikipedia.org/wiki/Decorator_pattern)


## Main uses
 * Tracing
    * Log *inputs* and *outputs*
    * Log the time it took to perform an operation
 * Caching
 * Transaction
    * [Transaction wrapper](https://github.com/aoancea/dynamic-decorator/tree/master/src/UnitTesting/Playground/AmbientTransaction)
    * Transaction re-trier

## DI Containers 
 * Unity
 * Simple Injector
 * Castle Windsor

## Nuget

## Contribute & Develop
Here you can find a document with the [contribution guidelines](https://github.com/aoancea/dynamic-decorator/blob/master/CONTRIBUTING.md)
