using Abp.Dependency;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal.TareasProgramadas
{
    public interface IJobManagerService : IDomainService, ITransientDependency
    {
        Task ProgramarJobs();
    }
}
