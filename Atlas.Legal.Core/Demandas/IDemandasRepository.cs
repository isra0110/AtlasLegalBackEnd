using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public interface IDemandasRepository : IRepository<OpeDemanda, int>
    {
        GuardarActualizarRegistroDemandaOutput GuardarActualizarRegistroDemanda(GuardarActualizarRegistroDemandaInput input);
        EliminarAsignacionOutput EliminarAsignacion(EliminarAsignacionInput input);
        AsignarActualizarAbogadoOutput AsignarActualizarAbogado(AsignarActualizarAbogadoInput input);
        ConsultaRegistroDemandaOutput ConsultaRegistroDemanda(ConsultarRegistroDemandaInput input);
        void GuardarTracking(string numeroDemanda, string usuario, DateTime fechaRegistro, string movimiento);
        void GuardarComentario(GuardarComentarioInput input);
        ConsultaDemandaTrackingOutput ConsultaDemandaTracking(ConsultaDemandaTrackingInput input);
        ConsultaDemandaComentarioOutput ConsultaDemandaComentario(ConsultaDemandaComentarioInput input);
        ObtenerDetalleDemandaOutput ObtenerDetalleDemanda(ObtenerDetalleDemandaInput input);
        ConsultaReporteJuiciosOutput ConsultaReporteJuicios(ConsultaReporteJuiciosInput input);
        ConsultaReporteJuiciosRelevantesOutput ConsultaReporteJuiciosRelevantes(ConsultaReporteJuiciosRelevantesInput input);
        ConsultaReporteJuiciosEnContraAseguradosOutput ConsultaReporteJuiciosEnContraAsegurados(ConsultaReporteJuiciosEnContraAseguradosInput input);
        ConsultaReporteConcluidosOutput ConsultaReporteConcluidos(ConsultaReporteConcluidosInput input);
        ObtenerRegExpOutput ObtenerRegExp(ObtenerRegExpInput input);
        void AgregarActualizarDocumentoLitigio(AgregarActualizarDocumentoLitigioInput input);
        ObtenerObjectIdOutput ObtenerObjectId(ObtenerObjectIdInput input);
        ObtenerDocumentoLitigioOuput ObtenerDocumentoLitigio(ObtenerDocumentoLitigioInput input);
        Task ConcluirDemandaLitigio(ConcluirDemandaLitigioInput input);
    }
}
