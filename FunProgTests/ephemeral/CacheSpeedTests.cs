using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.ephemeral
{
    [TestClass]
    public class CacheSpeedTests
    {
        private static readonly Random RandomNum = new Random();

        [TestMethod]
        public void SingleTest()
        {
            var memory = new byte[25];
            var count = RandomAccessMemory(memory, 1000);
            Console.WriteLine($"Test took {count:n0}");
        }

        [TestMethod]
        public void TimeVsBufferSize()
        {
            const int start = 18;
            const int samples = 50;
            const int duration = 333;

            // setup the data structers before taking measurments.
            var memory = new byte[samples][];
            for (var i = 0; i < memory.Length; i++)
            {
                var length = (int)Math.Pow(2.0, (i + start) / 2.5);
                memory[i] = new byte[length];
            }

            // Let the system settle a bit
            RandomAccessMemory(memory[20], 100);
            RandomAccessMemory(memory[10], 100);

            // Measrue the performance.
            Console.WriteLine("Test\tSize\tTime");
            for (var i = 0; i < memory.Length; i++)
            {
                // Do each step three times, to find the fastest
                var time = double.MaxValue;
                for (var j = 0; j < 3; j++)
                {
                    var count = RandomAccessMemory(memory[i], duration);
                    time = Math.Min(1.0e6 * duration / count, time);
                }
                Console.WriteLine($"{i}\t{memory[i].Length}\t{time}");
            }
        }

        private static int RandomAccessMemory(IList<byte> memory, long time)
        {
            // bring all the memory into the cache.
            for (var i = 0; i < memory.Count; i++)
                memory[i] = (byte)(i & 0xFF);

            // flush the cach to main memory
            Interlocked.MemoryBarrier();

            // read the data randomly
            var count = 0;
            var watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < time)
            {
                var index = RandomNum.Next(memory.Count);
                var data = memory[index];
                ++count;
            }
            return count;
        }
    }
}