using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest
{
    public interface IHelper
    {
        string Operation(
        IDictionary<int, string> param);
    }

}
