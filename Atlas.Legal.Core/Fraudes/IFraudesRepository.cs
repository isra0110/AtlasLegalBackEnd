using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.FraudesInputOutput;

namespace Atlas.Legal
{
    public interface IFraudesRepository : IRepository<Entity, int>
    {
        Task<RegistroFraudesOutput> RegistroFraudes(RegistroFraudesInput input);
        void GuardarCausaProblemaFraudes(GuardarCausaProblemaFraudesInput input);
        Task<GuardarAsignacionFraudesOutput> GuardarAsignacionFraudes(GuardarAsignacionFraudesInput input);
        void GuardarDocumentoFraudes(GuardarDocumentoFraudesInput input);
        Task<ConsultaFraudesOutput> ConsultaFraudes(ConsultaFraudesInput input);
        Task<DetalleFraudesOutput> DetalleFraudes(DetalleFraudesInput input);
        void ConcluirFraude(ConcluirFraudeInput input);
        void GuardarComentarioFraude(GuardarComentarioFraudeInput input);
        void GuardarTrackingFraude(GuardarTrackingFraudeInput input);
        Task<ObtenerTrackingFraudesOutput> ObtenerTrackingFraudes(ObtenerTrackingFraudesInput input);
        Task<ConsultaReportesFraudesOutput> ConsultaReportesFraudes(ConsultaReportesFraudesInput input);
        Task<ObtenerDocumentoFraudesOutput> ObtenerDocumentoFraudes(ObtenerDocumentoFraudesInput input);
        string ObtenerObjectIdFraude(ObtenerObjectIdFraudeInput input);
        ObtenerIdRegExpFraudeOutput ObtenerIdRegExpFraude(int IdFraude);
        Task<ObtenerComentariosFraudesOutput> ObtenerComentariosFraudes(ObtenerComentariosFraudesInput input);

    }
}
