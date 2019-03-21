using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuOa
{
    public interface IWorkjob
    {
        bool DoWork();
    }

    public interface IConsumer
    {
        object Recieve();
    }
}
