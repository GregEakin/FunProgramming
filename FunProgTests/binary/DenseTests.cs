﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using System;
using System.Text;

using FunProgLib.binary;
using FunProgLib.lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static FunProgTests.utilities.ExpectedException;

namespace FunProgTests.binary
{
    [TestClass]
    public class DenseTests
    {
        private static readonly List<Dense.Digit>.Node Zero = List<Dense.Digit>.Empty;
        private static readonly List<Dense.Digit>.Node One = Dense.Inc(Zero);
        private static readonly List<Dense.Digit>.Node Two = Dense.Inc(One);
        private static readonly List<Dense.Digit>.Node Three = Dense.Inc(Two);
        private static readonly List<Dense.Digit>.Node Five = Dense.Add(Two, Three);
        private static readonly List<Dense.Digit>.Node Fifteen = Dense.Add(Five, Dense.Add(Five, Five));

        private static string DumpNat(List<Dense.Digit>.Node number)
        {
            if (number == null) return "0";
            var result = new StringBuilder();
            while (number != null)
            {
                if (number.Element == Dense.Digit.Zero) result.Insert(0, "0");
                else if (number.Element == Dense.Digit.One) result.Insert(0, "1");
                else result.Insert(0, "*");
                number = number.Next;
            }
            return result.ToString();
        }

        [TestMethod]
        public void ZeroTest()
        {
            Assert.AreEqual("0", DumpNat(Zero));
            Assert.IsTrue(List<Dense.Digit>.IsEmpty(Zero));
        }

        [TestMethod]
        public void DecrementOneTest()
        {
            var zero = Dense.Dec(One);
            Assert.AreEqual("0", DumpNat(zero));
            Assert.IsTrue(List<Dense.Digit>.IsEmpty(zero));
        }

        [TestMethod]
        public void NegativeTest()
        {
            var exception = AssertThrows<ArgumentException>(() => Dense.Dec(Zero));
            Assert.AreEqual("Can't go negative\r\nParameter name: ds", exception.Message);
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
        public void DecTest()
        {
            var four = Dense.Dec(Five);
            Assert.AreEqual("100", DumpNat(four));
        }

        [TestMethod]
        public void DecWithCaryTest()
        {
            var four = Dense.Dec(Five);
            var three = Dense.Dec(four);
            Assert.AreEqual("11", DumpNat(three));
        }
    }
}