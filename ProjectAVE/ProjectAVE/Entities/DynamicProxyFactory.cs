using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ProjectAVE.Entities;

namespace ProjectAVE.Entities
{

        public static class DynamicProxyFactory
        {

            public static T1 MakeProxy<T1>(T1 real, IInvocationHandler interceptor)
            {
                TypeBuilder tb = new TypeBuilder();
                MethodInfo[] ms = real.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (MethodInfo m in ms)
                {

                }
                return real;
            }
        }
    
}
