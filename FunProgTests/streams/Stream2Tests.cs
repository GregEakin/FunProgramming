// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

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