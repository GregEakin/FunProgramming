// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.utilities
{
    public static class ExpectedException
    {
        public static T AssertThrows<T>(Action action) where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T ex)
            {
                if (ex.GetType() != typeof(T)) throw;
                return ex;
            }

            Assert.Fail("Failed to throw exception!");
            return null;
        }
    }
}