using Abp.Dependency;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal.TareasProgramadas
{
    public interface INotificarJob: IJob, ITransientDependency
    {
        Task Execute(IJobExecutionContext context);
    }
}
