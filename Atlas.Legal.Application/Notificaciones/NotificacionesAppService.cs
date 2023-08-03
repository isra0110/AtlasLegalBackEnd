using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Atlas.Legal.AtlasWS;
using Atlas.Legal.Autorizacion;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal
{
    public class NotificacionesAppService : LegalAppServiceBase
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IAtlasWSRepository _atlasWSRepository;
        private readonly ISiniestrosRepository _siniestrosRepository;

        public NotificacionesAppService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IObjectMapper objectMapper,
            IAtlasWSRepository atlasWSRepository,
            ISiniestrosRepository siniestrosRepository
            ) : base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;
            _atlasWSRepository = atlasWSRepository;
            _siniestrosRepository = siniestrosRepository;
        }

        /// <summary>
        /// WS para el envio de notificaciones por correo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NotificacionesOutput> enviaNotificacion(NotificacionesInput input)
        {
            try
            {
                var result = await _atlasWSRepository.EnviaNotificacion(input);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Envio de notificaciones para Aistencia Legal
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public async Task<NotificacionesOutput> enviaNotificacionAsistencia(DatosNotificacionesInput datos)
        {
            //Consultamos información de notificaciones de SP
            datos.Notificacion = false;
            var result = await _siniestrosRepository.ConsultaDatosNotificacion(datos);

            //Asignamos información obtenida al request
            SiniestroMailInput input = new SiniestroMailInput();
            input.NumeroReporte = result.NumeroReporte;
            input.Siniestro = result.NumeroSiniestro;
            input.Cuenta = "SEGUROS\\tATLAS";
            input.Asegurado = result.Asegurado;
            input.Entidad = result.Entidad;
            input.Municipio = result.Municipio;
            input.FechaApertura = result.FechaApertura;
            input.Abogado = result.Abogado;
            input.PersonasLesionadas = (int)result.NumeroLesionados;
            input.Homicidios = (int)result.NumeroFallecidos;
            input.archivos = new List<ArchivosInput>();
            //recorremos los archivos obtenidos de la consulta
            if (result.Documentos != null)
            {
                var documentos = JsonConvert.SerializeObject(result.Documentos);
                var lstDocumentos = JsonConvert.DeserializeObject<List<DocumentoNotificaciones>>(documentos);
                                
                foreach (var doc in lstDocumentos)
                {
                    ArchivosInput archivos = new ArchivosInput();
                    archivos.nombreArchivo = doc.Nombre.Remove(doc.Nombre.Length -4);
                    archivos.extArchivo = "." + doc.TipoMime;
                    archivos.archivo = doc.Objeto;
                    input.archivos.Add(archivos);
                }
            }

            char cEspacio = '\t';
            
            try
            {  
                string strBody = "<table border=1><thead>\\t<h4>El\\texpediente\\t" + input.NumeroReporte + "\\treporta\\tun\\tHomicidio:</h4>\\t</thead><br>\\t<tbody><tr><td>Cuenta:</td><td>" + input.Cuenta.Replace(' ', cEspacio) + "</td></tr><tr><td>Nuestro\\tusuario:</td><td>" + input.Asegurado.Replace(' ', cEspacio) + "</td></tr><tr><td>Siniestro:</td><td>" + input.Siniestro + "</td></tr>" +
                    "<tr><td>Entidad:</td><td>" + input.Entidad.Replace(' ', cEspacio) + "</td></tr><tr><td>Municipio:</td><td>" + input.Municipio.Replace(' ', cEspacio) + "</td></tr><tr><td>Fecha\\tde\\tapertura:</td><td>" + input.FechaApertura.ToString().Replace(' ', cEspacio) + "</td></tr><tr><td>Abogado:</td><td>" + input.Abogado.Replace(' ', cEspacio) + "</td></tr>" +
                    "<tr><td>Personas\\tlesionadas:</td><td>" + input.PersonasLesionadas + "</td></tr><tr><td>#\\tde\\tHomicidios:</td><td>" + input.Homicidios + "</td></tr></tbody>\\t</table>";
                
                

                NotificacionesInput notificacionesInput = new NotificacionesInput();
                notificacionesInput.para = "fencchtm@gmail.com";
                notificacionesInput.asunto = "LESIONES\\tPróximo\\ta\\tVencer";
                notificacionesInput.cc = null;
                notificacionesInput.cco = null;
                notificacionesInput.mensaje = strBody;
                notificacionesInput.archivos = input.archivos;

                var resultWS = await _atlasWSRepository.EnviaNotificacion(notificacionesInput);
                return resultWS;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Envio de notificaciones Conducef
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NotificacionesOutput> enviaNotificacionConducef(DatosCondusefNotificacion input)
        {
            try
            {
                char cEspacio = '\t';

                NotificacionesInput notificaciones = new NotificacionesInput();
                notificaciones.para = "miguel.herrera@ies-systems.com";
                notificaciones.cc = null;
                notificaciones.cco = null;
                notificaciones.asunto = "RECORDATORIO:\\tTérmino\\t" + input.FechaTermino.ToString().Replace(' ', cEspacio) + "\\,\\t" + input.RamoBusqueda.Replace(' ', cEspacio) + "\\,\\t" + input.Dependencia.Replace(' ', cEspacio) + "\\,\\t" + input.Partes.Replace(' ', cEspacio) + "\\,\\tSin." + input.Siniestro.Replace(' ', cEspacio) + ".";
                notificaciones.mensaje = "El\\tpresente\\tes\\tun\\tatento\\trecordatorio\\tdel\\t" + input.NumeroExpediente.Replace(' ', cEspacio) + "\\tde\\tfecha\\t" + input.Solantec.ToString().Replace(' ', cEspacio) + "\\,\\trespecto\\tdel\\trequerimiento\\ten\\tel\\tasunto\\tindicado\\,\\tya\\tque\\tel\\ttérmino\\tpara\\tatenderlo\\tvence\\tel\\tdía\\t" + input.FechaTermino.ToString().Replace(' ', cEspacio) + "\\,\\t" +
                                    "es\\tdecir\\,\\tdentro\\tde\\t" + input.DiasTermino + "\\tdías\\thábiles\\,\\tsiendo\\tel\\tcaso\\tque\\thabiéndolos\\tbuscado\\,\\tno\\tpudimos\\tlocalizar\\tlos\\tcomentarios\\ty\\tdocumentación\\trespectivos\\,\\tasí\\tlas\\tcosas\\,\\tsolicitamos\\tnuevamente\\tsu\\tatención\\tproporcionándonos\\ttales\\tcomentarios\\ty\\tdocumentación\\,\\tnecesarios\\tconocer\\ty\\tpronunciarnos\\ten\\t" +
                                    "tiempo\\ty\\tforma\\tsobre\\tel\\trequerimiento\\tque\\tnos\\tocupa\\ta\\tefecto\\tde\\tbrindar\\tla\\tatención\\tpertinente\\,\\tevitando\\tcon\\tello\\tser\\tsancionados.<br><br><br><br>" +
                                    "Quedamos\\ta\\tsus\\tórdenes\\ty\\tal\\tpendiente\\tde\\tsus\\tnoticias.";

                notificaciones.archivos = new List<ArchivosInput>();

                var resultWS = await _atlasWSRepository.EnviaNotificacion(notificaciones);
                return resultWS;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Envio de Alerta para requermientos no registrados en SIAB
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NotificacionesOutput> enviaAlertaRequerimientosSiab(DatosRequerimientoSiabNotificacion input)
        {
            try
            {
                char cEspacio = '\t';

                NotificacionesInput notificaciones = new NotificacionesInput();
                notificaciones.para = "miguel.herrera@ies-systems.com";
                notificaciones.cc = null;
                notificaciones.cco = null;
                notificaciones.asunto = "REQUERIMIENTOS\\tSIAB\\tNO\\tREGISTRADOS" + input.FechaRecepcion.ToString().Replace(' ', cEspacio) + ".";
                notificaciones.mensaje = "El\\tmotivo\\tdel\\tpresente\\tes\\tsolicitar\\tsu\\tpronta\\tatención\\tsobre\\tlos\\t" + input.NumeroRequerimientos + "requerimientos\\trelativos\\tal\\tSIAB\\trecibidos\\tel\\tdía\\t" + input.FechaRecepcion.ToString().Replace(' ', cEspacio) + "\\,\\tya\\tque\\tal\\tno\\thaberse\\tcompletado" +
                                        "\\tel\\tregistro\\tde\\tlos\\tmismos\\,\\tno\\tes\\tposible\\tpor\\ttanto\\tiniciar\\tel\\tproceso\\tde\\trevisión\\tpor\\tlas\\tdemas\\táreas\\tinvolucradas.<br><br><br><br>" +
                                        "Así\\tlas\\tcosas\\,\\tagradecemos\\tsu\\tinmediata\\tatención\\tal\\trespecto\\,\\trealizando\\tel\\tregistro\\tde\\tlos\\trequerimientos\\tpendientes\\,\\tsubiendo\\ttanto\\tla\\tinformación\\ty\\tdocumnetación\\trelativa\\tal\\tSistema\\tJurídico\\tde\\tSeguros\\tAtlas\\,\\tcomo\\tla\\trespuesta\\tprevia\\tal\\tSIAB\\,\\t" +
                                        "a\\tefecto\\tde\\tbrindar\\tla\\tatención\\tpertinente\\,\\tevitando\\tcon\\tello\\tser\\tsancionados.<br><br><br><br>" +
                                        "Quedamos\\ta\\tsus\\tórdenes\\ty\\ten\\tespera\\tde\\tsus\\tnoticias.<br><br><br><br>" +
                                        "Atentamente\\,<br><br><br><br>" +
                                        "Gerencia\\tCONDUSEF.";

                notificaciones.archivos = new List<ArchivosInput>();

                var resultWS = await _atlasWSRepository.EnviaNotificacion(notificaciones);
                return resultWS;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Envio de notificación de requerimientos registrados SIAB
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NotificacionesOutput> enviaRequerimientosRecibidosSiab(DatosRequerimientoSiabNotificacion input)
        {
            try
            {
                char cEspacio = '\t';

                NotificacionesInput notificaciones = new NotificacionesInput();
                notificaciones.para = "miguel.herrera@ies-systems.com";
                notificaciones.cc = null;
                notificaciones.cco = null;
                notificaciones.asunto = "INFORMATIVO\\tSIAB:\\tRequerimientos\\tSIAB\\trecibidos\\tel\\t" + input.FechaRecepcion.ToString().Replace(' ', cEspacio) + ".";
                notificaciones.mensaje = "Por\\tmedio\\tdel\\tpresente\\thacemos\\tde\\tsu\\tconocimiento\\tque\\tse\\tencuentra\\ta\\tsu\\tdisposición\\ten\\tla\\tliga\\tadjunta\\tlos\\t" + input.NumeroRequerimientos + "requerimientos\\ten\\tel\\tbloque\\trequerimientos\\trelativos\\tal\\tSIAB\\trecibidos\\tel\\tdía\\t" + input.FechaRecepcion.ToString().Replace(' ', cEspacio) + "\\,\\t" +
                                        "considerando\\tque\\tla\\tfecha\\tpara\\tdar\\trespuesta\\ta\\tla\\tCONDUSEF\\trespecto\\tde\\tlos\\tmismo\\tes\\ta\\tmás\\ttardar\\tel\\tdía\\t" + input.FechaTermino.ToString().Replace(' ', cEspacio) + "\\,\\tes\\tdecir\\,\\tdentro\\tde\\t" + input.DiasHabilesTermino + "días\\thábiles\\,\\tpor\\tlo\\tque\\tle\\tagradeceremos\\t" +
                                        "su\\tamable\\tatención\\tal\\trespecto\\,\\tproporcionándonos\\tla\\tinformación\\ty\\ten\\tsu\\tcaso\\tdocumentación\\,\\tnecesarios\\tconocer\\ty\\tpronunciarnos\\ten\\ttiempo\\ty\\tforma\\tsobre\\tdichos\\ta\\tefecto\\tde\\tbrindar\\tla\\tatención\\tpertinente\\,\\tevitando\\tcon\\tello\\tser\\tsancionados.<br><br><br><br>" +
                                        "Quedamos\\ta\\tsus\\tórdenes\\ty\\tatentos\\ta\\tsus\\tnoticias.<br><br><br><br>" +
                                        "Atentamente\\,<br><br><br><br>" +
                                        "Gerencia\\tCONDUSEF.";

                notificaciones.archivos = new List<ArchivosInput>();

                var resultWS = await _atlasWSRepository.EnviaNotificacion(notificaciones);
                return resultWS;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Envio de recordatorio para requerimientos SIAB
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NotificacionesOutput> enviaRecordatorioRequerimientosSiab(DatosRequerimientoSiabNotificacion input)
        {
            try
            {
                char cEspacio = '\t';

                NotificacionesInput notificaciones = new NotificacionesInput();
                notificaciones.para = "miguel.herrera@ies-systems.com";
                notificaciones.cc = null;
                notificaciones.cco = null;
                notificaciones.asunto = "Recordatorio\\trequerimientos\\tSIAB\\trecibidos\\tel\\t" + input.FechaRecepcion.ToString().Replace(' ', cEspacio) + ".";
                notificaciones.mensaje = "El\\tpresente\\tes\\tun\\trecordatorio\\trespecto\\tde\\tlos\\trequerimientos\\trecibidos\\tel\\tdía\\t" + input.FechaRecepcion.ToString().Replace(' ', cEspacio) + "considerando\\tque\\tla\\tfecha\\tpara\\tdar\\trespuesta\\ta\\tla\\tCONDUSEF\\trespecto\\tde\\tlos\\tmismo\\tes\\ta\\tmás\\ttardar\\t" +
                                        "el\\tdía" + input.FechaTermino.ToString().Replace(' ', cEspacio) + "\\,\\tes\\tdecir\\,\\tdentro\\tde\\t" + input.DiasHabilesTermino + "\\,\\tsiendo\\tel\\tcaso\\tque\\thabiéndolos\\tbuscado\\,\\tno\\tpudimos\\tlocalizar\\tla\\tinformación\\ty\\,\\ten\\tsu\\tcaso\\,\\tdocumentación\\trespectivos\\," +
                                        "\\tasí\\tlas\\tcosas\\,\\tsolicitamos\\tnuevamente\\tsu\\tatención\\tproporcionandonos\\ttal\\tinformación\\ty\\tdocumentación\\,\\tnecesarios\\tconocer\\ty\\tpronunciarnos\\ten\\ttiempo\\ty\\tforma\\tsobre\\tlos\\trequerimientos\\tque\\tnos\\tocupan\\ta\\tefecto\\tde\\tbrindar\\tla\\tatención\\tpertinente\\,\\tevitando\\tcon\\tello\\tser\\tsancionados" +
                                        "Quedamos\\ta\\tsus\\tórdenes\\ty\\tal\\tpendiente\\tde\\tsus\\tnoticias.<br><br><br><br>" +
                                        "Atentamente\\,<br><br><br><br>" +
                                        "Gerencia\\tCONDUSEF.";

                notificaciones.archivos = new List<ArchivosInput>();

                var resultWS = await _atlasWSRepository.EnviaNotificacion(notificaciones);
                return resultWS;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<NotificacionesOutput> EnviarNotificacion(NotificacionesInput input)
        {
            try
            {
                var result = await _atlasWSRepository.EnviaNotificacion(input);
                return result;
            }
            catch (Exception e)
            {
                throw e;
                throw;
            }
        }

    }
}
