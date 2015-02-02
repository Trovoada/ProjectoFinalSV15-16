using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ProjectAVE.Entities;
using ProjectTest;
using ProjectAVEDLL.Entities;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AVEUnitTest
{
    [TestClass]
    public class TestClass
    {
        [TestMethod]
        public void CREATE_TYPE()
        {
            IInvocationHandler logInterceptor = new LoggerInterceptor();
            Foo real = new Foo();
            Foo proxy = DynamicProxyFactory.MakeProxy<Foo>(
                            real,
                            logInterceptor
             );
          Assert.AreEqual(proxy.DoIt("test"), 4);

        }
        [TestMethod]
        public void CREATE_TYPE_INTERFACE()
        {
            IInvocationHandler mock = new MockInterceptor();
            Foo mockProxy = DynamicProxyFactory.MakeProxy<Foo>(mock);
            //mockProxy.DoIt("este");
            Assert.AreEqual(typeof(Int32), mockProxy.DoIt("este").GetType()); //retorna o tamanho da string
        }
       [TestMethod]
        public void CREATE_INTERFACE()
        {
            IInvocationHandler mockInterceptor = new MockInterceptor();
            IHelper p = DynamicProxyFactory.MakeProxy<IHelper>(
                 mockInterceptor
                 );
            Assert.AreNotEqual(typeof(IHelper), p.GetType()); // do tipo novo%
        }
    }
}
