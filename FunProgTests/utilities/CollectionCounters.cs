// Copyright 2014 Gregory Eakin <greg@eakin.dev>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace FunProgTests.utilities;

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