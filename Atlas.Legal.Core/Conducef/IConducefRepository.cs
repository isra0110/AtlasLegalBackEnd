using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public interface IConducefRepository : IRepository<Entity, int>
    {
        Task<ConsultaConducefOutput> ConsultaConducef(ConsultaConducefInput input);
        Task<DetalleConducefOutput> DetalleConducef(DetalleConducefInput input);
        Task<ConducefReporteGeneralOutput> ConducefReporteGeneral(ConducefReporteGeneralInput input);
        Task<ConducefReporteTrimestralOutput> ConducefReporteTrimestral(ConducefReporteTrimestralInput input);
        void GuardarTrackingConducef(GuardarTrackingConducefInput input);
        void GuardarSiniestroConducef(GuardarSiniestroConducefInput input);
        void GuardarObservacionConducef(GuardarObservacionConducefInput input);
        void GuardarComentarioConducef(GuardarComentarioConducefInput input);
        void GuardarDocumentoConducef(GuardarDocumentoConducefInput input);
        Task<RegistroConducefOutput> RegistroConducef(RegistroConducefInput input);
        void ConcluirRegistroConducef(ConcluirRegistroConducefInput input);
        void AsignarAbogadoConducef(AsignarAbogadoConducefInput input);
        string ObtenerObjectIdConducef(ObtenerObjectIdConducefInput input);
        ObtenerIdRegExpConducefOutput ObtenerIdRegExpConducef(int IdConducef);
        Task<ObtenerDocumentoConducefOutput> ObtenerDocumentoConducef(ObtenerDocumentoConducefInput input);
        Task<ObtenerTrackingsConducefOutput> ObtenerTrackingsConducef(ObtenerTrackingsConducefInput input);
        Task<ObtenerComentariosConducefOutput> ObtenerComentariosConducef(ObtenerComentariosConducefInput input);
        Task<ObtenerObservacionesConducefOutput> ObtenerObservacionesConducef(ObtenerObservacionesConducefInput input);
        void GuardarSiniestroSisa(GuardarSiniestroConducefInput input);
    }
}
