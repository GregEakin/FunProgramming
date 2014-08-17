// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		DenseTest.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.binary
{
    using System.Text;

    using FunProgLib.binary;
    using FunProgLib.persistence;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DenseTest
    {
        private static readonly LinkList<string>.List Zero = null;
        private static readonly LinkList<string>.List One = Dense.Inc(Zero);
        private static readonly LinkList<string>.List Two = Dense.Inc(One);

        private static string DumpNat(LinkList<string>.List number)
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
            var three = Dense.Inc(Two);
            var five = Dense.Add(Two, three);
            Assert.AreEqual("101", DumpNat(five));
        }

        [TestMethod]
        public void FourTest()
        {
            var three = Dense.Inc(Two);
            var five = Dense.Add(Two, three);
            var four = Dense.Dec(five);
            Assert.AreEqual("100", DumpNat(four));
        }

        [TestMethod]
        public void ThreeTest()
        {
            var three = Dense.Inc(Two);
            var five = Dense.Add(Two, three);
            var four = Dense.Dec(five);
            var three2 = Dense.Dec(four);
            Assert.AreEqual("11", DumpNat(three));
            Assert.AreEqual("11", DumpNat(three2));
        }
    }
}