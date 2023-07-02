// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunProgramming
// FILE:		BlockingQueue.cs
// AUTHOR:		Greg Eakin

using FunProgLib.queue;

namespace FunProgTests.ephemeral;

internal class BlockingQueue<T>
{
    private readonly object _lock = new object();
    private readonly int _size;
    private readonly CancellationToken _token;
    private RealTimeQueue<T>.Queue _queue = RealTimeQueue<T>.Empty;
    private int _count;

    public BlockingQueue(int size, CancellationToken token)
    {
        _size = size;
        _token = token;

        token.Register(() =>
        {
            lock(_lock)
                Monitor.PulseAll(_lock);
        });
    }

    public bool Enqueue(T t)
    {
        lock (_lock)
        {
            while (!_token.IsCancellationRequested && _count >= _size)
                Monitor.Wait(_lock);

            if (_token.IsCancellationRequested)
                return false;

            _count++;
            _queue = RealTimeQueue<T>.Snoc(_queue, t);

            Monitor.PulseAll(_lock);
        }

        return true;
    }

    public bool Dequeue(out T t)
    {
        t = default(T);

        lock (_lock)
        {
            while (!_token.IsCancellationRequested && RealTimeQueue<T>.IsEmpty(_queue))
                Monitor.Wait(_lock);

            // We can exit, once the queue is empty.
            if (RealTimeQueue<T>.IsEmpty(_queue))
                return false;

            _count--;
            t = RealTimeQueue<T>.Head(_queue);
            _queue = RealTimeQueue<T>.Tail(_queue);

            Monitor.PulseAll(_lock);
        }

        return true;
    }
}

public class BlockingQueueTests
{
    [Fact]
    public void BlockingQueueTest()
    {
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var tasks = new ConcurrentBag<Task>();
        var watch = Stopwatch.StartNew();
        var queue = new BlockingQueue<int>(4, token);

        // Producer
        var producer = Task.Factory.StartNew(() =>
        {
            for (var x = 0; queue.Enqueue(x); x++)
            {
                var threadId = Environment.CurrentManagedThreadId;
                var msg = $"{threadId,3}: {watch.ElapsedMilliseconds,3} {x,4:0000} >";
                Trace.WriteLine(msg);
            }

            Trace.WriteLine($"{Environment.CurrentManagedThreadId,3}: {watch.ElapsedMilliseconds,3} Producer finished");
        }, token);
        tasks.Add(producer);

        // Consumers
        for (var i = 0; i < 2; i++)
        {
            var consumer = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10);
                while (queue.Dequeue(out var x))
                {
                    var threadId = Environment.CurrentManagedThreadId;
                    var msg = $"{threadId,3}: {watch.ElapsedMilliseconds,3}      < {x,4:0000}";
                    Trace.WriteLine(msg);

                    Thread.Sleep(10);
                    //var cancelled = token.WaitHandle.WaitOne(10);
                    //if (cancelled)
                    //    break;
                }

                Trace.WriteLine($"{Environment.CurrentManagedThreadId,3}: {watch.ElapsedMilliseconds,3} Consumer finished");
            }, token);
            tasks.Add(consumer);
        }

        Trace.WriteLine($"{Environment.CurrentManagedThreadId,3}: {watch.ElapsedMilliseconds,3} Stopping after 27 ms");
        tokenSource.CancelAfter(27);

        Task.WaitAll(tasks.ToArray());
    }
}