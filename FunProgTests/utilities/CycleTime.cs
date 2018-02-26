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
    using Microsoft.Win32.SafeHandles;

    public interface ICycleTime
    {
        ulong Elapsed();
    }

    public sealed class ThreadCycleTime : ICycleTime
    {
        private readonly SafeWaitHandle _handle;
        private readonly ulong _startTime;

        public ThreadCycleTime(SafeWaitHandle handle)
        {
            _handle = handle;
            _startTime = Kernel32.QueryThreadCycleTime();
        }

        public ulong Elapsed()
        {
            var now = Kernel32.QueryThreadCycleTime();
            return now - _startTime;
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }

    public sealed class ProcessCycleTime : ICycleTime
    {
        private readonly SafeWaitHandle _handle;
        private readonly ulong _startTime;

        public ProcessCycleTime(SafeWaitHandle handle)
        {
            _handle = handle;
            _startTime = Kernel32.QueryProcessCycleTime(_handle);
        }

        public ulong Elapsed()
        {
            var now = Kernel32.QueryProcessCycleTime(_handle);
            return now - _startTime;
        }

        public void Dispose()
        {
            _handle.Dispose();
        }
    }
}
