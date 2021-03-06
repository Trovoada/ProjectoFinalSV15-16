﻿using System;
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
                                           .On<String, int>(real.DoIt2)
                                           .DoBefore<String>(ss => Console.WriteLine(ss))
                                           //.On<String, int>(real.DoIt)
                                           //.DoBefore<String>(ss => Console.WriteLine(ss+" ola"))
                                            .Make();


            Console.WriteLine(test.DoIt("Adeus"));
            Console.WriteLine(test.DoIt2("ola"));
            test.ToString();
        }
    }
}

