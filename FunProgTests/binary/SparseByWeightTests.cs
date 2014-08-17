// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		SparseByWeightTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.binary
{
    using System.Globalization;
    using System.Text;

    using FunProgLib.binary;
    using FunProgLib.persistence;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SparseByWeightTests
    {
        private static readonly LinkList<int>.List Zero = null;
        private static readonly LinkList<int>.List One = SparseByWeight.Inc(Zero);
        private static readonly LinkList<int>.List Two = SparseByWeight.Inc(One);
        private static readonly LinkList<int>.List Three = SparseByWeight.Inc(Two);
        private static readonly LinkList<int>.List Five = SparseByWeight.Add(Two, Three);
        private static readonly LinkList<int>.List Fifteen = SparseByWeight.Add(Five, SparseByWeight.Add(Five, Five));

        private static string DumpNat(LinkList<int>.List number)
        {
            if (number == null) return "0";
            var result = new StringBuilder();
            var digit = number;
            while (digit != null)
            {
                result.Insert(0, digit.Element.ToString(CultureInfo.InvariantCulture));
                result.Insert(0, ',');
                digit = digit.Next;
            }
            result = result.Remove(0, 1);

            return result.ToString();
        }

        [TestMethod]
        public void ZeroTest()
        {
            Assert.AreEqual("0", DumpNat(Zero));
        }

        [TestMethod]
        public void OneTest()
        {
            Assert.AreEqual("1", DumpNat(One));
        }

        [TestMethod]
        public void TwoTest()
        {
            Assert.AreEqual("2", DumpNat(Two));
        }

        [TestMethod]
        public void FiveTest()
        {
            Assert.AreEqual("4,1", DumpNat(Five));
        }

        [TestMethod]
        public void FifteenTest()
        {
            Assert.AreEqual("8,4,2,1", DumpNat(Fifteen));
        }

        [TestMethod]
        public void SixteenTest()
        {
            var sixteen = SparseByWeight.Inc(Fifteen);
            Assert.AreEqual("16", DumpNat(sixteen));
        }

        [TestMethod]
        public void SeventeenTest()
        {
            var seventeen = SparseByWeight.Add(Fifteen, Two);
            Assert.AreEqual("16,1", DumpNat(seventeen));
        }

        [TestMethod]
        public void FourTest()
        {
            var four = SparseByWeight.Dec(Five);
            Assert.AreEqual("4", DumpNat(four));
        }

        [TestMethod]
        public void ThreeTest()
        {
            var four = SparseByWeight.Dec(Five);
            var three = SparseByWeight.Dec(four);
            Assert.AreEqual("2,1", DumpNat(Three));
            Assert.AreEqual("2,1", DumpNat(three));
        }
    }
}