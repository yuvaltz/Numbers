using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Web.Generic
{
    public class ComparableEqualityComparer<T> : IEqualityComparer<T> where T : IComparable<T>
    {
        public bool Equals(T x, T y)
        {
            return x.CompareTo(y) == 0;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public bool Equals(object x, object y)
        {
            return ((T)x).CompareTo((T)y) == 0;
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}
