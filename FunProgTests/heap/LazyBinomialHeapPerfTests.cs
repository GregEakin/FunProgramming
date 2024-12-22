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

using FunProgLib.heap;
using FunProgTests.utilities;
using Xunit.Abstractions;

namespace FunProgTests.heap;

public class LazyBinomialHeapPerfTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public LazyBinomialHeapPerfTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void PerfTest1()
    {
        var modelSimulator = new ModelSimulator(_testOutputHelper);
        var codeTimer = new CodeTimer2(modelSimulator, modelSimulator.RunModel);
        var time = codeTimer.Time();
        _testOutputHelper.WriteLine("{0:#,##0} Cycles", time.CpuCycles);
        _testOutputHelper.WriteLine("{0:#,##0} GC0", time.CollectionCount0);
        _testOutputHelper.WriteLine("{0:#,##0} GC1", time.CollectionCount1);
        _testOutputHelper.WriteLine("{0:#,##0} GC2", time.CollectionCount2);
    }

    private class ModelSimulator : IModel
    {
        private const int Size = 200;
        private readonly Random _random = new Random(4432);
        private readonly ITestOutputHelper _testOutputHelper;

        public ModelSimulator(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public void RunModel(IModel _)
        {
            var heap = LazyBinomialHeap<int>.Empty;
            for (var i = 0; i < Size; i++)
            {
                var val = _random.Next(Size);
                heap = LazyBinomialHeap<int>.Insert(val, heap);
                Assert.False(heap.IsValueCreated);
            }

            _testOutputHelper.WriteLine(LazyBinomialHeapTests.DumpHeap(heap, true));

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