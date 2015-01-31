﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProjectAVE.Entities;
using System.Threading;

namespace ProjectAVEDLL.Entities
{
    public class SelectMethodProxy<T>
    {

        public Dictionary<MethodInfo, ProxyContent> Methods;

        public SelectMethodProxy(Dictionary<MethodInfo, ProxyContent> m)
        {
            this.Methods = m;

        }

        public FluidProxyBuilder<T> On<Tin, Tret>(Func<Tin, Tret> f)
        {
            if (!Methods.ContainsKey(f.Method)) throw new ArgumentException();

            return new FluidProxyBuilder<T>(Methods, f.Method);
        }

        public FluidProxyBuilder<T> On<T1, T2>(Action<T1, T2> f)
        {
            if (!Methods.ContainsKey(f.Method)) throw new ArgumentException();

            return new FluidProxyBuilder<T>(Methods, f.Method);
        }

    }
}
