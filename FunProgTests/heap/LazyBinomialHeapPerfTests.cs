// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgTests.heap
{
    using FunProgLib.heap;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using utilities;

    [TestClass]
    public class LazyBinomialHeapPerfTests
    {
        [TestMethod]
        public void PerfTest1()
        {
            var modelSimulator = new ModelSimulator();
            var codeTimer = new CodeTimer(modelSimulator, modelSimulator.RunModel);
            var time = codeTimer.Time();
            Console.WriteLine("{0:#,##0} Cycles", time.CpuCycles);
            Console.WriteLine("{0:#,##0} GC0", time.CollectionCount0);
            Console.WriteLine("{0:#,##0} GC1", time.CollectionCount1);
            Console.WriteLine("{0:#,##0} GC2", time.CollectionCount2);
        }

        private class ModelSimulator : IModel
        {
            private const int Size = 2000;
            private readonly Random _random = new Random(4432);

            public void RunModel(IModel obj)
            {
                var heap = LazyBinomialHeap<int>.Empty;
                for (var i = 0; i < Size; i++)
                {
                    var val = _random.Next(Size);
                    heap = LazyBinomialHeap<int>.Insert(val, heap);
                    Assert.IsFalse(heap.IsValueCreated);
                }

                Console.WriteLine(LazyBinomialHeapTests.DumpHeap(heap));

                var last = 0;
                var count = 0;
                while (!LazyBinomialHeap<int>.IsEmpty(heap))
                {
                    Assert.IsTrue(heap.IsValueCreated);

                    var next = LazyBinomialHeap<int>.FindMin(heap);
                    Assert.IsTrue(last <= next);
                    last = next;

                    heap = LazyBinomialHeap<int>.DeleteMin(heap);
                    count++;
                }

                Assert.AreEqual(Size, count);
            }
        }
    }
}
