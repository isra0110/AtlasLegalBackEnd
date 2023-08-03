using Castle.Core.Logging;
using Atlas.Legal.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Reflection;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Collections.Generic;
using Abp.UI;

namespace Atlas.Legal.AtlasWS
{
    public partial class AtlasWSRepository : IAtlasWSRepository
    {
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRestClient _client;

        public AtlasWSRepository(IHostingEnvironment env,
            ILogger logger)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
            _logger = logger;
            _client = new RestClient(_appConfiguration["EndPoints:BaseUrlNotificaciones"]);
        }

        public static bool IsEmpty<T>(List<T> list)
        {
            if (list == null)
            {
                return true;
            }

            return !list.Any();
        }

        /// <summary>
        /// MAHV
        /// Se envian notificaciones vìa correo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NotificacionesOutput> EnviaNotificacion(NotificacionesInput input)
        {
            try
            {
                //CloudEnviaCorreo                   
                string[] valores;
                string sResponseWS = string.Empty;
                NotificacionesOutput RespSalida = new NotificacionesOutput();
                var nHeader = "APPNAME=WSAutosSinAtlas&PRGNAME=CloudEnviaCorreoCristaleras&ARGUMENTS=-A";
                string jsonString = JsonSerializer.Serialize(input);

                input.id_cloud = 2;
                input.numeroReporte = "C0001";

                #region Extraer datos de Archivos
                int iContador = 1;
                bool bNArchivos = true;
                string sInicioArchivos = "[";
                string strArchivos = "";
                if (!IsEmpty(input.archivos) || input.archivos.Count() > 0)
                {
                    foreach (var item in input.archivos)
                    {
                        if (input.archivos.Count != 0 && bNArchivos)
                        {
                            strArchivos = sInicioArchivos + "{";
                            bNArchivos = false;
                        }
                        else
                        {
                            strArchivos += "{";
                        }

                        Type type = item.GetType();
                        PropertyInfo[] props = type.GetProperties();

                        foreach (var prop in props)
                        {
                            strArchivos += ("\"" + prop.Name + "\"" + ":" + "\"" + prop.GetValue(item) + "\"") + "\\,";
                        }

                        if (input.archivos.Count > 1 && iContador < input.archivos.Count())
                        {
                            strArchivos = strArchivos.Remove(strArchivos.Length - 1) + "},";
                        }
                        else
                        {
                            strArchivos = strArchivos.Remove(strArchivos.Length - 1) + "}";
                        }

                        iContador++;
                    }
                    strArchivos = strArchivos.Remove(strArchivos.Length - 2) + "}]";
                }

                var vCorreo = new CorreoInput
                {
                    id_cloud = input.id_cloud,
                    numeroReporte = input.numeroReporte,
                    para = input.para,
                    cc = input.cc,
                    cco = input.cco,
                    mensaje = input.mensaje,
                    asunto = input.asunto,
                    archivos = strArchivos
                };
                #endregion

                Type t = vCorreo.GetType();
                PropertyInfo[] pis = t.GetProperties();
                valores = pis.Select(p => "\"" + p.Name + "\"" + ":" + "\"" + p.GetValue(vCorreo) + "\"").ToArray();
                string delimitados = string.Join("\\,", valores);               
                var nSerialized = nHeader + "{" + delimitados + "}";
                if (!IsEmpty(input.archivos) || input.archivos.Count() > 0)
                {
                    nSerialized = nSerialized.Remove(nSerialized.Length - 2) + "}";
                    nSerialized = nSerialized.Replace("\"[{", "[{");
                    nSerialized = nSerialized.Replace("\"},", "}" + "\",");
                }                                   

                var mRequest = new RestRequest("MagicScripts/MGrqispi.dll", Method.POST);
                mRequest.AddHeader("Content-Type", "text/plain");
                mRequest.AddParameter("text/plain", nSerialized, ParameterType.RequestBody);

                var mResponse = _client.Execute(mRequest);

                if(mResponse.StatusCode.ToString() == "OK")
                {
                    sResponseWS = mResponse.Content;
                    if (sResponseWS.Contains("<HTML>"))
                        throw new Exception("Ocurrio un error al enviar la notificación por correo, favor de validar...");
                    else
                        RespSalida = JsonConvert.DeserializeObject<NotificacionesOutput>(sResponseWS);                  
                }

                return await Task.FromResult(RespSalida);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// MAHV
        /// Se consulta WS de Juridico proporcionado por Atlas
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<JuridicoOutput> ConsultaJuridico(JuridicoInput input)
        {
            string sResponseWS = string.Empty;
            JuridicoOutput RespSalida = new JuridicoOutput();
            var nHeader = "APPNAME=WSAtlas&PRGNAME=WS_Juridico&ARGUMENTS=-A";

            Type t = input.GetType();
            PropertyInfo[] pis = t.GetProperties();
            string valorInput = pis.Select(p => "\"" + p.Name + "\"" + ":" + "\"" + p.GetValue(input) + "\"").FirstOrDefault();
            var nSerialized = nHeader + "{" + valorInput + "}";

            var mRequest = new RestRequest("MagicScripts/MGrqispi.dll", Method.POST);
            mRequest.AddHeader("Content-Type", "text/plain");
            mRequest.AddParameter("text/plain", nSerialized, ParameterType.RequestBody);

            var mResponse = _client.Execute(mRequest);


            sResponseWS = mResponse.Content;
            RespSalida = JsonConvert.DeserializeObject<JuridicoOutput>(sResponseWS);


            return await Task.FromResult(RespSalida);
        }

        /// <summary>
        /// Consulta de Siniestro Condusef
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SiniestroAtlasOutput> ConsultaSiniestros(SiniestroAtlasInput input)
        {
            string sResponseWS = string.Empty;
            SiniestroAtlasOutput RespSalida = new SiniestroAtlasOutput();
            var nHeader = "APPNAME=WSAtlas&PRGNAME=WS_Juridico_Conducef&ARGUMENTS=-A";

            Type t = input.GetType();
            PropertyInfo[] pis = t.GetProperties();
            string[] valores = pis.Select(p => "\"" + p.Name + "\"" + ":" + "\"" + p.GetValue(input) + "\"").ToArray();
            string delimitados = string.Join("\\,", valores);
            var nSerialized = nHeader + "{" + delimitados + "}";

            var mRequest = new RestRequest("MagicScripts/MGrqispi.dll", Method.POST);
            mRequest.AddHeader("Content-Type", "text/plain");
            mRequest.AddParameter("text/plain", nSerialized, ParameterType.RequestBody);

            var mResponse = _client.Execute(mRequest);


            sResponseWS = mResponse.Content;
            RespSalida = JsonConvert.DeserializeObject<SiniestroAtlasOutput>(sResponseWS);


            return await Task.FromResult(RespSalida);
        }

        /// <summary>
        /// Consulta de siniestro para litigio
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<LitigioAtlasOutput> ConsultaLitigio(LitigioAtlasInput input)
        {
            string sResponseWS = string.Empty;
            LitigioAtlasOutput RespSalida = new LitigioAtlasOutput();
            var nHeader = "APPNAME=WSAtlas&PRGNAME=WS_Juridico_Litigio&ARGUMENTS=-A";

            Type t = input.GetType();
            PropertyInfo[] pis = t.GetProperties();
            string valorInput = pis.Select(p => "\"" + p.Name + "\"" + ":" + "\"" + p.GetValue(input) + "\"").FirstOrDefault();
            var nSerialized = nHeader + "{" + valorInput + "}";

            var mRequest = new RestRequest("MagicScripts/MGrqispi.dll", Method.POST);
            mRequest.AddHeader("Content-Type", "text/plain");
            mRequest.AddParameter("text/plain", nSerialized, ParameterType.RequestBody);

            var mResponse = _client.Execute(mRequest);


            sResponseWS = mResponse.Content;
            RespSalida = JsonConvert.DeserializeObject<LitigioAtlasOutput>(sResponseWS);


            return await Task.FromResult(RespSalida);
        }

        /// <summary>
        /// Consulta de Siniestros para Recuperaciones 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RecuperacionesSiniestroOutput> ConsultaRecuperaciones(RecuperacionesSiniestroInput input)
        {
            string sResponseWS = string.Empty;
            RecuperacionesSiniestroOutput RespSalida = new RecuperacionesSiniestroOutput();
            var nHeader = "APPNAME=WSAtlas&PRGNAME=WS_Juridico_Recupera&ARGUMENTS=-A";

            Type t = input.GetType();
            PropertyInfo[] pis = t.GetProperties();
            string[] valores = pis.Select(p => "\"" + p.Name + "\"" + ":" + "\"" + p.GetValue(input) + "\"").ToArray();
            string delimitados = string.Join("\\,", valores);
            var nSerialized = nHeader + "{" + delimitados + "}";

            var mRequest = new RestRequest("MagicScripts/MGrqispi.dll", Method.POST);
            mRequest.AddHeader("Content-Type", "text/plain");
            mRequest.AddParameter("text/plain", nSerialized, ParameterType.RequestBody);

            var mResponse = _client.Execute(mRequest);


            sResponseWS = mResponse.Content;
            RespSalida = JsonConvert.DeserializeObject<RecuperacionesSiniestroOutput>(sResponseWS);


            return await Task.FromResult(RespSalida);
        }

    }
}
