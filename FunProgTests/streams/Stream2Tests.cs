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

using FunProgLib.streams;

namespace FunProgTests.streams;

public class Stream2Tests
{
    private sealed class Stuff
    {
        public Stuff(int key)
        {
            Key = key;
        }

        public int Key { get; }
    }

    private static Stream<Stuff>.StreamCell Factory(int index)
    {
        return new Stream<Stuff>.StreamCell(new Stuff(index),
            new Lazy<Stream<Stuff>.StreamCell>(() => Factory(index + 1)));
    }

    [Fact]
    public void FirstTenTest()
    {
        var stream = Factory(-1).Next;

        for (var i = 0; i < 10; i++)
        {
            Assert.False(stream.IsValueCreated);
            var current = Interlocked.Exchange(ref stream, stream.Value.Next);
            Assert.Equal(i, current.Value.Element.Key);
        }

        Assert.False(stream.IsValueCreated);
    }
}