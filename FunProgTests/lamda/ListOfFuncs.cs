// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		ListOfFuncs.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.lamda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListOfFuncs
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