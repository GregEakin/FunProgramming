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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public static class Kernel32
    {
        public static ulong QueryThreadCycleTime()
        {
            var succeeded = QueryThreadCycleTime((IntPtr)(-2), out ulong cycleTime);
            if (!succeeded)
                throw new Win32Exception();
            return cycleTime;
        }

        public static ulong QueryThreadCycleTime(IntPtr handle)
        {
            var succeeded = QueryThreadCycleTime(handle, out ulong cycleTime);
            if (!succeeded)
                throw new Win32Exception();
            return cycleTime;
        }

        public static ulong QueryThreadCycleTime(SafeWaitHandle handle)
        {
            var succeeded = QueryThreadCycleTime(handle, out ulong cycleTime);
            if (!succeeded)
                throw new Win32Exception();
            return cycleTime;
        }

        public static ulong QueryProcessCycleTime(SafeWaitHandle handle)
        {
            var succeeded = QueryProcessCycleTime(handle, out ulong cycleTime);
            if (!succeeded)
                throw new Win32Exception();
            return cycleTime;
        }

        public static IEnumerable<ulong> QueryIdleProcessorCycleTime()
        {
            var processorCount = Environment.ProcessorCount;
            var bufferSize = processorCount * sizeof(ulong);
            var buffer = new ulong[processorCount];
            var succeeded = QueryIdleProcessorCycleTime(ref bufferSize, buffer);
            if (!succeeded)
                throw new Win32Exception();
            return buffer;
        }

        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QueryThreadCycleTime(IntPtr threadHandle, out ulong cycleTime);

        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QueryThreadCycleTime(SafeWaitHandle threadHandle, out ulong cycleTime);

        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QueryProcessCycleTime(SafeWaitHandle processHandle, out ulong cycleTime);

        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QueryIdleProcessorCycleTime(ref int bufferLength, ulong[] processorIdleCycleTime);

        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetCurrentThread();
    }
}
