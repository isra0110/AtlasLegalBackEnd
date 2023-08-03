using Abp.UI;
using Atlas.Legal.Configuration;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal.Content
{ 
    public partial class ContentRepository : IContentRepository
    {
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRestClient _client;

        private string _token;

        public ContentRepository(IHostingEnvironment env,
            ILogger logger)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
            _logger = logger;
            _client = new RestClient(_appConfiguration["EndPoints:BaseUrlWebContent"]);

            Login();
        }

        public void GuardarDocumento(GuardarDocumentoInput input)
        {
            if (input.IdRegExp == 0 || input.IdRegExp == null)
            {

#if (RELEASE)
                var mCrearExpedienteOutput = CrearExpediente(new CrearExpedienteInput
                {
                    nombreRegExp = input.ExpedienteLlave,
                    idTipoExpediente = 1, //TODO: En lo que se define el manejo de documentos y expedientes...
                });
#else
                var mCrearExpedienteOutput = CrearExpediente(new CrearExpedienteInput
                {
                    nombreRegExp = input.ExpedienteLlave,
                    idTipoExpediente = 178, //TODO: En lo que se define el manejo de documentos y expedientes...
                });
#endif

                if (mCrearExpedienteOutput.Resultado.codigoMensaje != 0)
                    throw new UserFriendlyException(mCrearExpedienteOutput.Resultado.mensaje);
                
                input.IdRegExp = mCrearExpedienteOutput.Resultado.idRegExp;
                
            }

            if (input.EsGuardado == true)
            {
                var mAgregarDocumentoOutput = AgregarDocumento(new AgregarDocumentoInput
                {
                    idRegExp = input.IdRegExp, //TODO: Id de expediente, hardcode en lo que se define el manejo de documentos y expedientes...
                    idTipoDocumental = input.IdTipoDocumental, //TODO: Id del tipo documental, hardcode en lo que se define el manejo de documentos y expedientes...
                    name = input.Nombre, //TODO: Investigar en el WebContent que dato enviar...
                    nodeType = input.NodeType, //TODO: Investigar en el WebContent que dato enviar...
                    folder = input.Folder, //Variable
                    ext = input.Extension, //TODO: Extensión del archivo, hardcoede en lo que se define el manejo de documentos y expedientes...
                    properties = input.atributos,
                    file = input.Base64
                });
                if (mAgregarDocumentoOutput.Resultado.exito == false)
                    throw new UserFriendlyException(mAgregarDocumentoOutput.Resultado.mensaje);

                input.ObjectId = mAgregarDocumentoOutput.Resultado.data.id;
            }
            
        
        }

        public void ObtenerDocumento(string objectId, out string contenido)
        {
            if (objectId == null)
                throw new UserFriendlyException("Se requiere el objectId");

            var mObtenerMetadatosOutput = ObtenerMetadatos(objectId);

            if (mObtenerMetadatosOutput.resultado.exito == false)
                throw new UserFriendlyException(mObtenerMetadatosOutput.resultado.mensaje);

            contenido = mObtenerMetadatosOutput.resultado.data.file;
        }

        public void EliminarDocumento(string objectId)
        {
            if (objectId == null)
                throw new UserFriendlyException("Se requiere el objectId");

            var mEliminarMetadatosOutput = EliminarMetadatos(objectId);

            if (mEliminarMetadatosOutput.resultado.exito == false)
                throw new UserFriendlyException(mEliminarMetadatosOutput.resultado.mensaje);
        }

        private ObtenerMetadatosOutput ObtenerMetadatos(string objectId)
        {
            string url = string.Format("xccm/object/get/{0}", objectId);
            var mRequest = new RestRequest(url, Method.GET);
            mRequest.AddHeader("loginName", "maria99");

            return Execute<ObtenerMetadatosOutput>(mRequest);
        }

        private EliminarMetadatosOutput EliminarMetadatos(string objectId)
        {
            string url = string.Format("xccm/object/deleteV2/{0}", objectId);
            var mRequest = new RestRequest(url, Method.DELETE);
            mRequest.AddHeader("loginName", "maria99");

            return Execute<EliminarMetadatosOutput>(mRequest);

        }

        private void Login()
        {
            var mRequest = new RestRequest("login", Method.POST);
            mRequest.AddJsonBody(new LoginInput
            {
                username = _appConfiguration["WebContent:username"],
                password = _appConfiguration["WebContent:password"]
            });
            var mResponse = _client.Execute<ContentOutputBase<LoginOutput>>(mRequest);
            if (mResponse.ErrorException != null)
            {
                const string message = "Error al obtener la respuesta. Revise los detalles para obtern mas informacion.";
                var mException = new Exception(message, mResponse.ErrorException);
                throw mException;
            }
            else
            {
                _token = mResponse.Data.data.token;
            }
        }

        private CrearExpedienteOutput CrearExpediente(CrearExpedienteInput request)
        {
            var mRequest = new RestRequest("expediente/administrar/registroExpedienteV2", Method.POST);
            mRequest.AddJsonBody(request);

            return Execute<CrearExpedienteOutput>(mRequest);
        }
        
        private AgregarDocumentoOutput AgregarDocumento(AgregarDocumentoInput request)
        {
            var mRequest = new RestRequest("xccm/object/createV2", Method.PUT);
            mRequest.AddJsonBody(request);

            mRequest.AddHeader("loginName", "maria99"); //TODO: Investigar en el WebContent que dato enviar...

            return Execute<AgregarDocumentoOutput>(mRequest);
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            _client.Authenticator = new JwtAuthenticator(_token);

            var mResponse = _client.Execute<T>(request);            

            if (mResponse.ErrorException != null)
            {
                const string message = "Error al obtener la respuesta. Revise los detalles para obtern mas informacion.";
                var mException = new Exception(message, mResponse.ErrorException);
                throw mException;
            }
            return mResponse.Data;
        }

    }  
}
