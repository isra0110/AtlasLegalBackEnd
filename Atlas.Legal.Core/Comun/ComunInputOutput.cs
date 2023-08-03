using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlas.Legal
{
    public class BaseOutput
    {
        [IgnoreDataMember]
        public int? CodigoMensaje { get; set; } = 0;
        [IgnoreDataMember]
        public string Mensaje { get; set; } = "Operación Exitosa";
    }

    public class ObtenerReporteOut : BaseOutput
    {
        public List<Dictionary<string, object>> Reporte { get; set; }

        public ObtenerReporteOut()
        {
            Reporte = new List<Dictionary<string, object>>();
        }
    }

    public class DocumentoModel
    {
        public int? IdDocumento { get; set; }
        public string ObjectId { get; set; }
        public int? IdTipoDocumental { get; set; }
        public string Nombre { get; set; }
        public string TipoMime { get; set; }
        public string Contenido { get; set; }
        public string TipoDocumento { get; set; }
        public string SubTipoDocumento { get; set; }
    }
}
