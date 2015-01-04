// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System.Collections;
using System.Globalization;
using System.Text;

namespace FunProgLib.Utilities
{
    public static class StringUtilities
    {
        public static string ToReadableString(this IEnumerable list)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var l in list)
            {
                sb.Append(l + ", ");
            }
            if (sb.Length > 2)
            {
                sb.Remove(sb.Length - 2, 2);
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static string FormatWith(this string mask, params object[] parameters)
        {
            return string.Format(CultureInfo.InvariantCulture, mask, parameters);
        }
    }
}