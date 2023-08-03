using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal
{
    public interface IDemandasAppService : IApplicationService
    {
        GuardarActualizarRegistroDemandaOutput GuardarActualizarRegistroDemanda(GuardarActualizarRegistroDemandaInput input);
        EliminarAsignacionOutput EliminarAsignacion(EliminarAsignacionInput input);
        AsignarActualizarAbogadoOutput AsignarActualizarAbogado(AsignarActualizarAbogadoInput input);
        ConsultaRegistroDemandaOutput ConsultaRegistroDemanda(ConsultarRegistroDemandaInput input);
        Task<GuardarComentarioOutput> GuardarComentario(GuardarComentarioInput input);
        ConsultaDemandaTrackingOutput ConsultaDemandaTracking(ConsultaDemandaTrackingInput input);
        ConsultaDemandaComentarioOutput ConsultaDemandaComentario(ConsultaDemandaComentarioInput input);
        ObtenerDetalleDemandaOutput ObtenerDetalleDemanda(ObtenerDetalleDemandaInput input);
        ConsultaReporteJuiciosOutput ConsultaReporteJuicios(ConsultaReporteJuiciosInput input);
        ConsultaReporteJuiciosRelevantesOutput ConsultaReporteJuiciosRelevantes(ConsultaReporteJuiciosRelevantesInput input);
        ConsultaReporteConcluidosOutput ConsultaReporteConcluidos(ConsultaReporteConcluidosInput input);
        Task<AgregarActualizarDocumentoLitigioOutput> AgregarActualizarDocumentoLitigio(AgregarActualizarDocumentoLitigioInput input);
        ObtenerDocumentoLitigioOuput ObtenerDocumentoLitigio(ObtenerDocumentoLitigioInput input);
        Task<ConcluirDemandaLitigioOutput> ConcluirDemandaLitigio(ConcluirDemandaLitigioInput input);
        Task<LitigioAtlasOutput> BuscarSiniestroLitigio(LitigioAtlasInput input);
    }
}
