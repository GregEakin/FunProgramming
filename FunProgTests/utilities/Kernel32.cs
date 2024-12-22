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

using Microsoft.Win32.SafeHandles;

namespace FunProgTests.utilities;

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