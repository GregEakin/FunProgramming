// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunProgramming
// FILE:		BlockingQueue.cs
// AUTHOR:		Greg Eakin

using System.Collections.Concurrent;

namespace FunProgTests.ephemeral
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using FunProgLib.queue;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    [TestClass]
    public class BlockingQueueTests
    {
        [TestMethod]
        public void BlockingQueueTest()
        {
            var tasks = new ConcurrentBag<Task>();
            var watch = Stopwatch.StartNew();

            using (var tokenSource = new CancellationTokenSource())
            {
                var queue = new BlockingQueue<int>(4, tokenSource.Token);

                // Producer
                var producer = Task.Factory.StartNew(() =>
                {
                    for (var x = 0; queue.Enqueue(x); x++)
                    {
                        var threadId = Thread.CurrentThread.ManagedThreadId;
                        var msg = $"{threadId,3}: {watch.ElapsedMilliseconds,3} {x,4:0000} >";
                        Trace.WriteLine(msg);
                    }

                    Trace.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3}: {watch.ElapsedMilliseconds,3} Producer finished");
                }, tokenSource.Token);
                tasks.Add(producer);

                // Consumers
                for (var i = 0; i < 2; i++)
                {
                    var consumer = Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(10);
                        while (queue.Dequeue(out var x))
                        {
                            var threadId = Thread.CurrentThread.ManagedThreadId;
                            var msg = $"{threadId,3}: {watch.ElapsedMilliseconds,3}      < {x,4:0000}";
                            Trace.WriteLine(msg);
                            Thread.Sleep(10);
                        }

                        Trace.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3}: {watch.ElapsedMilliseconds,3} Consumer finished");
                    }, tokenSource.Token);
                    tasks.Add(consumer);
                }

                Trace.WriteLine(
                    $"{Thread.CurrentThread.ManagedThreadId,3}: {watch.ElapsedMilliseconds,3} Stopping after 27 ms");
                tokenSource.CancelAfter(27);

                Task.WaitAll(tasks.ToArray());
            }
        }
    }
}