using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.ContratosInputOutput;

namespace Atlas.Legal
{
    public interface IContratosRepository : IRepository<Entity, int>
    {
        Task<RegistroNuevoContratoOutput> RegistroNuevaSolicitud(RegistroNuevoContratoInput input);
        Task<ConsultaContratoOutput> ConsultaContrato(ConsultaContratoInput input);
        string ObtenerObjectIdContrato(ObtenerObjectIdContratoInput input);
        ObtenerIdRegExpContratoOutput ObtenerIdRegExpContrato(int IdContrato);
        void GuardarDocumentoContrato(GuardarDocumentoContratosInput input);
        void GuardarListaArchivosContrato(RegistroNuevoContratoInput input);
        Task<ConsultaSolicitanteOutput> ConsultaSolicitante(ConsultaSolicitanteInput input);
        Task<ConsultaAbogadoOutput> ConsultaAbogado(ConsultaAbogadoInput input);
        Task<DetalleContratoOutput> DetalleSolicitud(DetalleContratoInput input);
        Task<ComentarioContratoOutput> ObtenerComentarioContrato(ComentarioContratoInput input);
        void GuardarComentarioContrato(GuardarComentarioContratoInput input);
        void GuardarTrackingContrato(GuardarTrackingContratoInput input);
        Task<ObtenerTrackingContratoOutput> ObtenerTrackingContrato(ObtenerTrackingContratoInput input);
        Task<ReporteGeneralContratoOutput> ReporteGeneralContrato(ReporteGeneralContratoInput input);
        Task<ReportePrestadoresOutput> ReportePrestadores(ReportePrestadoresInput input);
    }
}
