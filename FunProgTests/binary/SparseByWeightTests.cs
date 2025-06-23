// Copyright 2025 Gregory Eakin <greg@eakin.dev>
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
        if (FunList<int>.IsEmpty(number))
            return "0";
        var result = string.Join(',', FunList<int>.Reverse(number));
        return result;
    }

    [Test]
    public async Task ZeroTest()
    {
        await Assert.That(DumpNat(Zero)).IsEqualTo("0");
    }

    [Test]
    public async Task OneTest()
    {
        await Assert.That(DumpNat(One)).IsEqualTo("1");
    }

    [Test]
    public async Task TwoTest()
    {
        await Assert.That(DumpNat(Two)).IsEqualTo("2");
    }

    [Test]
    public async Task FiveTest()
    {
        await Assert.That(DumpNat(Five)).IsEqualTo("4,1");
    }

    [Test]
    public async Task FifteenTest()
    {
        await Assert.That(DumpNat(Fifteen)).IsEqualTo("8,4,2,1");
    }

    [Test]
    public async Task SixteenTest()
    {
        var sixteen = SparseByWeight.Inc(Fifteen);
        await Assert.That(DumpNat(sixteen)).IsEqualTo("16");
    }

    [Test]
    public async Task SeventeenTest()
    {
        var seventeen = SparseByWeight.Add(Fifteen, Two);
        await Assert.That(DumpNat(seventeen)).IsEqualTo("16,1");
    }

    [Test]
    public async Task FourTest()
    {
        var four = SparseByWeight.Dec(Five);
        await Assert.That(DumpNat(four)).IsEqualTo("4");
    }

    [Test]
    public async Task ThreeTest()
    {
        var four = SparseByWeight.Dec(Five);
        var three = SparseByWeight.Dec(four);
        await Assert.That(DumpNat(Three)).IsEqualTo("2,1");
        await Assert.That(DumpNat(three)).IsEqualTo("2,1");
    }
}