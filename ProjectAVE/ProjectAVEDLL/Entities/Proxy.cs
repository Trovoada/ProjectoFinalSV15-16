using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectAVEDLL.Interfaces;

namespace ProjectAVEDLL.Entities
{
    public class Proxy<T> : Subject<T>
    {
        private RealSubject<T> subject;
        public virtual void Request() { } //TODO
    }
}
