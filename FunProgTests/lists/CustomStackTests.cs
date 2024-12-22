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

using FunProgLib.lists;

namespace FunProgTests.lists;

public class CustomStackTests
{
    [Fact]
    public void EmptyTest()
    {
        var stack = CustomStack<string>.Empty;
        Assert.True(CustomStack<string>.IsEmpty(stack));
    }

    [Fact]
    public void NotEmptyTest()
    {
        var stack = CustomStack<string>.Cons("Hello", CustomStack<string>.Empty);
        Assert.False(CustomStack<string>.IsEmpty(stack));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var stack = CustomStack<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => CustomStack<string>.Head(stack));
    }

    [Fact]
    public void HeadTest()
    {
        var stack = CustomStack<string>.Cons("Hello", CustomStack<string>.Empty);
        var head = CustomStack<string>.Head(stack);
        Assert.Equal("Hello", head);
    }

    [Fact]
    public void TailTest()
    {
        var empty = CustomStack<string>.Empty;
        var hello = CustomStack<string>.Cons("Hello", empty);
        var world = CustomStack<string>.Cons("World", hello);
        var stack = CustomStack<string>.Tail(world);
        Assert.Equal(hello, stack);
    }
}