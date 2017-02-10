// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace FunProgTests.ephemeral
{
    [TestClass]
    public class CacheSpeedTests
    {
        private static readonly Random RandomNum = new Random();

        // [Ignore]
        // [TestMethod]
        public void SingleTest()
        {
            var memory = new byte[25];
            const int duration = 1000;  // ms
            var count = RandomAccessMemory(memory, duration, false);
            var time = 1.0e6 * duration / count;
            Console.WriteLine($"Test took {count:n0} times, for an average of {time} nanoseconds");
        }

        // [Ignore]
        // [TestMethod]
        public void TimeVsBufferSize()
        {
            const int start = 18;
            const int samples = 50;
            const int duration = 333;   // ms

            // setup the data structures before taking measurements.
            var memory = new byte[samples][];
            for (var i = 0; i < memory.Length; i++)
            {
                var length = (int)Math.Pow(2.0, (i + start) / 2.5);
                memory[i] = new byte[length];
            }

            // Let the system settle a bit.
            RandomAccessMemory(memory[20], 100, true);
            RandomAccessMemory(memory[10], 100, true);

            // Measure the performance.
            Console.WriteLine("Test\tSize\tWithout\tWith");
            for (var i = 0; i < memory.Length; i++)
            {
                // Do each step three times, to find the fastest.
                var time1 = double.MaxValue;
                var time2 = double.MaxValue;
                for (var j = 0; j < 3; j++)
                {
                    var count1 = RandomAccessMemory(memory[i], duration, false);
                    time1 = Math.Min(1.0e6 * duration / count1, time1);

                    var count2 = RandomAccessMemory(memory[i], duration, true);
                    time2 = Math.Min(1.0e6 * duration / count2, time2);
                }
                Console.WriteLine($"{i}\t{memory[i].Length}\t{time1}\t{time2}");
            }
        }

        private static int RandomAccessMemory(IList<byte> memory, long time, bool measure)
        {
            // bring all the memory into the cache.
            for (var i = 0; i < memory.Count; i++)
                memory[i] = (byte)(i & 0xFF);

            // Separate the writes from the reads.
            Interlocked.MemoryBarrier();

            // read the data randomly.
            var count = 0;
            var watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < time)
            {
                ++count;
                var index = RandomNum.Next(memory.Count);
                if (!measure) continue;
                var data = memory[index];
            }
            return count;
        }
    }
}
