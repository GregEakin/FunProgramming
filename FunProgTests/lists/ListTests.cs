// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.Utilities;

namespace FunProgTests.lists
{
    using System;
    using System.Linq;

    using FunProgLib.lists;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static utilities.ExpectedException;

    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = List<string>.Empty;
            Assert.IsTrue(List<string>.IsEmpty(list));
            list = List<string>.Cons("A", list);
            Assert.IsFalse(List<string>.IsEmpty(list));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var list = List<string>.Empty;
            var exception = AssertThrows<ArgumentNullException>(() => List<string>.Head(list));
            Assert.AreEqual("Value cannot be null.\r\nParameter name: list", exception.Message);
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var list = List<string>.Empty;
            var exception = AssertThrows<ArgumentNullException>(() => List<string>.Tail(list));
            Assert.AreEqual("Value cannot be null.\r\nParameter name: list", exception.Message);
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            const string data = "a b c";
            var list = data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));
            Assert.AreEqual("[c, b, a]", list.ToReadableString());
        }

        [TestMethod]
        public void ReverseEmptyListTest()
        {
            var list = List<string>.Reverse(List<string>.Empty);
            Assert.IsTrue(List<string>.IsEmpty(list));
        }

        [TestMethod]
        public void ReverseSingleListTest()
        {
            var list = List<string>.Cons("Wow", List<string>.Empty);
            var reverse = List<string>.Reverse(list);
            Assert.AreSame(list, reverse);
        }

        [TestMethod]
        public void ReverseListTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));
            var reverse = List<string>.Reverse(list);
            Assert.AreEqual("[How, now,, brown, cow?]", reverse.ToReadableString());
        }

        [TestMethod]
        public void CatBothEmptyTest()
        {
            var list = List<string>.Cat(List<string>.Empty, List<string>.Empty);
            Assert.IsTrue(List<string>.IsEmpty(list));
        }

        [TestMethod]
        public void CatLeftEmptyTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list2 = List<string>.Cat(List<string>.Empty, list);
            Assert.AreSame(list, list2);
        }

        [TestMethod]
        public void CatRightEmptyTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list2 = List<string>.Cat(list, List<string>.Empty);
            Assert.AreSame(list, list2);
        }

        [TestMethod]
        public void CatTest()
        {
            const string data1 = "How now,";
            var list1 = data1.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            const string data2 = "brown cow?";
            var list2 = data2.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list3 = List<string>.Cat(list1, list2);
            Assert.AreEqual("[now,, How, cow?, brown]", list3.ToReadableString());
        }

        [TestMethod]
        public void FoldRightSumTest()
        {
            var data = new[]{ 1, 2, 3, 4, 5 };
            var list = data.Aggregate(List<int>.Empty, (current, word) => List<int>.Cons(word, current));

            var sum = List<int>.FoldRight(list, 0, (x, y) => x + y);
            Assert.AreEqual(15, sum);
        }

        [TestMethod]
        public void FoldLeftSumTest()
        {
            var data = new[] { 1, 2, 3, 4, 5 };
            var list = data.Aggregate(List<int>.Empty, (current, word) => List<int>.Cons(word, current));

            var sum = List<int>.FoldLeft(list, 0, (x, y) => x + y);
            Assert.AreEqual(15, sum);
        }

        [TestMethod]
        public void FoldLeftRSumTest()
        {
            var data = new[] { 1, 2, 3, 4, 5 };
            var list = data.Aggregate(List<int>.Empty, (current, word) => List<int>.Cons(word, current));

            var sum = List<int>.FoldLeftR(list, 0, (x, y) => x + y);
            Assert.AreEqual(15, sum);
        }

        [TestMethod]
        public void FoldRightLSumTest()
        {
            var data = new[] { 1, 2, 3, 4, 5 };
            var list = data.Aggregate(List<int>.Empty, (current, word) => List<int>.Cons(word, current));

            var sum = List<int>.FoldRightL(list, 0, (x, y) => x + y);
            Assert.AreEqual(15, sum);
        }

        [TestMethod]
        public void FoldRightProductTest()
        {
            var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
            var list = data.Aggregate(List<double>.Empty, (current, word) => List<double>.Cons(word, current));

            var product = List<double>.FoldRight(list, 1.0, (x, y) => x * y);
            Assert.AreEqual(120.0, product);
        }

        [TestMethod]
        public void FoldLeftProductTest()
        {
            var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
            var list = data.Aggregate(List<double>.Empty, (current, word) => List<double>.Cons(word, current));

            var product = List<double>.FoldLeft(list, 1.0, (x, y) => x * y);
            Assert.AreEqual(120.0, product);
        }

        [TestMethod]
        public void FoldLeftRProductTest()
        {
            var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
            var list = data.Aggregate(List<double>.Empty, (current, word) => List<double>.Cons(word, current));

            var product = List<double>.FoldLeftR(list, 1.0, (x, y) => x * y);
            Assert.AreEqual(120.0, product);
        }

        [TestMethod]
        public void FoldRightLProductTest()
        {
            var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
            var list = data.Aggregate(List<double>.Empty, (current, word) => List<double>.Cons(word, current));

            var product = List<double>.FoldRightL(list, 1.0, (x, y) => x * y);
            Assert.AreEqual(120.0, product);
        }
    }
}