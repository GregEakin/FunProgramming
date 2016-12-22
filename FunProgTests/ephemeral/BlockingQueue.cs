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

    class BlockingQueue<T>
    {
        private readonly object _key = new object();
        private readonly int _size;
        private int _count;
        private volatile RealTimeQueue<T>.Queue _queue = RealTimeQueue<T>.Empty;
        private volatile bool _quit;

        public BlockingQueue(int size)
        {
            _size = size;
        }

        public void Quit()
        {
            lock (_key)
            {
                _quit = true;
                Monitor.PulseAll(_key);
            }
        }

        public bool Enqueue(T t)
        {
            lock (_key)
            {
                while (!_quit && _count >= _size) Monitor.Wait(_key);

                if (_quit) return false;

                Interlocked.Increment(ref _count);
                _queue = RealTimeQueue<T>.Snoc(_queue, t);

                Monitor.PulseAll(_key);
            }

            return true;
        }

        public bool Dequeue(out T t)
        {
            t = default(T);

            lock (_key)
            {
                while (!_quit && RealTimeQueue<T>.IsEmpty(_queue)) Monitor.Wait(_key);

                if (RealTimeQueue<T>.IsEmpty(_queue)) return false;

                Interlocked.Decrement(ref _count);
                t = RealTimeQueue<T>.Head(_queue);
                _queue = RealTimeQueue<T>.Tail(_queue);

                Monitor.PulseAll(_key);
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
            var watch = new Stopwatch();
            watch.Start();

            // Producer
            var producer = new Task(() =>
            {
                for (var x = 0; ; x++)
                {
                    if (!q.Enqueue(x)) break;
                    var msg = $"{Thread.CurrentThread.ManagedThreadId,3}: {watch.Elapsed,14} {x,4:0000} >";
                    Trace.WriteLine(msg);
                }
                Trace.WriteLine("Producer quitting");
            });
            producer.Start();
            tasks.Add(producer);

            // Consumers
            for (var i = 0; i < 2; i++)
            {
                var consumer = new Task(() =>
                {
                    for (; ; )
                    {
                        Thread.Sleep(10);
                        int x;
                        if (!q.Dequeue(out x)) break;
                        var msg = $"{Thread.CurrentThread.ManagedThreadId,3}: {watch.Elapsed,14}      < {x,4:0000}";
                        Trace.WriteLine(msg);
                    }
                    Trace.WriteLine("Consumer quitting");
                });
                consumer.Start();
                tasks.Add(consumer);
            }

            Thread.Sleep(100);

            Trace.WriteLine("Quitting");

            q.Quit();

            Task.WaitAll(tasks.ToArray());
        }
    }
}