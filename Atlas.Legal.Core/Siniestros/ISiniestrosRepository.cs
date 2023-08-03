using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal
{
    public interface ISiniestrosRepository : IRepository<Siniestro, int>
    {
        void InsertOrUpdateSiniestro(SiniestroInput input);
        void InserOrUpdateSiniestroSisa(JuridicoOutput input, int? IdRegExp);
        void InsertOrUpdateRecuperacionSisa(RecuperacionesInput input);
        void InsertOrUpdatePagoTercerosSisa(PagoTercerosInput input);
        void InsertOrUpdateSiniestroPrometeo(SiniestroPrometeoInput input);        
        BusquedaSolicitudOutput BusquedaSolicitud(BusquedaSolicitudInput input);
        DetalleSolicitudOutput DetalleSolicitud(DetalleSolicitudInput input);
        void GuardaBitacora(string noSiniestroAtlas, string noReporte, string usuario, int idModificacionIntegracion);
        ConsultaBitacoraOutput ConsultaBitacora(ConsultaBitacoraInput input);
        List<Dictionary<string, object>> BusquedaReporte(BusquedaReporteInput input);
        void GuardarActualizarDocumento(int? idDocumento, int idSiniestro, string nombre, string objectId, string tipoMime, string tipoDocumento, string subTipoDocumento, string usuario, bool borrar);
        ObtenerDocumentoSiniestroOutput ObtenerDocumentoSiniestro(ObtenerDocumentoSiniestroInput input);
        void ConsultarIdRegExp(int idSiniestro, out int idRegExp, out string numSiniestro);
        void ObtenerIdRegExp(string numeroSiniestroAtlas, out int idRegExp);
        Task<DatosNotificacionesOutput> ConsultaDatosNotificacion(DatosNotificacionesInput input);
        Task<InformacionAsistenciaNotificacionOutput> InformacionAsistenciaNotificacion(InformacionAsistenciaNotificacionInput input);
        Task<DatosVehiculosDetenidosAlertaOutput> ObtenerDatosVehiculosDetenidosAlerta(int idSiniestro);
        Task<string> ObtenerNumeroReporteProveedor(string numeroReporteCabina);

        Task GuardaBitacoraEventos(BitacoraEventosInput input);
    }
}
