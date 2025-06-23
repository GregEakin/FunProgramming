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

using FunProgLib.lists;

namespace FunProgTests.lists;

public class CustomStackTests
{
    [Test]
    public async Task EmptyTest()
    {
        var stack = CustomStack<string>.Empty;
        await Assert.That(CustomStack<string>.IsEmpty(stack)).IsTrue();
    }

    [Test]
    public async Task NotEmptyTest()
    {
        var stack = CustomStack<string>.Cons("Hello", CustomStack<string>.Empty);
        await Assert.That(CustomStack<string>.IsEmpty(stack)).IsFalse();
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var stack = CustomStack<string>.Empty;
        await Assert.That(() => CustomStack<string>.Head(stack)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task HeadTest()
    {
        var stack = CustomStack<string>.Cons("Hello", CustomStack<string>.Empty);
        var head = CustomStack<string>.Head(stack);
        await Assert.That(head).IsEqualTo("Hello");
    }

    [Test]
    public async Task TailTest()
    {
        var empty = CustomStack<string>.Empty;
        var hello = CustomStack<string>.Cons("Hello", empty);
        var world = CustomStack<string>.Cons("World", hello);
        var stack = CustomStack<string>.Tail(world);
        await Assert.That(stack).IsEqualTo(hello);
    }
}