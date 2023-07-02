// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.binary;
using FunProgLib.lists;

namespace FunProgTests.binary;

public class DenseTests
{
    private static readonly FunList<Dense.Digit>.Node Zero = FunList<Dense.Digit>.Empty;
    private static readonly FunList<Dense.Digit>.Node One = Dense.Inc(Zero);
    private static readonly FunList<Dense.Digit>.Node Two = Dense.Inc(One);
    private static readonly FunList<Dense.Digit>.Node Three = Dense.Inc(Two);
    private static readonly FunList<Dense.Digit>.Node Five = Dense.Add(Two, Three);
    private static readonly FunList<Dense.Digit>.Node Fifteen = Dense.Add(Five, Dense.Add(Five, Five));

    private static string DumpNat(FunList<Dense.Digit>.Node number)
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

    [Fact]
    public void ZeroTest()
    {
        Assert.Equal("0", DumpNat(Zero));
        Assert.True(FunList<Dense.Digit>.IsEmpty(Zero));
    }

    [Fact]
    public void DecrementOneTest()
    {
        var zero = Dense.Dec(One);
        Assert.Equal("0", DumpNat(zero));
        Assert.True(FunList<Dense.Digit>.IsEmpty(zero));
    }

    [Fact]
    public void NegativeTest()
    {
        var exception = Assert.Throws<ArgumentException>(() => Dense.Dec(Zero));
        Assert.Equal("Can't go negative (Parameter 'ds')", exception.Message);
    }

    [Fact]
    public void OneTest()
    {
        Assert.Equal("1", DumpNat(One));
    }

    [Fact]
    public void TwoTest()
    {
        Assert.Equal("10", DumpNat(Two));
    }

    [Fact]
    public void FiveTest()
    {
        Assert.Equal("101", DumpNat(Five));
    }

    [Fact]
    public void FifteenTest()
    {
        Assert.Equal("1111", DumpNat(Fifteen));
    }

    [Fact]
    public void SixteenTest()
    {
        var sixteen = Dense.Inc(Fifteen);
        Assert.Equal("10000", DumpNat(sixteen));
    }

    [Fact]
    public void DecTest()
    {
        var four = Dense.Dec(Five);
        Assert.Equal("100", DumpNat(four));
    }

    [Fact]
    public void DecWithCaryTest()
    {
        var four = Dense.Dec(Five);
        var three = Dense.Dec(four);
        Assert.Equal("11", DumpNat(three));
    }
}