// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

namespace FunProgTests.utilities;

public static class Counters
{
    public static int CountBinaryOnes(int n)
    {
        var count = 0;
        while (n != 0)
        {
            n &= n - 1;
            count++;
        }

        return count;
    }

    public static int CountChar(string s, char c) => s.Split(c).Length;
}