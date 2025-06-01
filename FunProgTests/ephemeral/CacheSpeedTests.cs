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

namespace FunProgTests.ephemeral;

public class CacheSpeedTests
{
    private static readonly Random RandomNum = new();
    private readonly ITestOutputHelper _testOutputHelper;

    public CacheSpeedTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    // [Ignore]
    // [Fact]
    public void SingleTest()
    {
        var memory = new byte[25];
        const int duration = 1000;  // ms
        var count = RandomAccessMemory(memory, duration, false);
        var time = 1.0e6 * duration / count;
        _testOutputHelper.WriteLine($"Test took {count:n0} times, for an average of {time} nanoseconds");
    }

    private struct TestRun
    {
        public int Index;
        public byte[] Memory;
        public double TimeWith;
        public double TimeWithout;
    }

    // [Ignore]
    // [Fact]
    public void TimeVsBufferSize()
    {
        const int start = 18;
        const int samples = 50;
        const int duration = 333;   // ms

        // setup the data structures before taking measurements.
        var testRuns = new TestRun[samples];
        for (var i = 0; i < testRuns.Length; i++)
        {
            var length = (int) Math.Pow(2.0, (i + start) / 2.5);
            testRuns[i] = new TestRun
            {
                Index = i + start,
                Memory = new byte[length],
                TimeWith = double.MaxValue,
                TimeWithout = double.MaxValue
            };
        }

        // Let the system settle a bit.
        if (testRuns.Length > 20)
            RandomAccessMemory(testRuns[20].Memory, 100, true);
        if (testRuns.Length > 10)
            RandomAccessMemory(testRuns[10].Memory, 100, true);

        // Do each step five times, to find the fastest.
        for (var j = 0; j < 5; j++)
        {
            var indices = Enumerable.Range(0, testRuns.Length).OrderBy(_ => RandomNum.Next());
            foreach (var i in indices)
            {
                // Measure the performance.

                // time = count / duration;
                var count1 = RandomAccessMemory(testRuns[i].Memory, duration, false);
                testRuns[i].TimeWithout = Math.Min(1.0e6 * duration / count1, testRuns[i].TimeWithout);

                var count2 = RandomAccessMemory(testRuns[i].Memory, duration, true);
                testRuns[i].TimeWith = Math.Min(1.0e6 * duration / count2, testRuns[i].TimeWith);
            }
        }

        _testOutputHelper.WriteLine("Test\t         Size\tWithout\t   With\t   Diff");
        foreach (var testRun in testRuns)
        {
            _testOutputHelper.WriteLine(
                $"{testRun.Index,4}\t{testRun.Memory.Length,13:N0}\t{testRun.TimeWithout,7:F2}\t{testRun.TimeWith,7:F2}\t{testRun.TimeWith - testRun.TimeWithout,7:F2}");
        }
    }

    private static long RandomAccessMemory(byte[] memory, long time, bool measure)
    {
        // bring all the memory into the cache.
        for (var i = 0; i < memory.Length; i++)
            memory[i] = (byte) (i & 0xFF);

        // Separate the writes from the reads.
        Interlocked.MemoryBarrier();

        // read the data randomly.
        var count = 0L;
        var watch = Stopwatch.StartNew();
        while (watch.ElapsedMilliseconds < time)
        {
            ++count;
            var index = RandomNum.Next(memory.Length);
            if (!measure) continue;
            _ = memory[index];
        }

        return count;
    }
}