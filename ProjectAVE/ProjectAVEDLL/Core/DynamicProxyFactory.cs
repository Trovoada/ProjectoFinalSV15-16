﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ProjectAVE.Entities;
using ProjectAVEDLL.Entities; 
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
                                            type.Name + "proxy" + real.GetHashCode(),
                                            TypeAttributes.Public
                                           );

            tb.SetParent(type);


            FieldBuilder fbInterceptor = tb.DefineField(
        "interceptor",

        interceptor.GetType(),
        FieldAttributes.Private);

            FieldBuilder fbReal = tb.DefineField(
      "real",
      type,
      FieldAttributes.Private);

            FieldBuilder fbMethods = tb.DefineField(
        "Methods",
        typeof(MethodInfo[]),
        FieldAttributes.Private);

            Type[] parameterTypes = { type, typeof(IInvocationHandler), typeof(MethodInfo[]) };

            ConstructorBuilder ctor1 = tb.DefineConstructor(
        MethodAttributes.Public,
        CallingConventions.Standard,
        parameterTypes);

            ILGenerator ctor1IL = ctor1.GetILGenerator();

            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_1);
            ctor1IL.Emit(OpCodes.Stfld, fbReal);
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_2);
            ctor1IL.Emit(OpCodes.Stfld, fbInterceptor);
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_3);
            ctor1IL.Emit(OpCodes.Stfld, fbMethods);
            ctor1IL.Emit(OpCodes.Ret);


            ConstructorInfo ci = typeof(CallInfo).GetConstructor(new Type[] { typeof(MethodInfo), typeof(object), typeof(object[]) });
            MethodInfo onCall = interceptor.GetType().GetMethod("OnCall");
            int idx = 0;
            MethodInfo[] ms = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            IEnumerable<MethodInfo> em = ms.AsQueryable().Where(m => m.IsVirtual);
            MethodInfo[] toConst = em.ToArray<MethodInfo>();

            foreach (MethodInfo m in toConst)
            {
                Type[] paramds = new Type[m.GetParameters().Length];
                int i = 0;
                foreach (ParameterInfo pi in m.GetParameters())
                {
                    paramds[i++] = pi.ParameterType;
                }
                MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                                                    m.Name,
                                                    MethodAttributes.Public | MethodAttributes.Virtual,
                                                    m.ReturnType,
                                                   paramds);

                ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();

                numberGetIL.DeclareLocal(typeof(MethodInfo));
                numberGetIL.DeclareLocal(typeof(object[]));
                numberGetIL.DeclareLocal(typeof(CallInfo));


                //vai buscar o metedo a chamar
                numberGetIL.Emit(OpCodes.Ldarg_0);
                numberGetIL.Emit(OpCodes.Ldfld, fbMethods);
                numberGetIL.Emit(OpCodes.Ldc_I4, idx++);
                numberGetIL.Emit(OpCodes.Ldelem_Ref);
                numberGetIL.Emit(OpCodes.Stloc_0);

                //constroi e preenche o array de argumentos

                numberGetIL.Emit(OpCodes.Ldc_I4, paramds.Length);
                numberGetIL.Emit(OpCodes.Newarr, typeof(object));
                numberGetIL.Emit(OpCodes.Stloc_1);

                for (i = 0; i < paramds.Length; i++)
                {
                    numberGetIL.Emit(OpCodes.Ldloc_1);
                    numberGetIL.Emit(OpCodes.Ldc_I4, i);
                    numberGetIL.Emit(OpCodes.Ldarg, i + 1);
                    if (paramds[i].IsValueType)
                        numberGetIL.Emit(OpCodes.Box, typeof(object));
                    numberGetIL.Emit(OpCodes.Stelem_Ref);
                }

                numberGetIL.Emit(OpCodes.Ldloc_0);
                numberGetIL.Emit(OpCodes.Ldarg_0);
                numberGetIL.Emit(OpCodes.Ldfld, fbReal);
                numberGetIL.Emit(OpCodes.Ldloc_1);
                numberGetIL.Emit(OpCodes.Newobj, ci);
                numberGetIL.Emit(OpCodes.Stloc, 2);

                numberGetIL.Emit(OpCodes.Ldarg_0);
                numberGetIL.Emit(OpCodes.Ldfld, fbInterceptor);

                numberGetIL.Emit(OpCodes.Ldloc, 2);
                numberGetIL.Emit(OpCodes.Callvirt, onCall);
                if (m.ReturnType == typeof(void))
                    numberGetIL.Emit(OpCodes.Pop);


                if (m.ReturnType.IsValueType)
                    numberGetIL.Emit(OpCodes.Unbox_Any, m.ReturnType);
                numberGetIL.Emit(OpCodes.Ret);
            }

            Type t = tb.CreateType();
            

            return (T1)Activator.CreateInstance(t, new object[] { real, interceptor, toConst });
        }

        public static T1 MakeProxy<T1>(IInvocationHandler interceptor)
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

            Type type = typeof(T1);
            TypeBuilder tb = mb.DefineType(
                                            type.Name + "proxy" + interceptor.GetType().Name,
                                            TypeAttributes.Public
                                           );
            if (type.IsInterface)
                tb.AddInterfaceImplementation(type);
            else
                tb.SetParent(type);


            FieldBuilder fbInterceptor = tb.DefineField(
        "interceptor",
                //typeof(IInvocationHandler),
        interceptor.GetType(),
        FieldAttributes.Private);

            Type[] parameterTypes = { typeof(IInvocationHandler) };

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
            ctor1IL.Emit(OpCodes.Stfld, fbInterceptor);
            ctor1IL.Emit(OpCodes.Ret);



            //MethodInfo getType = typeof(Type).GetMethod("GetType");
            // MethodInfo getMethod = typeof(Type).GetMethod("GetMethod", new Type[] { typeof(String) });
            ConstructorInfo ci = typeof(CallInfo).GetConstructor(new Type[] { typeof(MethodInfo), typeof(object), typeof(object[]) });
            MethodInfo onCall = interceptor.GetType().GetMethod("OnCall");

            MethodInfo[] ms = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            IEnumerable<MethodInfo> em = ms.AsQueryable().Where(m => m.IsVirtual);
            MethodInfo[] toConst = em.ToArray<MethodInfo>();

            foreach (MethodInfo m in toConst)
            {
                Type[] paramds = new Type[m.GetParameters().Length];
                int i = 0;
                foreach (ParameterInfo pi in m.GetParameters())
                {
                    paramds[i++] = pi.ParameterType;
                }
                MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                                                    m.Name,
                                                   MethodAttributes.Public | MethodAttributes.Virtual,
                                                    m.ReturnType,
                                                   paramds);

                ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();


                numberGetIL.DeclareLocal(typeof(CallInfo));


                //vai buscar o metedo a chamar

                numberGetIL.Emit(OpCodes.Ldnull);
                numberGetIL.Emit(OpCodes.Ldnull);
                numberGetIL.Emit(OpCodes.Ldnull);
                numberGetIL.Emit(OpCodes.Newobj, ci);
                numberGetIL.Emit(OpCodes.Stloc, 0);

                numberGetIL.Emit(OpCodes.Ldarg_0);
                numberGetIL.Emit(OpCodes.Ldfld, fbInterceptor);

                numberGetIL.Emit(OpCodes.Ldloc, 0);
                numberGetIL.Emit(OpCodes.Callvirt, onCall);

                numberGetIL.Emit(OpCodes.Ret);


            }


            Type t = tb.CreateType();
            return (T1)Activator.CreateInstance(t, new object[] { interceptor });

        }
        public static SelectMethodProxy<T> With<T>()
        {
            MethodInfo[] ms = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance);
            IEnumerable<MethodInfo> em = ms.AsQueryable().Where(m => m.IsVirtual);
            Dictionary<MethodInfo, ProxyContent> toRet = new Dictionary<MethodInfo, ProxyContent>();
            foreach (MethodInfo m in em)
                toRet.Add(m, null);
            return new SelectMethodProxy<T>(toRet);

        }

    }
}
