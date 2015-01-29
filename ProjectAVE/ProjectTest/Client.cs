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

            Func<String, int> f = real.DoIt;
        }
    }
}
