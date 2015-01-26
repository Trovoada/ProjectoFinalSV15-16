using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectAVEDLL;
using ProjectAVE.Entities;
using System.Reflection;
using ProjectAVEDLL.Entities;
namespace ProjectTest
{

    /****************TEST***************/
    public class Foo
    {
        public virtual object DoIt(String v)
        {
            Console.WriteLine(
            "AClass.DoIt() with {0}",
            v
            );
            return "ola";
           // return 1;
        }
    }

    class Client
    {
        static void Main(string[] args)
        {
            IInvocationHandler logInterceptor = new LoggerInterceptor();
            //Foo real = new Foo();
            //Foo proxy = DynamicProxyFactory.MakeProxy<Foo>(
            //                real,
            //                logInterceptor
            // );
            //proxy.DoIt("12");
            IInvocationHandler mock =  new MockInterceptor();
            Foo mockProxy = DynamicProxyFactory.MakeProxy<Foo>(mock);
            string mockRes = (string)mockProxy.DoIt("adeus");
            Console.WriteLine(mockRes);
            //foreach (MethodInfo mi in proxy.GetType().GetMethods())
            //{
            //    Console.WriteLine(mi.Name + " " + mi.ReturnParameter.Name + " " + mi.IsVirtual);
            //}
        }
    }
}
