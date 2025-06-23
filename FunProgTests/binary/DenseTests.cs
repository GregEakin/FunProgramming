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
        if (number == null)
            return "0";
        var result = new StringBuilder();
        while (number != null)
        {
            if (number.Element == Dense.Digit.Zero)
                result.Insert(0, "0");
            else if (number.Element == Dense.Digit.One)
                result.Insert(0, "1");
            else
                result.Insert(0, "*");
            number = number.Next;
        }

        return result.ToString();
    }

    [Test]
    public async Task ZeroTest()
    {
        await Assert.That(DumpNat(Zero)).IsEqualTo("0");
        await Assert.That(FunList<Dense.Digit>.IsEmpty(Zero)).IsTrue();
    }

    [Test]
    public async Task DecrementOneTest()
    {
        var zero = Dense.Dec(One);
        await Assert.That(DumpNat(zero)).IsEqualTo("0");
        await Assert.That(FunList<Dense.Digit>.IsEmpty(zero)).IsTrue();
    }

    [Test]
    public async Task NegativeTest()
    {
        var exception = await Assert.That(() => Dense.Dec(Zero)).Throws<ArgumentException>();
        await Assert.That(exception.Message).IsEqualTo("Can't go negative (Parameter 'ds')");
    }

    [Test]
    public async Task OneTest()
    {
        await Assert.That(DumpNat(One)).IsEqualTo("1");
    }

    [Test]
    public async Task TwoTest()
    {
        await Assert.That(DumpNat(Two)).IsEqualTo("10"); ;
    }

    [Test]
    public async Task FiveTest()
    {
        await Assert.That(DumpNat(Five)).IsEqualTo("101"); ;
    }

    [Test]
    public async Task FifteenTest()
    {
        await Assert.That(DumpNat(Fifteen)).IsEqualTo("1111"); ;
    }

    [Test]
    public async Task SixteenTest()
    {
        var sixteen = Dense.Inc(Fifteen);
        await Assert.That(DumpNat(sixteen)).IsEqualTo("10000"); ;
    }

    [Test]
    public async Task DecTest()
    {
        var four = Dense.Dec(Five);
        await Assert.That(DumpNat(four)).IsEqualTo("100"); ;
    }

    [Test]
    public async Task DecWithCaryTest()
    {
        var four = Dense.Dec(Five);
        var three = Dense.Dec(four);
        await Assert.That(DumpNat(three)).IsEqualTo("11"); ;
    }
}