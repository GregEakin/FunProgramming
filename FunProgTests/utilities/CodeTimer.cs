// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgTests.utilities
{
    using System;
    using System.Threading;

    public interface IModel
    { }

    public sealed class CodeTimer
    {
        private readonly Thread _thread;
        private readonly IModel _model;
        private readonly Action<IModel> _method;

        private int _collectionCount0;
        private int _collectionCount1;
        private int _collectionCount2;
        private ulong _cpuCycles;

        public CodeTimer(IModel model, Action<IModel> method)
        {
            _thread = new Thread(PerformanceTest);
            _model = model;
            _method = method;

            PrepareForOperation();
        }

        private void PerformanceTest()
        {
            PrepareForOperation();
            var thread = Kernel32.GetCurrentThread();
            var start = Kernel32.QueryThreadCycleTime(thread);
            _method(_model);
            _cpuCycles = Kernel32.QueryThreadCycleTime(thread) - start;
        }

        public CollectionCounters Time()
        {
            _thread.Start();
            _thread.Join();
            return new CollectionCounters(GC.CollectionCount(0) - _collectionCount0,
                GC.CollectionCount(1) - _collectionCount1,
                GC.CollectionCount(2) - _collectionCount2,
                _cpuCycles);
        }

        private void PrepareForOperation()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _collectionCount0 = GC.CollectionCount(0);
            _collectionCount1 = GC.CollectionCount(1);
            _collectionCount2 = GC.CollectionCount(2);
        }
    }
}
