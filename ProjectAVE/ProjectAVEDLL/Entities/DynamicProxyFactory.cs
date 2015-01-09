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

                FieldBuilder fbReal = tb.DefineField(
          "real",
          type,
          FieldAttributes.Private);

                Type[] parameterTypes = {type, typeof(IInvocationHandler) };

                ConstructorBuilder ctor1 = tb.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            parameterTypes);

                ILGenerator ctor1IL = ctor1.GetILGenerator();
                // For a constructor, argument zero is a reference to the new 
                // instance. Push it on the stack before calling the base 
                // class constructor. Specify the default constructor of the  
                // base class (System.Object) by passing an empty array of  
                // types (Type.EmptyTypes) to GetConstructor.

                ctor1IL.Emit(OpCodes.Ldarg_0);
                ctor1IL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
                ctor1IL.Emit(OpCodes.Ldarg_0);
                ctor1IL.Emit(OpCodes.Ldarg_1);
                ctor1IL.Emit(OpCodes.Stfld, fbReal);
                ctor1IL.Emit(OpCodes.Ldarg_0);
                ctor1IL.Emit(OpCodes.Ldarg_2);
                ctor1IL.Emit(OpCodes.Stfld, fbInterceptor);
                ctor1IL.Emit(OpCodes.Ret);
                



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

                    //numberGetIL.Emit(OpCodes.);
                    numberGetIL.Emit(OpCodes.Ret);
                }
                Minha a = new Minha(real, interceptor);
               // a.Ola("adeus");

                Type t = tb.CreateType();
                return (T1)Activator.CreateInstance(t,new object[] {real, interceptor});
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

        public class Minha
        {
            private IInvocationHandler este;
            private Object esta;

            public Minha(Object real, IInvocationHandler arg1)
            {
                esta = real;
                este = arg1;
            }

            public virtual void Ola(String a)
            {
                int fgsd;

                int b;

                b = 56;
                fgsd = 2 * b;
                CallInfo ci = new CallInfo(esta.GetType().GetMethod("Ola", new Type[] { typeof(String) }),  //list of public methods
                this, 
                new object[]{a});
               este.OnCall(ci);
               b = 56;
            }

            public virtual void Ola(int a)
            {
                
                CallInfo ci = new CallInfo(typeof(Minha).GetMethod("Ola", new Type[] {typeof(int)}),  //list of public methods
                this,
                new object[] { a });
                este.OnCall(ci);
            }

            public virtual void Ola(int a, double v)
            {

                CallInfo ci = new CallInfo(typeof(Minha).GetMethod("Ola", new Type[] { typeof(int), typeof(double) }),  //list of public methods
                this,
                new object[] { a });
                este.OnCall(ci);
            }

        }
}
