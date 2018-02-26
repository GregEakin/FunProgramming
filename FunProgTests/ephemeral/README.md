# CPU Access Time
Here's a measurement of CPU access time, when it kept to a given dataset size. 
The idea is when all of the data fits into a given cache level, it can access the data faster.
If the dataset size is larger, then it needs to go to a slower (but larger) cache.

![CPU Memory Access Time](images/CacheSpeedTests.png)

Here, we can see six different slops, as we transition to larger caches.
The curve starts on the left as a flat line, because everything is being pulled from the L1 cache.
Then the curve picks up some slope as the data is being shared between the L1 and L2 caches. 
Here, all the data fits within the L2 cache, but blocks are being copied back into L1, and sometime the next bits of data is still in the L1 cache.


