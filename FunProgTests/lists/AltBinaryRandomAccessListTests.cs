// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System.Text;
using static FunProgTests.utilities.ExpectedException;

namespace FunProgTests.lists
{
    using System;
    using System.Linq;

    using FunProgLib.lists;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AltBinaryRandomAccessListTests
    {
        private static string DumpTree<T>(AltBinaryRandomAccessList<T>.DataType tree) where T : IComparable<T>
        {
            if (tree == null)
                return "null";

            var zero = tree as AltBinaryRandomAccessList<T>.Zero;
            if (zero != null)
            {
                var result = new StringBuilder();
                result.Append("[Zero: ");
                result.Append(DumpList(zero.RList));
                result.Append("]");
                return result.ToString();
            }

            var one = tree as AltBinaryRandomAccessList<T>.One;
            if (one != null)
            {
                var result = new StringBuilder();
                result.Append("[One: ");
                result.Append(one.Alpha);
                result.Append(", ");
                result.Append(DumpList(one.RList));
                result.Append("]");
                return result.ToString();
            }

            throw new ArgumentException();
        }

        private static string DumpList<T>(RList<Tuple<T, T>>.Node list)
        {
            var result = new StringBuilder();
            result.Append("{");
            var separator = "";
            while (true)
            {
                if (list == null) break;
                result.Append(separator);
                separator = ", ";
                var head = RList<Tuple<T, T>>.Head(list);
                result.Append(head);
                list = RList<Tuple<T, T>>.Tail(list);
            }
            result.Append("}");
            return result.ToString();
        }

        [TestMethod]
        public void DumpEmptyTree()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            Assert.AreEqual("null", DumpTree(list));
        }

        [TestMethod]
        public void DumpOddTree()
        {
            const string data = "a b c";
            var tree = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("[One: c, {(b, a)}]", DumpTree(tree));
        }

        [TestMethod]
        public void DumpEvenTree()
        {
            const string data = "a b c d";
            var tree = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("[Zero: {(d, c), (b, a)}]", DumpTree(tree));
        }

        [TestMethod]
        public void IsEmptyTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            Assert.IsTrue(AltBinaryRandomAccessList<string>.IsEmpty(list));
            list = AltBinaryRandomAccessList<string>.Cons("A", list);
            Assert.IsFalse(AltBinaryRandomAccessList<string>.IsEmpty(list));
        }

        [TestMethod]
        public void ConsEmptyTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            list = AltBinaryRandomAccessList<string>.Cons("A", list);
            Assert.AreEqual("[One: A, {}]", DumpTree(list));
        }

        [TestMethod]
        public void ConsOneTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            list = AltBinaryRandomAccessList<string>.Cons("A", list);
            list = AltBinaryRandomAccessList<string>.Cons("B", list);
            Assert.AreEqual("[Zero: {(B, A)}]", DumpTree(list));
        }

        [TestMethod]
        public void ConsTwoTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            list = AltBinaryRandomAccessList<string>.Cons("A", list);
            list = AltBinaryRandomAccessList<string>.Cons("B", list);
            list = AltBinaryRandomAccessList<string>.Cons("C", list);
            Assert.AreEqual("[One: C, {(B, A)}]", DumpTree(list));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            var ex = AssertThrows<ArgumentException>(() => AltBinaryRandomAccessList<string>.Head(list));
            Assert.AreEqual("must be Zero or One\r\nParameter name: dataType", ex.Message);
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            var ex = AssertThrows<ArgumentException>(() => AltBinaryRandomAccessList<string>.Tail(list));
            Assert.AreEqual("must be Zero or One\r\nParameter name: dataType", ex.Message);
        }

        [TestMethod]
        public void HeadTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("cow?", AltBinaryRandomAccessList<string>.Head(data));
        }

        [TestMethod]
        public void TailTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            data = AltBinaryRandomAccessList<string>.Tail(data);
            Assert.AreEqual("[One: brown, {(now,, How)}]", DumpTree(data));
        }

        [TestMethod]
        public void LookupNullTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            var ex = AssertThrows<ArgumentException>(() => AltBinaryRandomAccessList<string>.Lookup(0, list));
            Assert.AreEqual("must be Zero or One\r\nParameter name: ts", ex.Message);
        }

        [TestMethod]
        public void LookupSingleTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            list = AltBinaryRandomAccessList<string>.Cons("A", list);
            Assert.AreEqual("A", AltBinaryRandomAccessList<string>.Lookup(0, list));
        }

        [TestMethod]
        public void LookupDoubleTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            list = AltBinaryRandomAccessList<string>.Cons("A", list);
            list = AltBinaryRandomAccessList<string>.Cons("B", list);
            Assert.AreEqual("[Zero: {(B, A)}]", DumpTree(list));

            Assert.AreEqual("B", AltBinaryRandomAccessList<string>.Lookup(0, list));
        }

        [TestMethod]
        public void UpdateTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            data = AltBinaryRandomAccessList<string>.Update(1, "green", data);
            Assert.AreEqual("[Zero: {(cow?, green), (now,, How)}]", DumpTree(data));
            Assert.AreEqual("green", AltBinaryRandomAccessList<string>.Lookup(1, data));
        }

        [TestMethod]
        public void FupdateTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));

            data = AltBinaryRandomAccessList<string>.Fupdate(value => value + "-" + value, 1, data);
            Assert.AreEqual("[Zero: {(cow?, brown-brown), (now,, How)}]", DumpTree(data));
            Assert.AreEqual("brown-brown", AltBinaryRandomAccessList<string>.Lookup(1, data));
        }

        [TestMethod]
        public void RoseTest()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("[Zero: {(sweet., as), (smell, would), (name, other), (any, by), (rose, a), (call, we), (which, That), (name?, a), (in, What's)}]", DumpTree(data));
            Assert.AreEqual("sweet.", AltBinaryRandomAccessList<string>.Lookup(0, data));
            Assert.AreEqual("What's", AltBinaryRandomAccessList<string>.Lookup(17, data));
        }

        [TestMethod]
        public void Test1()
        {
            var data = AltBinaryRandomAccessList<int>.Empty;
            for (var i = 0; i < 11; i++)
                data = AltBinaryRandomAccessList<int>.Cons(i, data);
            Assert.AreEqual("[One: 10, {(9, 8), (7, 6), (5, 4), (3, 2), (1, 0)}]", DumpTree(data));
        }
    }
}