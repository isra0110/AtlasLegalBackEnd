using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public interface IRecuperacionesRepository : IRepository<Entity, int>
    {
        Task<BuscarSiniestroRecuperacionesOutput> BusquedaRecuperacionSiniestroTransporte(BuscarSiniestroRecuperacionesInput input);
        Task<RegistroDemandaRecuperacionesOutput> RegistroDemandaRecuperaciones(RegistroDemandaRecuperacionesInput input);
        Task<RegistroDemandaRecuperacionesDocumentoOutput> RegistroDemandaRecuperacionesDocumento(RegistroDemandaRecuperacionesDocumentoInput input);
        Task<AsignarAbogadoRecuperacionesOutput> AsignarAbogadoRecuperaciones(AsignarAbogadoRecuperacionesInput.AsignacionModel input);
        Task<ConsultaRecuperacionOutput> ConsultaRecuperacion(ConsultaRecuperacionInput input);
        Task<DetalleRecuperacionOutput> DetalleRecuperacion(DetalleRecuperacionInput input);
        Task<ConcluirRecuperacionOutput> ConcluirRecuperacion(ConcluirRecuperacionInput input);
        Task<AperturarRechazarSiniestroRecuperacionesOut> AperturarRechazarSiniestroRecuperacion(AperturarRechazarSiniestroRecuperacionesInput input);
        Task<ObtenerReporteOut> ObtenerReporte<T>(string storedProcedureName, T input);
        string ObtenerObjectIdRecuperaciones(ObtenerObjectIdRecuperacionesInput input);
        ObtenerIdRegExpRecuperacionesOutput ObtenerIdRegExp(int idDemanda);
        Task<ObtenerDocumentoRecuperacionesOutput> ObtenerDocumentoRecuperaciones(ObtenerDocumentoRecuperacionesInput input);
        void GuardaTrackingRecuperaciones(GuardaTrackingRecuperacionesInput input);
        Task<ObtenerTrackingsRecuperacionesOutput> ObtenerTrackingsRecuperaciones(ObtenerTrackingsRecuperacionesInput input);
        void GuardarComentarioRecuperaciones(GuardarComentarioRecuperacionesInput input);
        Task<ObtenerComentariosRecuperacionesOutput> ObtenerComentariosRecuperaciones(ObtenerComentariosRecuperacionesInput input);
    }
}