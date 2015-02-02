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
}
