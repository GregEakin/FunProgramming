// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.lamda
{
    [TestClass]
    public class ListOfFuncsTests
    {
        // return a list of funcs, where each one returns a loaded page
        static IEnumerable<Func<int>> GetEnumerable(int? page = null, int limit = 10)
        {
            var currentPage = page ?? 1;
            while (true)
            {
                for (var i = limit * (currentPage - 1); i < limit * currentPage; i++)
                {
                    var i1 = i;
                    yield return () =>
                    {
                        Thread.Sleep(10);
                        return i1;
                    };
                }
                currentPage++;
            }
        }

        [TestMethod]
        public void Test1()
        {
            foreach (var item in GetEnumerable().Skip(100).Take(10))
            {
                Console.WriteLine(item());
            }
        }
    }
}