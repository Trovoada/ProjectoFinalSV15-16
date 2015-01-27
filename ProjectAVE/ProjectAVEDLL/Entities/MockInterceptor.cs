using ProjectAVE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAVE.Entities
{
    public class MockInterceptor : IInvocationHandler
    {
        public object OnCall(CallInfo info)
        {
            // just a mock interceptor
            // that always returns the same value
            Console.WriteLine("mock entrance");
            return "some text";
        }
    }

}