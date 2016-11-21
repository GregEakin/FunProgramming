﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using FunProgLib.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.utilities
{
    [TestClass]
    public class StringUtilitiesTests
    {
        [TestMethod]
        public void EnumberableToReadableStringEmptyTest()
        {
            var data = new string[0];
            Assert.AreEqual("[]", data.ToReadableString());
        }

        [TestMethod]
        public void EnumerableToReadableStringTest()
        {
            var data = new[] { "A", "B" };
            Assert.AreEqual("[A, B]", data.ToReadableString());
        }

        [TestMethod]
        public void StringFormatWithTest()
        {
            Assert.AreEqual("1, 2", "{0}, {1}".FormatWith(1, 2));
        }
    }
}