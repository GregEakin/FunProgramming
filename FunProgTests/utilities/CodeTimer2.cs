using System;
using System.Threading.Tasks;

namespace FunProgTests.utilities
{
    //public interface IModel
    //{ }

    public sealed class CodeTimer2
    {
        private readonly Task _task;
        private readonly IModel _model;
        private readonly Action<IModel> _method;

        private int _collectionCount0;
        private int _collectionCount1;
        private int _collectionCount2;
        private ulong _cpuCycles;


        public CodeTimer2(IModel model, Action<IModel> method)
        {
            _model = model;
            _method = method;
            _task = new Task(PerformanceTest);
        }

        public CollectionCounters Time()
        {
            _task.RunSynchronously();

            return new CollectionCounters(GC.CollectionCount(0) - _collectionCount0,
                GC.CollectionCount(1) - _collectionCount1,
                GC.CollectionCount(2) - _collectionCount2,
                _cpuCycles);
        }

        private void PerformanceTest()
        {
            try
            {
                PrepareForOperation();
                var thread = Kernel32.GetCurrentThread();
                var start = Kernel32.QueryThreadCycleTime(thread);
                _method(_model);
                _cpuCycles = Kernel32.QueryThreadCycleTime(thread) - start;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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