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
    public  class LoggerInterceptor : IInvocationHandler
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
}
