using System;
using System.Collections.Generic;
using System.Text;

namespace BlueBlocksLib.BaseClasses
{
    public class Pair<T1, T2>
    {
        public T1 a;
        public T2 b;

        public override bool Equals(object obj)
        {
            if (obj is Pair<T1,T2>)
            {
                var p = obj as Pair<T1,T2>;
                return (this.a.Equals(p.a)) && (this.b.Equals(p.b));
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (a.GetHashCode() + b.GetHashCode()) ^ (a.GetHashCode() - b.GetHashCode());
        }
        public override string ToString()
        {
            return a + "-" + b;
        }
    }
}
