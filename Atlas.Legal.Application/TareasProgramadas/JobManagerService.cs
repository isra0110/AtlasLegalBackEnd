using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Quartz;
using Atlas.Legal.Comun;
using Castle.Core.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal.TareasProgramadas
{
    public class JobManagerService : IJobManagerService
    {
        private const string JOBS = "JOBS";
        private readonly ILogger _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IQuartzScheduleJobManager _jobManager;
        private readonly IRepository<CatParametro, string> _catParametroRepository;

        public JobManagerService(
            ILogger logger,
            IUnitOfWorkManager unitOfWorkManager,
            IQuartzScheduleJobManager jobManager,
            IRepository<CatParametro, string> catParametroRepository
            )
        {
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
            _jobManager = jobManager;
            _catParametroRepository = catParametroRepository;
        }

        public async Task ProgramarJobs()
        {
            var mParametro = _catParametroRepository.Get(JOBS);
            if (mParametro != null)
            {
                var mValoresJob = mParametro.Valor.Split('|');
                var mJobGrupo = mValoresJob[0];
                var mJobNombre = mValoresJob[1];
                var mJobHorario = mValoresJob[2];
                await _jobManager.ScheduleAsync<INotificarJob>(
                    job =>
                    {
                        job.WithIdentity(mJobNombre, mJobGrupo)
                        .WithDescription(JOBS);
                    },
                    trigger =>
                    {
                        trigger.StartNow()
                            .WithSchedule(CronScheduleBuilder.CronSchedule(mJobHorario))
                            .Build();
                    });
                _logger.Info(string.Format("Tarea {0}.{1} programada, con horario CRON: {2}", mJobGrupo, mJobNombre, mJobHorario));
            }
        }
    }
}
