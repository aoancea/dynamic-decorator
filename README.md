# dynamic-decorator

This is an attempt to simplify the work with decorators in such a way that you won't have to write the whole class implementation but have it generated for you. A decorator has almost 90% of the code identical to the decorated class, so it does make sense to not write it yourself.

For more information please read about the Decorator pattern [here](https://en.wikipedia.org/wiki/Decorator_pattern)


## Main branches of use
 * Tracing
    * Log *inputs* and *outputs*
    * Log the time it took to perform an operation
 * Caching
