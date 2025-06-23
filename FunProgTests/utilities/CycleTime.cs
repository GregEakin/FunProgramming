// Copyright 2025 Gregory Eakin <greg@eakin.dev>
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

public interface ICycleTime
{
    ulong Elapsed();
}

public sealed class ThreadCycleTime : ICycleTime, IDisposable
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

public sealed class ProcessCycleTime : ICycleTime, IDisposable
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