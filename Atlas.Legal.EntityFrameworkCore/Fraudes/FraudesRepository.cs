using Abp.Data;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Abp.UI;
using Atlas.Legal.EntityFrameworkCore;
using Atlas.Legal.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.FraudesInputOutput;

namespace Atlas.Legal
{
    public class FraudesRepository : LegalRepositoryBase<Entity, int>, IFraudesRepository
    {
        public FraudesRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base (dbContextProvider, transactionProvider)
        {

        }

        /*
         * MAHV 12/05/2022 
         * WS para el registro de Fraudes en BD
        */
        public async Task<RegistroFraudesOutput> RegistroFraudes(RegistroFraudesInput input)
        {
            try
            {

                var result = new RegistroFraudesOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesRegistro", CommandType.StoredProcedure,
                    new SqlParameter("@idPricedimientoEnContraDe", input.IdProcedimientoEnContraDe),
                    new SqlParameter("@idMultpleViaSolucionUno", input.IdMultipleViasSolucionUno),
                    new SqlParameter("@idMultpleViaSolucionDos", input.IdMultipleViasSolucionDos),
                    new SqlParameter("@idMultpleViaSolucionTres", input.IdMultipleViasSolucionTres),
                    new SqlParameter("@fechaTurnado", input.FechaTurnado),
                    new SqlParameter("@antiguedad", input.Antiguedad),
                    new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                    new SqlParameter("@fechaRegistro", input.FechaRegistro),
                    new SqlParameter("@montoInicialARecuperar", input.MontoInicialARecuperar),
                    new SqlParameter("@montoPresupuestoAuditoria", input.MontoPropuestoAuditoria),
                    new SqlParameter("@montoFinalARecuperar", input.MontoFinalARecuperar),
                    new SqlParameter("@montoDeterminadoAutoridad", input.MontoDeterminadoAutoridad),
                    new SqlParameter("@montoNnegociado", input.MontoNnegociado),
                    new SqlParameter("@idAreaQueIdentifica", input.IdAreaQueIdentifica),
                    new SqlParameter("@honorarioPagado", input.HonorarioPagado),
                    new SqlParameter("@fechaPago", input.FechaPago),
                    new SqlParameter("@fechaCreacion", input.FechaCreacion),
                    new SqlParameter("@numeroFraude", input.NumeroFraude),
                    new SqlParameter("@investigacionJuicioDeCampo", input.investigacionActiva),
                    new SqlParameter("@judicializarLaCDI", input.JudicializarLaCDI),
                    new SqlParameter("@idRegExp", input.IdRegExp));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string msj = reader["Mensaje"].As<string>();
                    if ((reader["ClaveMensaje"].To<int>() == 0 || reader["ClaveMensaje"].To<int>() == 1))
                    {
                        result.Mensaje = "Se guardo el registro con el No. de Folio: " + reader["NumeroFraude"];
                        result.IdFraude = !reader.IsDBNull("IdFraude") ? reader["IdFraude"].To<int>() : 0;
                        result.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : 0;
                        result.NumeroFraude = !reader.IsDBNull("NumeroFraude") ? reader["NumeroFraude"].As<string>() : null;
                    }
                    else
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }

                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        } 
        
        /*
         * MAHV 13/05/2022 
         * WS que realiza el guardado de la causa o problema del Fraude
        */
        public void GuardarCausaProblemaFraudes(GuardarCausaProblemaFraudesInput input)
        {
            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesRegistraCausaProblema", CommandType.StoredProcedure,
                    new SqlParameter("@id", input.Id),
                    new SqlParameter("@numeroFraude", input.NumeroFraude),
                    new SqlParameter("@causaProblema", input.CausaProblema),
                    new SqlParameter("@realizadoPor", input.RealizadoPor),
                    new SqlParameter("@fechaRegistro", input.FechaRegistro));

                var reader = mCommand.ExecuteReader();

