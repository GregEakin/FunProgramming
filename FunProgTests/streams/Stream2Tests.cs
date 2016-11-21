// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using System.Threading;
using System.Threading.Tasks;
using FunProgLib.streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.streams
{
    [TestClass]
    public class Stream2Tests
    {
        private sealed class Stuff
        {
            public Stuff(int key)
            {
                Key = key;
            }

            public int Key { get; }
        }

        private static Stream<Stuff>.StreamCell Factory(int index)
        {
            return new Stream<Stuff>.StreamCell(new Stuff(index),
                new Lazy<Stream<Stuff>.StreamCell>(() => Factory(index + 1)));
        }

        [TestMethod]
        public void FirstTenTest()
        {
            var stream = Factory(-1).Next;

            for (var i = 0; i < 10; i++)
            {
                Assert.IsFalse(stream.IsValueCreated);
                var current = Interlocked.Exchange(ref stream, stream.Value.Next);
                Assert.AreEqual(i, current.Value.Element.Key);
            }

            Assert.IsFalse(stream.IsValueCreated);
        }
    }
}