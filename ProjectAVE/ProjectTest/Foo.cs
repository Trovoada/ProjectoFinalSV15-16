using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest
{
    public class Foo
    {
        public virtual int DoIt(String v)
        {
            Console.WriteLine(
            "AClass.DoIt() with {0}",
            v
            );
            return v.Length;
        }

        public virtual int DoIt2(String v)
        {
            Console.WriteLine(
            "BClass.DoIt() with {0}",
            v
            );
            return v.Length;
        }


    }
}