                while (reader.Read())
                {
                    string msj = reader["Mensaje"].As<string>();
                    
                    if(!(reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }

                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 13/05/2022
         * WS que realiza el guardado de la asignación del fraude
         */
        public async Task<GuardarAsignacionFraudesOutput> GuardarAsignacionFraudes(GuardarAsignacionFraudesInput input)
        {
            try
            {
                var result = new GuardarAsignacionFraudesOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesRegistraAbogado", CommandType.StoredProcedure,
                    new SqlParameter("@idAbogado", input.IdAbogado),
                    new SqlParameter("@numeroFraude", input.NumeroFraude),
                    new SqlParameter("@esInterno", input.EsInterno),
                    new SqlParameter("@idProveedor", input.IdProveedor),
                    new SqlParameter("@fechaCreacion", input.FechaCreacion));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string msj = reader["Mensaje"].As<string>();

                    if ((reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                    {
                        result.Mensaje = "Se ha realizado la asignación al registro con el No de Folio : " + reader["FolioFraude"];
                    }
                    else
                    {
                        msj = "No se ha podido realizar la asignación, favor de verificar...";
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }

                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 13/05/2022
         * WS para el guardado de documentos en la BD de Atlas-Legal
        */
        public void GuardarDocumentoFraudes(GuardarDocumentoFraudesInput input)
        {
           
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesRegistroDocumentos", CommandType.StoredProcedure,
                new SqlParameter("@idDocumento", input.IdDocumento),
                new SqlParameter("@objeto", input.ObjectId),
                new SqlParameter("@nombre", input.Nombre),
                new SqlParameter("@tipoMime", input.TipoMime),
                new SqlParameter("@tipoDocumento", input.TipoDocumento),
                new SqlParameter("@subtipoDocumento", input.SubTipoDocumento),
                new SqlParameter("@idFraudes", input.IdFraudes),
                new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                new SqlParameter("@fechaRegistro", input.FechaRegistro),
                new SqlParameter("@borrar", input.Borrar));

                var reader = mCommand.ExecuteReader();

                while (reader.Read())
                {
                    string msj = reader["Mensaje"].As<string>();
                    if (!(reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }
                reader.Close();
        }

        /*
         * MAHV 16/05/2022
         * WS para la consulta de los fraudes
         */
        public async Task<ConsultaFraudesOutput> ConsultaFraudes(ConsultaFraudesInput input)
        {
            try
            {
                var result = new ConsultaFraudesOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesConsulta", CommandType.StoredProcedure,
                    new SqlParameter("@numeroFraude", input.NumeroFraude),
                    new SqlParameter("@idProcedimientoEnContraDe", input.IdProcedimientoEnContraDe),
                    new SqlParameter("@idProveedor", input.IdProveedor),
                    new SqlParameter("@fechaRegistroInicial", input.FechaRegistroInicial),
                    new SqlParameter("@fechaRegistroFinal", input.FechaRegistroFinal));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.Fraudes.Add(new ConsultaFraudesOutput.FraudesDto
                    {
                        VerDetalle = !reader.IsDBNull("NumeroFolio") ? reader["NumeroFolio"].As<string>() : null,
                        NoDeFolio = !reader.IsDBNull("NumeroFolio") ? reader["NumeroFolio"].As<string>() : null,
                        ProcedimientoEnContraDe = !reader.IsDBNull("ProcedimientoEnContraDe") ? reader["ProcedimientoEnContraDe"].As<string>() : null,
                        MultiplesViasSolucion = !reader.IsDBNull("MultpleViaSolucionUno") ? reader["MultpleViaSolucionUno"].As<string>() : null,
                        FechaTurnado = !reader.IsDBNull("fechaTurnado") ? reader["fechaTurnado"].To<DateTime>() : (DateTime?) null,
                        Antiguedad = !reader.IsDBNull("antiguedad") ? reader["antiguedad"].To<int>() : (int?) null,
                        MontoInicialARecuperarse = !reader.IsDBNull("montoInicialARecuperar") ? reader["montoInicialARecuperar"].As<string>() : null,
                        MontoPresupuestoAuditoria = !reader.IsDBNull("montoPresupuestoAuditoria") ? reader["montoPresupuestoAuditoria"].As<string>() : null,
                        MontoFinalARecuperar = !reader.IsDBNull("montoFinalARecuperar") ? reader["montoFinalARecuperar"].As<string>() : null,
                        MontoDeterminadoAutoridad = !reader.IsDBNull("montoDeterminadoAutoridad") ? reader["montoDeterminadoAutoridad"].As<string>() : null,
                        MontoNegociado = !reader.IsDBNull("montoNegociado") ? reader["montoNegociado"].As<string>() : null,
                        AreaQueIdentifica = !reader.IsDBNull("AreaQueIdentifica") ? reader["AreaQueIdentifica"].As<string>() : null,
                        InvestigacionDelCampo = !reader.IsDBNull("investigacionJuicioDeCampo") ? reader["investigacionJuicioDeCampo"].To<bool>() : (bool?)null,
                        Proveedor = !reader.IsDBNull("Proveedor") ? JsonConvert.DeserializeObject<Object>(reader["Proveedor"].As<string>()) : null
                });
                }

                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }            
        }

        /*
         * MAHV 17/05/2022 
         * WS para la consulta del detalle del Fraude
        */
        public async Task<DetalleFraudesOutput> DetalleFraudes (DetalleFraudesInput input)
        {
            try
            {
                var result = new DetalleFraudesOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesDetalle", CommandType.StoredProcedure,
                    new SqlParameter("@numeroFraude", input.NumeroFraude));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.IdFraudes = !reader.IsDBNull("idFraudes") ? reader["idFraudes"].To<int>() : (int?)null;
                    result.IdProcedimientoEnContraDe = !reader.IsDBNull("idProcedimientoEnContraDe") ? reader["idProcedimientoEnContraDe"].To<int>() : (int?)null;
                    result.IdMultpleViaSolucionUno = !reader.IsDBNull("idMultpleViaSolucionUno") ? reader["idMultpleViaSolucionUno"].To<int>() : (int?)null;
                    result.IdMultpleViaSolucionDos = !reader.IsDBNull("idMultpleViaSolucionDos") ? reader["idMultpleViaSolucionDos"].To<int>() : (int?)null;
                    result.IdMultpleViaSolucionTres = !reader.IsDBNull("idMultpleViaSolucionTres") ? reader["idMultpleViaSolucionTres"].To<int>() : (int?)null;
                    result.FechaTurnado = !reader.IsDBNull("fechaTurnado") ? reader["fechaTurnado"].To<DateTime>() : (DateTime?)null;
                    result.Antiguedad = !reader.IsDBNull("antiguedad") ? reader["antiguedad"].To<int>() : (int?)null;
                    result.UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null;
                    result.FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : (DateTime?)null;
                    result.MontoInicialARecuperarse = !reader.IsDBNull("montoInicialARecuperar") ? reader["montoInicialARecuperar"].As<string>() : null;
                    result.MontoPresupuestoAuditoria = !reader.IsDBNull("montoPresupuestoAuditoria") ? reader["montoPresupuestoAuditoria"].As<string>() : null;
                    result.MontoFinalARecuperar = !reader.IsDBNull("montoFinalARecuperar") ? reader["montoFinalARecuperar"].As<string>() : null;
                    result.MontoDeterminadoAutoridad = !reader.IsDBNull("montoDeterminadoAutoridad") ? reader["montoDeterminadoAutoridad"].As<string>() : null;
                    result.MontoNegociado = !reader.IsDBNull("montoNegociado") ? reader["montoNegociado"].As<string>() : null;
                    result.IdAreaQueIdentifica = !reader.IsDBNull("idAreaQueIdentifica") ? reader["idAreaQueIdentifica"].To<int>() : (int?)null;
                    result.HonorarioPagado = !reader.IsDBNull("honorarioPagado") ? reader["honorarioPagado"].As<string>() : null;
                    result.FechaPago = !reader.IsDBNull("fechaPago") ? reader["fechaPago"].To<DateTime>() : (DateTime?)null;
                    result.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                    result.InvestigacionJuicioDeCampo = !reader.IsDBNull("investigacionJuicioDeCampo") ? reader["investigacionJuicioDeCampo"].To<bool>() : (bool?)null;
                    result.JudicializarLaCDI = !reader.IsDBNull("judicializarLaCDI") ? reader["judicializarLaCDI"].To<bool>() : (bool?)null;
                    result.Concluir = !reader.IsDBNull("concluir") ? reader["concluir"].To<bool>() : (bool?)null;
                    result.FolioFraude = !reader.IsDBNull("FolioFraude") ? reader["FolioFraude"].As<string>() : null;
                    result.CausaProblema = !reader.IsDBNull("CausaProblema") ? JsonConvert.DeserializeObject<Object>(reader["CausaProblema"].As<string>()) : null;
                    result.Documentos = !reader.IsDBNull("Documentos") ? JsonConvert.DeserializeObject<Object>(reader["Documentos"].As<string>()) : null;
                    result.Asignacion = !reader.IsDBNull("Asignacion") ? JsonConvert.DeserializeObject<Object>(reader["Asignacion"].As<string>()) : null;
                }

                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022
         * WS para concluir el Fraude
        */
        public void ConcluirFraude(ConcluirFraudeInput input)
        {
            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesConcluir", CommandType.StoredProcedure,
                    new SqlParameter("@numeroFraude", input.NumeroFraude),
                    new SqlParameter("@concluir", input.Concluido));

                var reader = mCommand.ExecuteReader();

                while (reader.Read())
                {
                    string msj = reader["Mensaje"].As<string>();

                    if (reader["CodigoMensaje"].To<int>() != 0)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }

                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022
         * WS para el guardado del Tracking de Fraudes
         */
        public void GuardarTrackingFraude(GuardarTrackingFraudeInput input)
        {
            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesRegistroTracking", CommandType.StoredProcedure,
                new SqlParameter("@numeroFraude", input.NumeroFraude),
                new SqlParameter("@fechaRegistro", input.FechaRegistro),
                new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                new SqlParameter("@movimiento", input.Movimiento));

                var reader = mCommand.ExecuteReader();

                while (reader.Read())
                {
                    string msj = reader["Mensaje"].As<string>();

                    if (reader["CodigoMmensaje"].To<int>() != 0)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022
         * WS para el registro del comentario 
         */
        public void GuardarComentarioFraude(GuardarComentarioFraudeInput input)
        {
            try
            {
                EnsureConnectionOpen();
                
                var mCommand = CreateCommand("spFraudesRegistroComentario", CommandType.StoredProcedure,
                new SqlParameter("@numeroFraude", input.NumeroFraude),
                new SqlParameter("@fechaRegistro", input.FechaRegistro),
                new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                new SqlParameter("@comentario", input.Comentario));

                var reader = mCommand.ExecuteReader();

                while (reader.Read())
                {
                    string msj = reader["Mensaje"].As<string>();

                    if (reader["CodigoMmensaje"].To<int>() != 0)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }            
        }

        /*
         * MAHV 17/05/2022
         * WS para obtener los trackings de fraudes
         */
        public async Task<ObtenerTrackingFraudesOutput> ObtenerTrackingFraudes(ObtenerTrackingFraudesInput input)
        {
            try
            {
                var result = new ObtenerTrackingFraudesOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesConsultaTracking", CommandType.StoredProcedure,
                    new SqlParameter("@numeroFraude", input.NumeroFraude));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    var mError = HasColumn(reader, "CodigoMensaje");
                    if (mError)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, reader["CodigoMensaje"].As<string>());
                    }

                    while (reader.Read())
                    {
                        result.Trackings.Add(new ObtenerTrackingFraudesOutput.TrackingDto
                        {
                            IdDemandaTracking = !reader.IsDBNull("idDemandaTracking") ? reader["idDemandaTracking"].To<int>() : (int?)null,
                            NumeroFraude = !reader.IsDBNull("numeroFraude") ? reader["numeroFraude"].As<string>() : null,
                            FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : (DateTime?)null,
                            UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                            Movimiento = !reader.IsDBNull("movimiento") ? reader["movimiento"].As<string>() : null                            
                        });
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 17/05/2022
         * WS que realiza la busqueda de los reportes para Fraudes
         */
        public async Task<ConsultaReportesFraudesOutput> ConsultaReportesFraudes(ConsultaReportesFraudesInput input)
        {
            try
            {
                var result = new ConsultaReportesFraudesOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spFraudesBusquedaReporte", CommandType.StoredProcedure,
                    new SqlParameter("@numeroFraude", input.NumeroFraude),
                    new SqlParameter("@idProcedimientoEnContraDe", input.IdProcedimientoEnContraDe),
                    new SqlParameter("@idProveedor", input.IdProveedor),
                    new SqlParameter("@fechaRegistroInicial", input.FechaRegistroInicial),
                    new SqlParameter("@fechaRegistroFinal", input.FechaRegistroFinal));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.Fraudes.Add(new ConsultaReportesFraudesOutput.ReportesDto
                    {
                        NoDeFolio = !reader.IsDBNull("NumeroFolio") ? reader["NumeroFolio"].As<string>() : null,
                        ProcedimientoEnContraDe = !reader.IsDBNull("ProcedimientoEnContraDe") ? reader["ProcedimientoEnContraDe"].As<string>() : null,
                        MultiplesViasSolucion = !reader.IsDBNull("MultpleViaSolucionUno") ? reader["MultpleViaSolucionUno"].As<string>() : null,
                        FechaTurnado = !reader.IsDBNull("fechaTurnado") ? reader["fechaTurnado"].To<DateTime>() : (DateTime?)null,
                        Antiguedad = !reader.IsDBNull("antiguedad") ? reader["antiguedad"].To<int>() : (int?)null,
                        MontoInicialARecuperarse = !reader.IsDBNull("montoInicialARecuperar") ? reader["montoInicialARecuperar"].As<string>() : null,
                        MontoPresupuestoAuditoria = !reader.IsDBNull("montoPresupuestoAuditoria") ? reader["montoPresupuestoAuditoria"].As<string>() : null,
                        MontoFinalARecuperar = !reader.IsDBNull("montoFinalARecuperar") ? reader["montoFinalARecuperar"].As<string>() : null,
                        MontoDeterminadoAutoridad = !reader.IsDBNull("montoDeterminadoAutoridad") ? reader["montoDeterminadoAutoridad"].As<string>() : null,
                        MontoNegociado = !reader.IsDBNull("montoNegociado") ? reader["montoNegociado"].As<string>() : null,
                        AreaQueIdentifica = !reader.IsDBNull("AreaQueIdentifica") ? reader["AreaQueIdentifica"].As<string>() : null,
                        InvestigacionDelCampo = !reader.IsDBNull("investigacionJuicioDeCampo") ? reader["investigacionJuicioDeCampo"].To<bool>() : (bool?)null,
                        Proveedor = !reader.IsDBNull("Proveedor") ? JsonConvert.DeserializeObject<Object>(reader["Proveedor"].As<string>()) : null
                    });
                }

                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /*
         * MAHV 17/05/2022 
         * WS para descargar el documento seleccionado
         */
        public async Task<ObtenerDocumentoFraudesOutput> ObtenerDocumentoFraudes(ObtenerDocumentoFraudesInput input)
        {
            var result = new ObtenerDocumentoFraudesOutput();
            EnsureConnectionOpen();
            string texto = $"SELECT * FROM OpeFraudesDocumentos WHERE idDocumento = {input.IdDocumento}";
            using (var mCommand = CreateCommand(texto, CommandType.Text))
            {
                var reader = mCommand.ExecuteReader();
                while (reader.Read())
                {
                    result.IdDocumento = !reader.IsDBNull(reader.GetOrdinal("idDocumento")) ? reader["idDocumento"].To<int>() : 0;
                    result.IdFraude = !reader.IsDBNull(reader.GetOrdinal("idFraude")) ? reader["idFraude"].To<int>() : 0;
                    result.Nombre = !reader.IsDBNull(reader.GetOrdinal("nombre")) ? reader["nombre"].As<string>() : null;
                    result.Objeto = !reader.IsDBNull(reader.GetOrdinal("objeto")) ? reader["objeto"].As<string>() : null;
                    result.FechaRegistro = !reader.IsDBNull(reader.GetOrdinal("fechaRegistro")) ? reader["fechaRegistro"].To<DateTime>() : DateTime.MinValue;
                    result.TipoMime = !reader.IsDBNull(reader.GetOrdinal("tipoMime")) ? reader["tipoMime"].As<string>() : null;
                    result.UsuarioRegistro = !reader.IsDBNull(reader.GetOrdinal("usuarioRegistro")) ? reader["usuarioRegistro"].As<string>() : null;
                    result.TipoDocumento = !reader.IsDBNull(reader.GetOrdinal("tipoDocumento")) ? reader["tipoDocumento"].As<string>() : null;
                    result.SubTipoDocumento = !reader.IsDBNull(reader.GetOrdinal("subTipoDocumento")) ? reader["subTipoDocumento"].As<string>() : null;
                }
                reader.Close();
            }

            return result;
        }

        /*
         * MAHV 18/05/2022
         * WS para obtener el id del documento
         */
        public string ObtenerObjectIdFraude(ObtenerObjectIdFraudeInput input)
        {
            string texto = $"SELECT objeto FROM OpeFraudesDocumentos WHERE idDocumento = {input.IdDocumento}";
            string objectId = "";
            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);
            using (var reader = mCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    objectId = !reader.IsDBNull("objeto") ? reader["objeto"].As<string>() : null;
                }
            }
            return objectId;
        }

        /*
         * MAHV 18/05/2022
         * WS para obtener el Id registro expediente del fraude
         */
        public ObtenerIdRegExpFraudeOutput ObtenerIdRegExpFraude(int IdFraude)
        {
            var output = new ObtenerIdRegExpFraudeOutput();
            string texto = $"SELECT idRegExp, numeroFraude FROM OpeFraude WHERE idFraudes = {IdFraude}";

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);
            using (var reader = mCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    output.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                    output.NumeroFraude = !reader.IsDBNull("numeroFraude") ? reader["numeroFraude"].As<string>() : null;
                }
            }
            return output;
        }

        /*
         * MAHV 24/05/2022
         * WS para obtener los comentarios de Fraudes
         */
        public async Task<ObtenerComentariosFraudesOutput> ObtenerComentariosFraudes(ObtenerComentariosFraudesInput input)
        {
            var result = new ObtenerComentariosFraudesOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spFraudesConsultaComentario", CommandType.StoredProcedure,
                new SqlParameter("@numeroFraude", input.NumeroFraude));

            using (var reader = await mCommand.ExecuteReaderAsync())
            {
                var mError = HasColumn(reader, "CodigoMmensaje");
                if (mError)
                {
                    reader.Close();
                    throw new UserFriendlyException(400, reader["CodigoMmensaje"].As<string>());
                }

                while (await reader.ReadAsync())
                {
                    result.Comentarios.Add(new ObtenerComentariosFraudesOutput.ComentarioDto
                    {
                        IdDemandaComentario = !reader.IsDBNull("idDemandaComentario") ? reader["idDemandaComentario"].To<int>() : (int?)null,
                        NumeroFraude = !reader.IsDBNull("numeroFraude") ? reader["numeroFraude"].As<string>() : null,
                        FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : (DateTime?)null,
                        UsuarioRegistro = !reader.IsDBNull("usuarioRregistro") ? reader["usuarioRregistro"].As<string>() : null,
                        Comentario = !reader.IsDBNull("comentario") ? reader["comentario"].As<string>() : null
                    });
                }
            }
            return result;
        }

        /*
         * MAHV 24/05/2022
         * Se obtiene el Object ID
         */
        public string ObtenerObjectIdFraudes(ObtenerObjectIdFraudeInput input)
        {
            string texto = $"SELECT objeto FROM OpeFraudesDocumentos WHERE idDocumento = {input.IdDocumento}";
            string objectId = "";
            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);
            using (var reader = mCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    objectId = !reader.IsDBNull("objeto") ? reader["objeto"].As<string>() : null;
                }
            }
            return objectId;
        }

    }           
}               
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                