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

  
            private static AssemblyName aName;

            private static AssemblyBuilder ab;
            private static ModuleBuilder mb;
            public static T1 MakeProxy<T1>(T1 real, IInvocationHandler interceptor)
            {
                if (aName == null)
                {
                    aName = new AssemblyName("DynamicAssemblyExample");
                    ab =
                        AppDomain.CurrentDomain.DefineDynamicAssembly(
                            aName,
                            AssemblyBuilderAccess.RunAndSave);

                    // For a single-module assembly, the module name is usually 
                    // the assembly name plus an extension.
                    mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
                }
                Type type = real.GetType();
                TypeBuilder tb = mb.DefineType(
                                                type.Name + "proxy",
                                                TypeAttributes.Public
                                               );
                tb.SetParent(type);
                FieldBuilder fbInterceptor = tb.DefineField(
            "interceptor",
            typeof(IInvocationHandler),
            FieldAttributes.Private);

                FieldBuilder fbType = tb.DefineField(
            "type",
            typeof(Type),
            FieldAttributes.Private);


                MethodInfo[] ms = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (MethodInfo m in ms)
                {
                    Type[] paramds = new Type[m.GetParameters().Length];
                    int i = 0;
                    foreach(ParameterInfo pi in m.GetParameters()){
                        paramds[i++] = pi.ParameterType;
                    }
                    MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                                                        m.Name,
                                                        m.Attributes,
                                                        m.ReturnType,
                                                        paramds);

                    ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
                    // For an instance property, argument zero is the instance. Load the  
                    // instance, then load the private field and return, leaving the 
                    // field value on the stack.
                    
                    numberGetIL.Emit(OpCodes.Ret);
                }

                Type t = tb.CreateType();
                return (T1)Activator.CreateInstance(t) ;
            }


            public static int Aux()
            {

                return 1;
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
