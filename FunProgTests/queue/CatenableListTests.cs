// Fun Programming Data Structures 1.0
// 
// Copyright © 2015 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using FunProgLib.queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.queue
{
    [TestClass]
    public class CatenableListTests
    {
        [TestMethod]
        public void Test1()
        {
            var list = CatenableList<RealTimeDeque<string>.Queue>.Empty;
            var queue = RealTimeDeque<string>.Empty;
            list = CatenableList<RealTimeDeque<string>.Queue>.Cons(queue, list);
        }
    }
}