using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class StringExt
    {
        public static string PopFrontMatch(this string self, string str)
        {
            if (str.Length > self.Length)
                return self;
            else if (self.IndexOf(str) == 0)
                return self.Substring(str.Length);
            else
                return self;
        }

        public static string PopBackMatch(this string self, string str)
        {
            if (str.Length > self.Length)
                return self;
            else if (self.LastIndexOf(str) == self.Length - str.Length)
                return self.Substring(0, self.Length - str.Length);
            else
                return self;
        }

        public static string PopFront(this string self, int count = 1)
        {
            if (count > self.Length || count < 0)
                throw new IndexOutOfRangeException();
            return self.Substring(count);
        }

        public static string PopBack(this string self, int count = 1)
        {
            if (count > self.Length || count < 0)
                throw new IndexOutOfRangeException();
            return self.Substring(0, self.Length - count);
        }
    }
}
