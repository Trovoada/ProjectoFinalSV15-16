using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectAVEDLL;
using ProjectAVE.Entities;
using System.Reflection;
namespace ProjectTest
{

    /****************TEST***************/
    public class Foo
    {
        public virtual void DoIt(String v, int d)
        {
            Console.WriteLine(
            "AClass.DoIt() with {0}",
            v
            );
            //return v.Length;
        }
    }

    class Client
    {
        static void Main(string[] args)
        {
            IInvocationHandler logInterceptor = new LoggerInterceptor();
            Foo real = new Foo();
            Foo proxy = DynamicProxyFactory.MakeProxy<Foo>(
                            real,
                            logInterceptor
             );
            proxy.DoIt("12", 12);
            foreach (MethodInfo mi in proxy.GetType().GetMethods())
            {
                Console.WriteLine(mi.Name);
            }
        }
    }
}
