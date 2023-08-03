using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Atlas.Legal.AtlasWS;
using Atlas.Legal.Comun;
using Atlas.Legal.Notificacion;
using Castle.Core.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal.TareasProgramadas
{
    public class NotificarJob : INotificarJob
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IObjectMapper _objectMapper;
        private readonly INotificacion _notificacion;
        private readonly IRepository<OpeNotificacion, int> _opeNotificacionRepository;
        private readonly ISiniestrosRepository _siniestroRepository;
        private readonly IRepository<CatPlantillaCorreo, int> _catPlantillaCorreoRepository;
        private readonly IAtlasWSRepository _atlasWSRepository;
        
        public NotificarJob(ILogger logger,
            IUnitOfWorkManager unitOfWorkManager,
            IObjectMapper objectMapper,
            INotificacion notificacion,
            ISiniestrosRepository siniestroRepository,
            IRepository<CatPlantillaCorreo, int> catPlantillaCorreoRepository,
            IAtlasWSRepository atlasWSRepository,
            IRepository<OpeNotificacion, int> opeNotificacionRepository) : base()
        {
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
            _objectMapper = objectMapper;
            _notificacion = notificacion;
            _opeNotificacionRepository = opeNotificacionRepository;
            _siniestroRepository = siniestroRepository;
            _catPlantillaCorreoRepository = catPlantillaCorreoRepository;
            _atlasWSRepository = atlasWSRepository;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            await _notificacion.GenerarNotificaciones();
            await EnviarNotificacionesPendientesAsistencia();
            await EnviarNotificacionVehiculosDetenidosLiberacionVehiculo();
        }

        public async Task EnviarNotificacionesPendientesAsistencia()
        {
            var mNotificacion = _opeNotificacionRepository.FirstOrDefault(n => !n.EstaEnviada && (n.IdPlantillaCorreo == 2 || n.IdPlantillaCorreo == 4));

            while (mNotificacion != null)
            {
                try
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await NotificarAsistenciaLegal(mNotificacion.IdEntidad, mNotificacion.IdPlantillaCorreo);
                        mNotificacion.EstaEnviada = true;
                        mNotificacion.FechaEnvio = DateTime.Now;
                        _opeNotificacionRepository.Update(mNotificacion);
                        unitOfWork.Complete();
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(string.Format("NotificarJob.EnviarNotificacionesPendientes -> Error al enviar notificaciones."), e);
                    throw e;
                }
                mNotificacion = _opeNotificacionRepository.FirstOrDefault(n => !n.EstaEnviada && n.IdPlantillaCorreo == 2);
            }
        }

        public async Task EnviarNotificacionVehiculosDetenidosLiberacionVehiculo()
        {
            var mNotificacion = _opeNotificacionRepository.FirstOrDefault(n => !n.EstaEnviada && (n.IdPlantillaCorreo == 5 || n.IdPlantillaCorreo == 6 || n.IdPlantillaCorreo == 7 || n.IdPlantillaCorreo == 8));

            while (mNotificacion != null)
            {
                try
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await NotificarVehiculosDetenidosLiberacionVehiculo(mNotificacion.IdEntidad, mNotificacion.IdPlantillaCorreo);
                        mNotificacion.EstaEnviada = true;
                        mNotificacion.FechaEnvio = DateTime.Now;
                        _opeNotificacionRepository.Update(mNotificacion);
                        unitOfWork.Complete();
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(string.Format("NotificarJob.EnviarNotificacionesPendientes -> Error al enviar notificaciones."), e);
                    throw e;
                }
                mNotificacion = _opeNotificacionRepository.FirstOrDefault(n => !n.EstaEnviada && (n.IdPlantillaCorreo == 4 || n.IdPlantillaCorreo == 5 || n.IdPlantillaCorreo == 6 || n.IdPlantillaCorreo == 7));
            }
        }


        public async Task NotificarAsistenciaLegal(int idEntidad, int idPlantillaCorreo)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var mRegistro = await _siniestroRepository.InformacionAsistenciaNotificacion(new InformacionAsistenciaNotificacionInput { IdSiniestro = idEntidad });
                    mRegistro.Cuenta = "SEGUROS ATLAS";
                    var mPlantilla = _catPlantillaCorreoRepository.Get(idPlantillaCorreo);
                    var mArchivos = new List<ArchivosInput>();

                    if (mPlantilla != null)
                    {
                        var mCuerpo = PlantillaManager.LlenarPlantilla(mPlantilla.Plantilla, mRegistro);

                        var caracteres = mCuerpo.Split(' ');
                        var mCuerpoNuevo = string.Join("\\t", caracteres);

                        string mAsunto = null;

                        if(mRegistro.IdProveedor == 1)
                            mAsunto = $"El expediente {mRegistro.NumeroReporte} {mPlantilla.Asunto} - AUSA";
                        else if (mRegistro.IdProveedor == 26)
                            mAsunto = $"El expediente {mRegistro.NumeroReporte} {mPlantilla.Asunto} - INFOASIST";
                        else
                            mAsunto = $"El expediente {mRegistro.NumeroReporte} {mPlantilla.Asunto}";


                        caracteres = mAsunto.Split(' ');
                        var mAsuntoNuevo = string.Join("\\t", caracteres);


                        caracteres = mPlantilla.Destinatarios.Split(' ');
                        string mDest = string.Join("\\t", caracteres);


                        var resultCorreo = _atlasWSRepository.EnviaNotificacion(new AtlasWSOutput.NotificacionesInput
                        {
                            asunto = mAsuntoNuevo,
                            para = mDest,
                            mensaje = mCuerpoNuevo,
                            numeroReporte = mRegistro.NumeroReporte,
                            archivos = mArchivos
                        });

                        if (resultCorreo.Result.RespSalida.estatus_resp != "1" )
                            throw new Exception("Un error se ha presentado");
                        

                        _logger.Info(string.Format("NotificarJob.Notificar -> Notificacion {0} enviada correctamente por correo, Numero de siniestro:{1}, Numero de reporte:{2}.", mPlantilla.Nombre, mRegistro.NumeroSiniestroAtlas, mRegistro.NumeroReporte));
                    }
                    unitOfWork.Complete();
                }
            }
            catch (Exception e)
            {
                _logger.Error(string.Format("NotificarJob.Notificar -> Error al enviar notificación idPlantillaCorreo:{0}, IdContrato:{1}.", idPlantillaCorreo, idEntidad), e);
                throw e;
            }
        }

        public async Task NotificarVehiculosDetenidosLiberacionVehiculo(int idEntidad, int idPlantillaCorreo)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var mSolicitud = await _siniestroRepository.ObtenerDatosVehiculosDetenidosAlerta(idEntidad);
                    var mPlantilla = _catPlantillaCorreoRepository.Get(idPlantillaCorreo);
                    var mArchivos = new List<ArchivosInput>();

                    if (mPlantilla != null)
                    {
                        var mCuerpo = PlantillaManager.LlenarPlantilla(mPlantilla.Plantilla, mSolicitud);

                        var caracteres = mCuerpo.Split(' ');
                        var mCuerpoNuevo = string.Join("\\t", caracteres);

                        string mAsunto = null;

                        if (mSolicitud.IdProveedor == 1)
                            mAsunto = $"{mPlantilla.Asunto} {mSolicitud.NumeroReporte} - AUSA";
                        else if (mSolicitud.IdProveedor == 26)
                            mAsunto = $"{mPlantilla.Asunto} {mSolicitud.NumeroReporte} - INFOASIST";
                        else 
                            mAsunto = $"{mPlantilla.Asunto} {mSolicitud.NumeroReporte}";

                        caracteres = mAsunto.Split(' ');
                        var mAsuntoNuevo = string.Join("\\t", caracteres);

                        string mDestinararios = null;

#if (!RELEASE)
                        mDestinararios = mPlantilla.Destinatarios;
#else
                        if (mSolicitud.IdProveedor == 1)
                            mDestinararios = "amstraffon@segurosatlas.com.mx";
                        else if (mSolicitud.IdProveedor == 8)
                            mDestinararios = "eperez@segurosatlas.com.mx";
#endif

                        caracteres = mDestinararios.Split(' ');
                        string mDest = string.Join("\\t", caracteres);


                        var resultCorreo = _atlasWSRepository.EnviaNotificacion(new AtlasWSOutput.NotificacionesInput
                        {
                            asunto = mAsuntoNuevo,
                            para = mDest,
                            mensaje = mCuerpoNuevo,
                            numeroReporte = mSolicitud.NumeroReporte,
                            archivos = mArchivos
                        });

                        _logger.Info(string.Format("NotificarJob.Notificar -> Notificacion {0} enviada correctamente por correo, Numero de siniestro:{1}, Numero de reporte:{2}.", mPlantilla.Nombre, mSolicitud.NumeroSiniestroAtlas, mSolicitud.NumeroReporte));
                    }
                    unitOfWork.Complete();
                }
            }
            catch (Exception e)
            {
                _logger.Error(string.Format("NotificarJob.Notificar -> Error al enviar notificación idPlantillaCorreo:{0}, IdContrato:{1}.", idPlantillaCorreo, idEntidad), e);
                throw e;
            }
        }
    }
}
