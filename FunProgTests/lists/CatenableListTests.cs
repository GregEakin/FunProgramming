// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System.Linq;
using System.Text;

namespace FunProgTests.lists
{
    using System;
    using FunProgLib.lists;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static utilities.ExpectedException;

    [TestClass]
    public class CatenableListTests
    {
        private static string DumpList<T>(CatenableList<T>.C list)
        {
            if (CatenableList<T>.IsEmpty(list))
                return "\u2205";

            var result = new StringBuilder();
            while(!CatenableList<T>.IsEmpty(list))
            {
                result.Append(CatenableList<T>.Head(list));
                list = CatenableList<T>.Tail(list);
                result.Append(", ");
            }
            result.Remove(result.Length - 2, 2);
            return result.ToString();
        }

        [TestMethod]
        public void IsEmptyTest()
        {
            var list = CatenableList<string>.Empty;
            Assert.IsTrue(CatenableList<string>.IsEmpty(list));
            Assert.AreEqual("\u2205", DumpList(list));

            list = CatenableList<string>.Cons("A", list);
            Assert.IsFalse(CatenableList<string>.IsEmpty(list));
            Assert.AreEqual("A", DumpList(list));
        }

        [TestMethod]
        public void Cat1Test()
        {
            var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
            var list3 = CatenableList<string>.Cat(list1, CatenableList<string>.Empty);
            Assert.AreSame(list1, list3);
        }

        [TestMethod]
        public void Cat2Test()
        {
            var list2 = "x y z".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
            var list3 = CatenableList<string>.Cat(CatenableList<string>.Empty, list2);
            Assert.AreSame(list2, list3);
        }

        [TestMethod]
        public void Cat3Test()
        {
            var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
            var list2 = "x y z".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
            var list3 = CatenableList<string>.Cat(list1, list2);
            Assert.AreEqual("c, b, a, z, y, x", DumpList(list3));
        }

        [TestMethod]
        public void ConsTest()
        {
            var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
            Assert.AreEqual("c, b, a", DumpList(list1));
        }

        [TestMethod]
        public void SnocTest()
        {
            var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Snoc(current, word));
            Assert.AreEqual("a, b, c", DumpList(list1));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var list = CatenableList<string>.Empty;
            AssertThrows<ArgumentNullException>(() => CatenableList<string>.Head(list));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string head = "Head";
            var list0 = CatenableList<string>.Empty;
            var list1 = CatenableList<string>.Cons("Rest", list0);
            var list2 = CatenableList<string>.Cons(head, list1);
            Assert.AreSame(head, CatenableList<string>.Head(list2));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var list = CatenableList<string>.Empty;
            AssertThrows<ArgumentNullException>(() => CatenableList<string>.Tail(list));
        }

        [TestMethod]
        public void TailTest()
        {
            var list0 = CatenableList<string>.Empty;
            var list1 = CatenableList<string>.Cons("Rest", list0);
            var list2 = CatenableList<string>.Cons("Head", list1);
            Assert.AreSame(list1, CatenableList<string>.Tail(list2));
        }
    }
}