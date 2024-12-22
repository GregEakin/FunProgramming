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
            throw;
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