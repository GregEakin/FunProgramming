// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using System;
using FunProgLib.lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static FunProgTests.utilities.ExpectedException;

namespace FunProgTests.lists
{
    [TestClass]
    public class CustomStackTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var stack = CustomStack<string>.Empty;
            Assert.IsTrue(CustomStack<string>.IsEmpty(stack));
        }

        [TestMethod]
        public void NotEmptyTest()
        {
            var stack = CustomStack<string>.Cons("Hello", CustomStack<string>.Empty);
            Assert.IsFalse(CustomStack<string>.IsEmpty(stack));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var stack = CustomStack<string>.Empty;
            AssertThrows<ArgumentNullException>(() => CustomStack<string>.Head(stack));
        }

        [TestMethod]
        public void HeadTest()
        {
            var stack = CustomStack<string>.Cons("Hello", CustomStack<string>.Empty);
            var head = CustomStack<string>.Head(stack);
            Assert.AreEqual("Hello", head);
        }

        [TestMethod]
        public void TailTest()
        {
            var empty = CustomStack<string>.Empty;
            var hello = CustomStack<string>.Cons("Hello", empty);
            var world = CustomStack<string>.Cons("World", hello);
            var stack = CustomStack<string>.Tail(world);
            Assert.AreEqual(hello, stack);
        }
    }
}