using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AVE1
{
    /*FRAMEWORK*/
    public interface IInvocationHandler
    {
        object OnCall(CallInfo info);
    }

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
                mb =
                    ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
            }

            TypeBuilder tb = mb.DefineType(
                real.GetType().Name + "proxy",
                 TypeAttributes.Public);

            MethodInfo[] ms = real.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo m in ms)
            {
                Type[] paramds = new Type[m.GetParameters().Length];
                foreach(ParameterInfo pi in m.GetParameters()){

                }
                        MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                                                           m.Name,
                                                           m.Attributes,
                                                           ,
                                                           Type.EmptyTypes);

                        ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
                        // For an instance property, argument zero is the instance. Load the  
                        // instance, then load the private field and return, leaving the 
                        // field value on the stack.
                        numberGetIL.Emit(OpCodes.Ldarg_0);
                        numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
                        numberGetIL.Emit(OpCodes.Ret);
            }
            return real;
        }
    }

    public class CallInfo
    {
        private MethodInfo _targetMethod;
        private object _target;
        private object[] _parameters;
        public CallInfo(MethodInfo tm, object t, object[] pm)
        {
            _targetMethod = tm;
            _target = t;
            _parameters = pm;
        }
        public MethodInfo TargetMethod
        {
            get { return _targetMethod; }
        }
        public object Target
        {
            get { return _target; }
        }
        public object[] Parameters
        {
            get { return _parameters; }
        }
    }
    /* aux */

    class LoggerInterceptor : IInvocationHandler
    {
        private long start;
        private Stopwatch watch = new Stopwatch();
        public object OnCall(CallInfo info)
        {

            start = watch.ElapsedTicks;

            // call real method using reflection
            object res = info.TargetMethod.Invoke(
            info.Target,
            info.Parameters);

            Console.WriteLine("Executed in {0} ticks",
            watch.ElapsedTicks - start);

            return res;
        }
    }
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

    class Program
    {
        static void Main(string[] args)
        {
            IInvocationHandler logInterceptor = new LoggerInterceptor();
            Foo real = new Foo();
            Foo proxy = DynamicProxyFactory.MakeProxy<Foo>(
                            real,
                            logInterceptor
             );
            proxy.DoIt("12");
        }
    }
}
