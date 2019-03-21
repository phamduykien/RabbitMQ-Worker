using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuOa
{
    public class WorkerManager
    {

       


        public WorkerProcessAgent Create(IWorkjob job, IConsumer consumer)
        {
            WorkerProcessAgent process = new WorkerProcessAgent();
            return process;
        }
    }
}
