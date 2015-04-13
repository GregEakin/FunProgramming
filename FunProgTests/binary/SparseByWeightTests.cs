// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System.Globalization;
using System.Text;
using FunProgLib.binary;
using FunProgLib.lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.binary
{
    [TestClass]
    public class SparseByWeightTests
    {
        private static readonly List<int>.Node Zero = List<int>.Empty;
        private static readonly List<int>.Node One = SparseByWeight.Inc(Zero);
        private static readonly List<int>.Node Two = SparseByWeight.Inc(One);
        private static readonly List<int>.Node Three = SparseByWeight.Inc(Two);
        private static readonly List<int>.Node Five = SparseByWeight.Add(Two, Three);
        private static readonly List<int>.Node Fifteen = SparseByWeight.Add(Five, SparseByWeight.Add(Five, Five));

        private static string DumpNat(List<int>.Node number)
        {
            if (List<int>.IsEmpty(number)) return "0";
            var result = new StringBuilder();
            var digit = number;
            while (!List<int>.IsEmpty(digit))
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