// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using FunProgLib.lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.lists
{
    [TestClass]
    public class AltBinaryRandomAccessEmptyTests
    {
        private readonly AltBinaryRandomAccessList<string> _list = AltBinaryRandomAccessList<string>.Empty;

        [TestMethod]
        public void EmptyTest()
        {
            Assert.IsTrue(_list.IsEmpty);
        }

        [TestMethod]
        public void ConsTest()
        {
            var list1 = _list.Cons("Test");
            // Assert.IsNotInstanceOfType(list1, typeof(AltBinaryRandomAccessOne<string>));
            // Assert.AreSame(List2<Tuple<string, string>>.Empty, list1.List);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UnconsTest()
        {
            var hd = _list.Uncons;
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void HeadTest()
        {
            var hd = _list.Head;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TailTest()
        {
            var tl = _list.Tail;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LookupTest()
        {
            var tl = _list.Lookup(0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FupdateTest()
        {
            var tl = _list.Fupdate(x => "test", 0);
        }
    }
}