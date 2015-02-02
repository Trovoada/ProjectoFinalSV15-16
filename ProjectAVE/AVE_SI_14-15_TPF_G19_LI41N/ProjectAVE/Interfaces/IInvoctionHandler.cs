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
    public interface IInvocationHandler
    {
        object OnCall(CallInfo info);
    }

}
