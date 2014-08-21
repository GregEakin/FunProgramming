// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		DenseTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.binary
{
    using System.Text;

    using FunProgLib.binary;
    using FunProgLib.lists;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DenseTests
    {
        private static readonly List<string>.Node Zero = null;
        private static readonly List<string>.Node One = Dense.Inc(Zero);
        private static readonly List<string>.Node Two = Dense.Inc(One);
        private static readonly List<string>.Node Three = Dense.Inc(Two);
        private static readonly List<string>.Node Five = Dense.Add(Two, Three);
        private static readonly List<string>.Node Fifteen = Dense.Add(Five, Dense.Add(Five, Five));

        private static string DumpNat(List<string>.Node number)
        {
            if (number == null) return "0";
            var result = new StringBuilder();
            var digit = number;
            while (digit != null)
            {
                if (digit.Element == "Zero") result.Insert(0, "0");
                else if (digit.Element == "One") result.Insert(0, "1");
                else result.Insert(0, "*");
                digit = digit.Next;
            }
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
            Assert.AreEqual("10", DumpNat(Two));
        }

        [TestMethod]
        public void FiveTest()
        {
            Assert.AreEqual("101", DumpNat(Five));
        }

        [TestMethod]
        public void FifteenTest()
        {
            Assert.AreEqual("1111", DumpNat(Fifteen));
        }

        [TestMethod]
        public void SixteenTest()
        {
            var sixteen = Dense.Inc(Fifteen);
            Assert.AreEqual("10000", DumpNat(sixteen));
        }

        [TestMethod]
        public void FourTest()
        {
            var four = Dense.Dec(Five);
            Assert.AreEqual("100", DumpNat(four));
        }

        [TestMethod]
        public void ThreeTest()
        {
            var four = Dense.Dec(Five);
            var three = Dense.Dec(four);
            Assert.AreEqual("11", DumpNat(Three));
            Assert.AreEqual("11", DumpNat(three));
        }
    }
}