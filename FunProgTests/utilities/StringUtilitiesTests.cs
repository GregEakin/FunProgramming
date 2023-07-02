// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.Utilities;

namespace FunProgTests.utilities;

public class StringUtilitiesTests
{
    [Fact]
    public void EnumerableToReadableStringEmptyTest()
    {
        var data = new string[0];
        Assert.Equal("[]", data.ToReadableString());
    }

    [Fact]
    public void EnumerableToReadableStringTest()
    {
        var data = new[] { "A", "B" };
        Assert.Equal("[A, B]", data.ToReadableString());
    }
}