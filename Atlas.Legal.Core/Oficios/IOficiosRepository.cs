using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public interface IOficiosRepository : IRepository<Entity, int>
    {
        Task<RegistraOficioOutput> RegistraOficio(RegistraOficioInput input);
        Task<RegistraOficioDocumentoOutput> RegistraOficioDocumento(RegistraOficioDocumentoInput input);
        Task<AsignarAbogadoOficiosOutput> AsignarAbogadoOficios(AsignarAbogadoOficiosInput.AsignacionModel input);
        Task<ConsultaOficioOutput> ConsultaOficio(ConsultaOficioInput input);
        Task<DetalleOficioOutput> DetalleOficio(DetalleOficioInput input);
        Task<ConcluirOficioOutput> ConcluirOficio(ConcluirOficioInput input);
        Task<ObtenerReporteOut> ObtenerReporte<T>(string storedProcedureName, T input);
        Task<ObtenerTrackingsOficioOutput> ObtenerTrackingsOficio(ObtenerTrackingsOficioInput input);
        void GuardaTrackingOficio(GuardaTrackingOficioInput input);
        Task<ObtenerComentariosOficiosOutput> ObtenerComentariosOficios(ObtenerComentariosOficiosInput input);
        void GuardaComentarioOficios(GuardaComentarioOficiosInput input);
        string ObtenerObjectIdOficios(ObtenerObjectIdOficiosInput input);
        ObtenerIdRegExpOficiosOutput ObtenerIdRegExp(int idOficio);
        Task<ObtenerDocumentoOficiosOutput> ObtenerDocumentoOficios(ObtenerDocumentoOficiosInput input);
    }
}