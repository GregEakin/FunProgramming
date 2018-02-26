# CPU Access Time
Here's a measurement of CPU access time, when it kept to a given dataset size. 
The idea is when all of the data fits into a given cache level, it can access the data faster.
If the dataset size is larger, then it needs to go to a slower (but larger) cache.

![CPU Memory Access Time](images/CacheSpeedTests.png)
![CPU Memory Access Time](https://raw.githubusercontent.com/GregEakin/FunProgramming/master/FunProgTests/ephemeral/images/CacheSpeedTest.png)

Here, we can see five different slops, as we transition to larger caches.
The curve starts on the left as a flat line, because everything is being pulled from the L1 cache.
Then the curve picks up some slope as the data is being shared between the L1 and L2 caches. 
Here, all the data fits within the L2 cache, but as blocks are being copied from L2 down into L1, sometimes the next needed bits are already in the L1 cache.

## Measurement Code:
This graph was measured by using `byte[]` of various sizes.
Where it counted memory accesses, in a given amount of `time`.
To subtract the overhead of the random number generator, we run the test twice: one with and one without the memory access.
```C#
// read the data randomly.
var count = 0L;
var watch = Stopwatch.StartNew();
while (watch.ElapsedMilliseconds < time)
{
    ++count;
    var index = RandomNum.Next(memory.Length);
    if (!measure) continue;
    var data = memory[index];
}

return count;
```
