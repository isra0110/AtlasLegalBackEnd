using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal.Content
{

    public class GuardarDocumentoInput
    {
        public int? IdRegExp { get; set; }
        public int? IdTipoDocumental { get; set; }
        public int? IdTipoExpediente { get; set; }
        public string ExpedienteLlave { get; set; }
        public string DocumentoLlave { get; set; }
        public string ObjectId { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string NodeType { get; set; }
        public string Folder { get; set; }        
        public string Base64 { get; set; }
        public bool EsGuardado { get; set; }
        public Dictionary<string, string> atributos { get; set; }
    }

    public partial class ContentRepository
    {
        private class LoginInput
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        private class LoginOutput
        {
            public string token { get; set; }
            public string refreshToken { get; set; }
            public bool changePassword { get; set; }
            public InfUsuarioModel infUsuario { get; set; }

            public class InfUsuarioModel
            {
                public int idUser { get; set; }
                public string apellidoPaterno { get; set; }
                public string apellidoMaterno { get; set; }
                public string nombre { get; set; }
                public string email { get; set; }
                public string loginName { get; set; }
                public bool bloqueado { get; set; }
                public DateTime fechaCreacion { get; set; }
            }
        }

        private class CrearExpedienteInput
        {
            public int? idRegExp { get; set; }
            public string nombreRegExp { get; set; }
            public int? idTipoExpediente { get; set; }
            public bool borrar { get; set; }
        }

        private class CrearExpedienteOutput
        {
            public ResultadoModel Resultado { get; set; }

            public class ResultadoModel
            {
                public int idRegExp { get; set; }
                public int? codigoMensaje { get; set; }
                public string mensaje { get; set; }
            }
            
        }        

        private class ObtenerMetadatosOutput
        {
            public ResultadoModel resultado { get; set; }
            public class ResultadoModel
            {
                public bool exito { get; set; }
                public string mensaje { get; set; }
                public dataModel data { get; set; }

                public class dataModel
                {
                    public string file { get; set; }
                }
            }
        }

        private class EliminarMetadatosOutput
        {
            public ResultadoModel resultado { get; set; }

            public class ResultadoModel
            {
                public bool exito { get; set; }
                public string mensaje { get; set; }                
            }
        }

        private class AgregarDocumentoInput
        {
            public int? idRegDoc { get; set; }
            public int? idRegExp { get; set; }
            public int? idTipoDocumental { get; set; }
            public string name { get; set; }
            public string nodeType { get; set; }
            public string folder { get; set; }
            public string ext { get; set; }
            public string file { get; set; }
            public Dictionary<string, string> properties { get; set; }
        }

        private class AgregarDocumentoOutput
        {
            public ResultadoModel Resultado { get; set; }

            public class ResultadoModel
            {
                public bool exito { get; set; }
                public string mensaje { get; set; }
                public dataModel data { get; set; }

                public class dataModel
                {
                    public string id { get; set; }
                }
            }
            
        }

        private class ContentOutputBase<T>
        {
            public HeaderModel header { get; set; }
            public T resultado { get; set; }

            public bool exito { get; set; }
            public string mensaje { get; set; }
            public T data { get; set; }

            public class HeaderModel
            {
                public string codigoOperacionBackend { get; set; }
                public string codigoRespuesta { get; set; }
                public string descripcionRespuesta { get; set; }
                public int tiempoRespuesta { get; set; }
                public DateTime horaRespuesta { get; set; }
                public List<RazonFallaModel> razonesFalla { get; set; }
            }

            public class RazonFallaModel
            {
                public string descripcion { get; set; }
            }
        }
    }

}
