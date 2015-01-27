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
        }
        //public virtual string DoIt()
        //{
        //    Console.WriteLine(
        //    "AClass.DoIt() 2" );
        //    return "ola";
        //    // return 1;
        //}
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
            //string str= (string) proxy.DoIt("12");
            //Console.WriteLine(str);
            ////proxy.DoIt();
            IInvocationHandler mock =  new MockInterceptor();
            Foo mockProxy = DynamicProxyFactory.MakeProxy<Foo>(mock);
            string mockRes = (string)mockProxy.DoIt("adeus");
            Console.WriteLine(mockRes);
            //foreach (MethodInfo mi in proxy.GetType().GetMethods())
            //{
            //    Console.WriteLine(mi.Name + " " + mi.ReturnParameter.Name + " " + mi.IsVirtual);
            //}

            MethodInfo method = mockProxy.GetType().GetMethod("DoIt");
            IInvocationHandler log = new LoggerInterceptor();
            Foo realidade = new Foo();
            object [] pm = { "ola" };
            CallInfo info = new CallInfo(method, mockProxy, pm);
            object ola =  log.OnCall(info);
            Console.WriteLine(ola.ToString());

            MethodInfo method1 = real.GetType().GetMethod("DoIt");
            IInvocationHandler log1 = new LoggerInterceptor();
            object[] pm1 = { "ola1" };
            CallInfo info1 = new CallInfo(method, real, pm);
            object ola1 = logInterceptor.OnCall(info1); // continua com erro no Invoke
            Console.WriteLine(ola1.ToString());
        }
    }
}
