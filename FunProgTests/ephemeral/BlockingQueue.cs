// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunProgramming
// FILE:		BlockingQueue.cs
// AUTHOR:		Greg Eakin

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
        private int _count;
        private RealTimeQueue<T>.Queue _queue = RealTimeQueue<T>.Empty;
        private bool _quit;

        public BlockingQueue(int size)
        {
            _size = size;
        }

        public void Quit()
        {
            lock (_lock)
            {
                _quit = true;
                Monitor.PulseAll(_lock);
            }
        }

        public bool Enqueue(T t)
        {
            lock (_lock)
            {
                while (!_quit && _count >= _size) Monitor.Wait(_lock);

                if (_quit) return false;

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
                while (!_quit && RealTimeQueue<T>.IsEmpty(_queue)) Monitor.Wait(_lock);

                if (RealTimeQueue<T>.IsEmpty(_queue)) return false;

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
        // [TestMethod]
        public void BlockingQueueTest()
        {
            var tasks = new System.Collections.Generic.List<Task>();
            var q = new BlockingQueue<int>(4);
            var watch = Stopwatch.StartNew();

            // Producer
            var producer = new Task(() =>
            {
                for (var x = 0; q.Enqueue(x); x++)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    var msg = $"{threadId,3}: {watch.ElapsedMilliseconds,3} {x,4:0000} >";
                    Trace.WriteLine(msg);
                }

                Trace.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3}: Producer finished");
            });
            producer.Start();
            tasks.Add(producer);

            // Consumers
            for (var i = 0; i < 2; i++)
            {
                var consumer = new Task(() =>
                {
                    Thread.Sleep(10);
                    while (q.Dequeue(out var x))
                    {
                        var threadId = Thread.CurrentThread.ManagedThreadId;
                        var msg = $"{threadId,3}: {watch.ElapsedMilliseconds,3}      < {x,4:0000}";
                        Trace.WriteLine(msg);
                        Thread.Sleep(10);
                    }

                    Trace.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3}: Consumer finished");
                });
                consumer.Start();
                tasks.Add(consumer);
            }

            Thread.Sleep(100);

            Trace.WriteLine($"{Thread.CurrentThread.ManagedThreadId,3}: Stopping after 100 ms");

            q.Quit();

            Task.WaitAll(tasks.ToArray());
        }
    }
}
