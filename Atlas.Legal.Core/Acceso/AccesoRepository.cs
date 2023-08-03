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


namespace Atlas.Legal.Acceso
{
    public partial class AccesoRepository : IAccesoRepository
    {
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRestClient _client;        

        public AccesoRepository(IHostingEnvironment env,
            ILogger logger)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
            _logger = logger;
            _client = new RestClient(_appConfiguration["EndPoints:BaseUrlAccess"]);
        }

        public ValidarAccesoWSOutput ValidarAccesoWS(ValidarAccesoWSInput acceso)
        {
            var mRequest = new RestRequest("api/account/login", Method.POST);
                mRequest.AddJsonBody(acceso);

            var mResponse = _client.Execute<ValidarAccesoWSOutput>(mRequest);
            mResponse.Data.HttpStatusCode = mResponse.StatusCode;

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
