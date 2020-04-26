// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: FunProgTests
// FILE:  MemoryTests.cs
// AUTHOR:  Greg Eakin

using FunProgLib.queue;
using JetBrains.dotMemoryUnit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FunProgLib.lists;

namespace FunProgTests.memory
{
    [TestClass]
    public class MemoryTests
    {
        [TestMethod]
        [DotMemoryUnit(FailIfRunWithoutSupport = false)]
        public void Test1()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);

            dotMemory.Check(memory =>
            {
                Assert.AreEqual(5,
                    memory.GetObjects(where => where.Type.Is<List<string>.Node>()).ObjectsCount);
            });

            foreach (var expected in data.Split())
            {
                var head = PhysicistsQueue<string>.Head(queue);
                Assert.AreEqual(expected, head);
                queue = PhysicistsQueue<string>.Tail(queue);
            }

            dotMemory.Check(memory =>
            {
                Assert.AreEqual(0,
                    memory.GetObjects(where => where.Type.Is<PhysicistsQueue<string>.Queue>()).ObjectsCount);
            });
        }
    }
}