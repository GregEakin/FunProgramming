// Copyright 2014 Gregory Eakin <greg@eakin.dev>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using FunProgLib.binary;
using FunProgLib.lists;

namespace FunProgTests.binary;

public class SparseByWeightTests
{
    private static readonly FunList<int>.Node Zero = FunList<int>.Empty;
    private static readonly FunList<int>.Node One = SparseByWeight.Inc(Zero);
    private static readonly FunList<int>.Node Two = SparseByWeight.Inc(One);
    private static readonly FunList<int>.Node Three = SparseByWeight.Inc(Two);
    private static readonly FunList<int>.Node Five = SparseByWeight.Add(Two, Three);
    private static readonly FunList<int>.Node Fifteen = SparseByWeight.Add(Five, SparseByWeight.Add(Five, Five));

    private static string DumpNat(FunList<int>.Node number)
    {
        if (FunList<int>.IsEmpty(number)) return "0";
        var result = string.Join(',', FunList<int>.Reverse(number));
        return result;
    }

    [Fact]
    public void ZeroTest()
    {
        Assert.Equal("0", DumpNat(Zero));
    }

    [Fact]
    public void OneTest()
    {
        Assert.Equal("1", DumpNat(One));
    }

    [Fact]
    public void TwoTest()
    {
        Assert.Equal("2", DumpNat(Two));
    }

    [Fact]
    public void FiveTest()
    {
        Assert.Equal("4,1", DumpNat(Five));
    }

    [Fact]
    public void FifteenTest()
    {
        Assert.Equal("8,4,2,1", DumpNat(Fifteen));
    }

    [Fact]
    public void SixteenTest()
    {
        var sixteen = SparseByWeight.Inc(Fifteen);
        Assert.Equal("16", DumpNat(sixteen));
    }

    [Fact]
    public void SeventeenTest()
    {
        var seventeen = SparseByWeight.Add(Fifteen, Two);
        Assert.Equal("16,1", DumpNat(seventeen));
    }

    [Fact]
    public void FourTest()
    {
        var four = SparseByWeight.Dec(Five);
        Assert.Equal("4", DumpNat(four));
    }

    [Fact]
    public void ThreeTest()
    {
        var four = SparseByWeight.Dec(Five);
        var three = SparseByWeight.Dec(four);
        Assert.Equal("2,1", DumpNat(Three));
        Assert.Equal("2,1", DumpNat(three));
    }
}