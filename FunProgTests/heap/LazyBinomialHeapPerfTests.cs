// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.heap;
using FunProgTests.utilities;

namespace FunProgTests.heap;

public class LazyBinomialHeapPerfTests
{
    [Fact]
    public void PerfTest1()
    {
        var modelSimulator = new ModelSimulator();
        var codeTimer = new CodeTimer2(modelSimulator, modelSimulator.RunModel);
        var time = codeTimer.Time();
        Console.WriteLine("{0:#,##0} Cycles", time.CpuCycles);
        Console.WriteLine("{0:#,##0} GC0", time.CollectionCount0);
        Console.WriteLine("{0:#,##0} GC1", time.CollectionCount1);
        Console.WriteLine("{0:#,##0} GC2", time.CollectionCount2);
    }

    private class ModelSimulator : IModel
    {
        private const int Size = 200;
        private readonly Random _random = new Random(4432);

        public void RunModel(IModel _)
        {
            var heap = LazyBinomialHeap<int>.Empty;
            for (var i = 0; i < Size; i++)
            {
                var val = _random.Next(Size);
                heap = LazyBinomialHeap<int>.Insert(val, heap);
                Assert.False(heap.IsValueCreated);
            }

            Console.WriteLine(LazyBinomialHeapTests.DumpHeap(heap, true));

            var last = 0;
            var count = 0;
            while (!LazyBinomialHeap<int>.IsEmpty(heap))
            {
                Assert.True(heap.IsValueCreated);

                var next = LazyBinomialHeap<int>.FindMin(heap);
                Assert.True(last <= next);
                last = next;

                heap = LazyBinomialHeap<int>.DeleteMin(heap);
                count++;
            }

            Assert.Equal(Size, count);
        }
    }
}