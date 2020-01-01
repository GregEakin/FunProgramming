// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

namespace FunProgTests.utilities
{
    public class CollectionCounters
    {
        public int CollectionCount0 { get; }
        public int CollectionCount1 { get; }
        public int CollectionCount2 { get; }
        public ulong CpuCycles { get; }

        public CollectionCounters(int collectionCount0, int collectionCount1, int collectionCount2, ulong cpuCycles)
        {
            CollectionCount0 = collectionCount0;
            CollectionCount1 = collectionCount1;
            CollectionCount2 = collectionCount2;
            CpuCycles = cpuCycles;
        }
    }
}
