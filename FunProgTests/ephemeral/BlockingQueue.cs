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
        private readonly object key = new object();
        private readonly int size;
        private int count;
        private volatile RealTimeQueue<T>.Queue queue = RealTimeQueue<T>.Empty;
        private volatile bool quit;

        public BlockingQueue(int size)
        {
            this.size = size;
        }

        public void Quit()
        {
            lock (this.key)
            {
                this.quit = true;
                Monitor.PulseAll(this.key);
            }
        }

        public bool Enqueue(T t)
        {
            lock (this.key)
            {
                while (!this.quit && count >= this.size) Monitor.Wait(this.key);

                if (this.quit) return false;

                Interlocked.Increment(ref count);
                queue = RealTimeQueue<T>.Snoc(queue, t);

                Monitor.PulseAll(this.key);
            }

            return true;
        }

        public bool Dequeue(out T t)
        {
            t = default(T);

            lock (this.key)
            {
                while (!this.quit && RealTimeQueue<T>.IsEmpty(queue)) Monitor.Wait(this.key);

                if (RealTimeQueue<T>.IsEmpty(queue)) return false;

                Interlocked.Decrement(ref count);
                t = RealTimeQueue<T>.Head(queue);
                queue = RealTimeQueue<T>.Tail(queue);

                Monitor.PulseAll(this.key);
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
                    var msg = string.Format("{0,3}: {1,14} {2,4} >", Thread.CurrentThread.ManagedThreadId, watch.Elapsed, x.ToString("0000"));
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
                        var msg = string.Format("{0,3}: {1,14}      < {2,4}", Thread.CurrentThread.ManagedThreadId, watch.Elapsed, x.ToString("0000"));
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