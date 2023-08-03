using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public interface ISiniestrosAppService : IApplicationService
    {
        Task<SiniestroOutput> CrearActualizarSiniestroProveedor(SiniestroInput input);
        //Task<SiniestroSisaOutput> CrearActualizarSiniestroSisa(SiniestroSisaInput input);
        Task<SiniestroSisaOutput> CrearActualizarSiniestroSisa(SiniestroSISAInput input);
        Task<SiniestroPrometeoOutput> CrearActualizarSiniestroPrometeo(SiniestroPrometeoInput input);        
        BusquedaSolicitudOutput BusquedaSolicitud(BusquedaSolicitudInput input);
        DetalleSolicitudOutput DetalleSolicitud(DetalleSolicitudInput input);
        Task<GuardaBitacoraSolicitudOutput> GuardaBitacoraSolicitud(GuardaBitacoraSolicitudInput input);
        ConsultaBitacoraOutput ConsultaBitacora(ConsultaBitacoraInput input);
        List<Dictionary<string, object>> BusquedaReporte(BusquedaReporteInput input);
        ObtenerDocumentoSiniestroOutput ObtenerDocumentoSiniestro(ObtenerDocumentoSiniestroInput input);
        Task<AgregarActualizarDocumentoSiniestroOutput> AgregarActualizarDocumentoSiniestro(AgregarActualizarDocumentoSiniestroInput input);
    }
}