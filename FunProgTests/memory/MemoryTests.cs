// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: FunProgTests
// FILE:  MemoryTests.cs
// AUTHOR:  Greg Eakin

using FunProgLib.queue;
using FunProgLib.lists;
// using JetBrains.dotMemoryUnit;

namespace FunProgTests.memory;

public class MemoryTests
{
    // [Fact]
    // [DotMemoryUnit(FailIfRunWithoutSupport = false)]
    // public void Test1()
    // {
    //     const string data = "One Two Three One Three";
    //     var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
    //
    //     dotMemory.Check(memory =>
    //     {
    //         Assert.Equal(5,
    //             memory.GetObjects(where => where.Type.Is<FunList<string>.Node>()).ObjectsCount);
    //     });
    //
    //     foreach (var expected in data.Split())
    //     {
    //         var head = PhysicistsQueue<string>.Head(queue);
    //         Assert.Equal(expected, head);
    //         queue = PhysicistsQueue<string>.Tail(queue);
    //     }
    //
    //     dotMemory.Check(memory =>
    //     {
    //         Assert.Equal(0,
    //             memory.GetObjects(where => where.Type.Is<PhysicistsQueue<string>.Queue>()).ObjectsCount);
    //     });
    // }
}