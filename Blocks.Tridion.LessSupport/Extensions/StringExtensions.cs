using System;
using System.Collections.Generic;
using System.Linq;

namespace Blocks.Tridion.LessSupport
{
    internal static class StringExtensions
    {
        public static bool InsensitiveEquals(this string target, string other)
        {
            return target.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string Join(this IEnumerable<string> items, string separator)
        {
            return string.Join(separator, items.ToArray());
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string GetExtension(this string fileName)
        {
            var index = fileName.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase);
            return index >= 0 ? fileName.Substring(index, fileName.Length - index) : ".less";
        }
    }
}