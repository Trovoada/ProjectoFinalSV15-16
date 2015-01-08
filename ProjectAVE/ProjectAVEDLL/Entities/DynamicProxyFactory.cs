using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ProjectAVE.Entities;
using System.Threading;
namespace ProjectAVE.Entities
{

        public static class DynamicProxyFactory
        {

            public static T1 MakeProxy<T1>(T1 real, IInvocationHandler interceptor)
            {
                try
                {
                    AssemblyName myAssembly = new AssemblyName("bla");// posteriormente mudar para input
                }
                catch (Exception e)
                {
                    
                    throw;
                }
                AppDomain myDomain = Thread.GetDomain();
                //AssemblyBuilder myAss
                
                TypeBuilder tb = new TypeBuilder();
                MethodInfo[] ms = real.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (MethodInfo m in ms)
                {

                }
                return real;
            }
            //CallInfo ci = new CallInfo(
            //    typeof(Foo).GetMethod("DoIt")),  //list of public methods
            //    real, 
            //    new object[]{s});
            //   return (int)handler.Oncall(ci); //

            //example
            //class Proxy : Foo
            //{
            //    IInvocationHandler handler;
            //    public Proxy(IInvocationHandler handler) { this.handler = handler; }
            //    public int DoIt(string s) { return s.(); }
            //}
		  
	

        }
    
}
