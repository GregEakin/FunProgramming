// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

namespace FunProgLib.Utilities;

public static class StringUtilities
{
    public static string ToReadableString(this IEnumerable list)
    {
        var sb = new StringBuilder();
        sb.Append("[");
        foreach (var l in list)
            sb.Append(l + ", ");
        if (sb.Length > 2)
            sb.Remove(sb.Length - 2, 2);
        sb.Append("]");
        return sb.ToString();
    }
}