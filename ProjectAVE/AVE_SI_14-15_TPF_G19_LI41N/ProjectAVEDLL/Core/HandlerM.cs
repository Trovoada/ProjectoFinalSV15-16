using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProjectAVE.Entities;
using System.Threading;
namespace ProjectAVEDLL.Entities
{
    public class HandlerM : IInvocationHandler
    {
        Dictionary<MethodInfo, ProxyContent> Methods;
        public HandlerM(Dictionary<MethodInfo, ProxyContent> Methods)
        {
            this.Methods = Methods;
        }
        public object OnCall(CallInfo info)
        {
            MethodInfo methodinf = info.TargetMethod;
            ProxyContent proxyCont = Methods[methodinf];
            ParameterInfo[] pInfo = methodinf.GetParameters();

            object res;

            if (proxyCont == null) 
                return info.TargetMethod.Invoke(
                                    info.Target,
                                    info.Parameters);
           
            if (proxyCont.DoBefore != null)
                proxyCont.DoBefore.DynamicInvoke(info.Parameters);
            if (proxyCont.Replace != null)
                res = proxyCont.Replace.DynamicInvoke(info.Parameters);
            else
                res = info.TargetMethod.Invoke(
                                    info.Target,
                                    info.Parameters);
            if (proxyCont.DoAfter != null)
                proxyCont.DoAfter.DynamicInvoke(info.Parameters);
            return res;

        }
    }
}
