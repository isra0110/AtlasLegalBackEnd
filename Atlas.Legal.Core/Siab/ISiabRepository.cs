using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public interface ISiabRepository : IRepository<Entity, int>
    {
        Task<GenerarRequerimientoSiabOutput> GenerarRequerimientoSiab(GenerarRequerimientoSiabInput input);
        Task GuardaActualizaDocumentoSiab(GuardaDocumentoSiabInput input);
        Task GuardarTrackingSiab(GuardarTrackingSiabInput input);
        Task<GuardarActualizarRegistroSiabOutput> GuardarActualizarRegistroSiab(GuardarActualizarRegistroSiabInput input);
        Task<BusquedaRequerimientoSiabOutput> BusquedaRequerimientoSiab(BusquedaRequerimientoSiabInput input);
        Task<GuardarSeguimientoRegistroSiabOutput> GuardarSeguimientoRegistroSiab(GuardarSeguimientoRegistroSiabInput input);
        Task<ObtenerIdRegExpSiabOutput> ObtenerIdRegExpSiab(ObtenerIdRegExpSiabInput input);
        Task<ReporteRequerimientoSiabOutput> ReporteRequerimientoSiab(ReporteRequerimientoSiabInput input);
        Task<ObtenerDetalleRequerimientoSiabOutput> ObtenerDetalleRequerimientoSiab(ObtenerDetalleRequerimientoSiabInput input);
        Task GuardarComentarioSiab(GuardarComentarioSiabInput input);
        Task<ObtenerObjectIdSiabOutput> ObtenerObjectIdSiab(ObtenerObjectIdSiabInput input);
        Task<ObtenerTrackingsSiabOutput> ObtenerTrackingsSiab(ObtenerTrackingsSiabInput input);
        Task<ObtenerComentariosSiabOutput> ObtenerComentariosSiab(ObtenerComentariosSiabInput input);
        Task<ObtenerDocumentoSiabOutput> ObtenerDocumentoSiab(ObtenerDocumentoSiabInput input);
        Task<ValidacionSiabOutput> ValidacionSiab(ValidacionSiabInput input);
    }
}
