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
    public class FluidProxyBuilder<T> : SelectMethodProxy<T>
    {

        public MethodInfo Selected;


        public FluidProxyBuilder(Dictionary<MethodInfo, ProxyContent> Methods, MethodInfo m)
            : base(Methods)
        {

            this.Selected = m;
        }

        public FluidProxyBuilder<T> DoBefore<T1>(Action<T1> a)
        {

            Methods[Selected].DoBefore = Delegate.Combine(Methods[Selected].DoBefore, a);
            return this;
        }
        public FluidProxyBuilder<T> DoBefore<T1, T2>(Action<T1, T2> a)
        {

            Methods[Selected].DoBefore = Delegate.Combine(Methods[Selected].DoBefore, a);
            return this;
        }

        public FluidProxyBuilder<T> DoAfter<T1>(Action<T1> a)
        {
            Methods[Selected].DoAfter = Delegate.Combine(Methods[Selected].DoAfter, a);
            return this;
        }

        public FluidProxyBuilder<T> DoAfter<T1, T2>(Action<T1, T2> a)
        {
            Methods[Selected].DoAfter = Delegate.Combine(Methods[Selected].DoAfter, a);
            return this;
        }

        public FluidProxyBuilder<T> Replace<T1, T2>(Func<T1, T2> d)
        {
            Methods[Selected].Replace = d;

            return this;
        }

        public FluidProxyBuilder<T> Replace<T1, T2, t3>(Func<T1, T2, t3> d)
        {
            Methods[Selected].Replace = d;

            return this;
        }

        public FluidProxyBuilder<T> Replace<T1, T2>(Action<T1, T2> d)
        {
            Methods[Selected].Replace = d;

            return this;
        }

        public T Make()
        {
            return DynamicProxyFactory.MakeProxy<T>((T)Activator.CreateInstance(typeof(T)), new HandlerM(Methods));
        }

        public override FluidProxyBuilder<T> On<T1, T2>(Action<T1, T2> f)
        {
            if (!Methods.ContainsKey(f.Method)) throw new ArgumentException();
            Methods[f.Method] = new ProxyContent();
            Selected = f.Method;
            return this;
        }

        public override FluidProxyBuilder<T> On<Tin, Tret>(Func<Tin, Tret> f)
        {
            if (!Methods.ContainsKey(f.Method)) throw new ArgumentException();
            Methods[f.Method] = new ProxyContent();
            Selected = f.Method;
            return this;
        }

        public override FluidProxyBuilder<T> On<Tin1, Tin2, Tret>(Func<Tin1, Tin2, Tret> f)
        {
            if (!Methods.ContainsKey(f.Method)) throw new ArgumentException();
            Methods[f.Method] = new ProxyContent();
            Selected = f.Method;
            return this;
        }

    }
}
