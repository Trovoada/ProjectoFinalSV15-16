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
        public virtual int DoIt(String v)
        {
            Console.WriteLine(
            "AClass.DoIt() with {0}",
            v
            );
            return v.Length;
        }


     
    }

    public interface IHelper
    {
        string Operation(
        IDictionary<int, string> param);
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
           
            IInvocationHandler mock =  new MockInterceptor();
            Foo mockProxy = DynamicProxyFactory.MakeProxy<Foo>(mock);

            mockProxy.DoIt("este");

            proxy.DoIt("test");

            IInvocationHandler mockInterceptor = new MockInterceptor();
            IHelper p = DynamicProxyFactory.MakeProxy<IHelper>(
                 mockInterceptor
                 );
            string s = p.Operation(
             new Dictionary<int, string>());

            Foo test = DynamicProxyFactory.With<Foo>()
                                            .On<String, int>(real.DoIt)
                                            .DoBefore<String>(ss => Console.WriteLine(ss))
                                            .DoAfter<String>(ss=>Console.WriteLine(ss.GetType().Name))
                                           // .Replace<String,int>(ss=>ss.Length * 5)
                                            .Make();


            Console.WriteLine(test.DoIt("Adeus"));

        }
    }
}
