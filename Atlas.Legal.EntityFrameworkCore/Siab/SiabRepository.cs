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
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public class SiabRepository : LegalRepositoryBase<Entity, int>, ISiabRepository
    {
        public SiabRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider, transactionProvider)
        {

        }

        public async Task<GenerarRequerimientoSiabOutput> GenerarRequerimientoSiab(GenerarRequerimientoSiabInput input)
        {
            var result = new GenerarRequerimientoSiabOutput();

            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabRegistroRequerimiento", CommandType.StoredProcedure,
                    new SqlParameter("@requerimientosDia", input.RequerimientosDelDia),
                    new SqlParameter("@fechaElaboracionRequerimiento", input.FechaElaboracionRequerimientos),
                    new SqlParameter("@fechaCreacion", DateTime.Now),
                    new SqlParameter("@usuario", input.UsuarioRegistro));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string msj = reader["Mensaje"].As<string>();
                        if (reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1)
                        {
                            result.Mensaje = "Operacion Exitosa";
                            result.Requerimientos = !reader.IsDBNull("Requerimiento") ? JsonConvert.DeserializeObject<Object>(reader["Requerimiento"].As<string>()) : null;
                        }
                        else
                        {
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task GuardaActualizaDocumentoSiab(GuardaDocumentoSiabInput input)
        {
            try
            {
                EnsureConnectionOpen();
                var mCommand = CreateCommand("spSiabRegistraDocumentos", CommandType.StoredProcedure,
                    new SqlParameter("@idDdocumento", input.IdDocumento),
                    new SqlParameter("@idRegistro", input.IdRegistro),
                    new SqlParameter("@nombre", input.Nombre),
                    new SqlParameter("@tipoMime", input.TipoMime),
                    new SqlParameter("@tipoDocumento", input.TipoDocumento),
                    new SqlParameter("@subTipoDocumento", input.SubTipoDocumento),
                    new SqlParameter("@objeto", input.ObjectId),
                    new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                    new SqlParameter("@fechaRegistro", input.FechaRegistro),
                    new SqlParameter("@borrar", input.Borrar));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string msj = reader["Mensaje"].As<string>();
                        if (!(reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                        {
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task GuardarTrackingSiab(GuardarTrackingSiabInput input)
        {
            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabRegistroTracking", CommandType.StoredProcedure,
                    new SqlParameter("@numeroSiab", input.NumeroSiab),
                    new SqlParameter("@fechaRegistro", input.FechaRegistro),
                    new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                    new SqlParameter("@movimiento", input.Movimiento));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string msj = reader["Mensaje"].As<string>();
                        if (reader["CodigoMmensaje"].To<int>() != 0)
                        {
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GuardarActualizarRegistroSiabOutput> GuardarActualizarRegistroSiab(GuardarActualizarRegistroSiabInput input)
        {
            var result = new GuardarActualizarRegistroSiabOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spSiabRegistro/Actualiza", CommandType.StoredProcedure,
                new SqlParameter("@numeroRegistro", input.NumeroRegistro),
                new SqlParameter("@fechaRecepcion", input.FechaRecepcion),
                new SqlParameter("@fechaTermino", input.FechaTermino),
                new SqlParameter("@nombreAsegurado", input.NombreAsegurado),
                new SqlParameter("@fechaNacimiento", input.FechaNacimiento),
                new SqlParameter("@fechaDefuncion", input.FechaDefuncion),
                new SqlParameter("@sexo", input.Sexo),
                new SqlParameter("@folioSio", input.FolioSio),
                new SqlParameter("@idEntidad", input.IdIdentidad),
                new SqlParameter("@cp", input.cp),
                new SqlParameter("@nombreBeneficiario", input.NombreBeneficiario),
                new SqlParameter("@respuestaPrevia", input.RespuestaPrevia),
                new SqlParameter("@idRespuesta", input.IdRespuesta),
                new SqlParameter("@fechaRespuesta", input.FechaRespuesta),
                new SqlParameter("@idRegExp", input.IdRegExp),
                new SqlParameter("@poliza", input.Poliza),
                new SqlParameter("@actualizar", input.Actualizar));

            using (var reader = await mCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string msj = reader["Mensaje"].As<string>();
                    int iCodigo = reader["CodigoMensaje"].To<int>();
                    if ((reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                    {
                        result.Mensaje = "Operacion Exitosa";
                        result.IdRegistro = !reader.IsDBNull("IdRegistro") ? reader["IdRegistro"].To<int>() : (int?)null;
                        result.IdRegExp = !reader.IsDBNull("IdRegExp") ? reader["IdRegExp"].To<int>() : (int?)null;
                        result.NumeroRegistro = !reader.IsDBNull("numeroRegistro") ? reader["numeroRegistro"].As<string>() : null;
                        result.Estatus = !reader.IsDBNull("Estatus") ? reader["Estatus"].As<string>() : null;
                        result.FechaElaboracion = !reader.IsDBNull("FechaElaboracion") ? reader["FechaElaboracion"].To<DateTime>() : (DateTime?)null;

                        var mRespuesta = !reader.IsDBNull("ValorRespuesta") ? reader["ValorRespuesta"].As<string>() : null;

                        switch (input.IdRespuesta)
                        {
                            case 1:
                                result.ValorRespuesta = string.Format(mRespuesta, input.FolioSio, input.NombreAsegurado, input.FechaNacimiento, input.FechaDefuncion, input.NombreBeneficiario);
                                break;
                            case 2:
                                result.ValorRespuesta = string.Format(mRespuesta, input.FolioSio, input.NombreAsegurado, input.FechaNacimiento, input.FechaDefuncion, input.Poliza, input.NombreBeneficiario);
                                break;
                            case 3:
                                result.ValorRespuesta = string.Format(mRespuesta, input.FolioSio, input.NombreAsegurado, input.FechaNacimiento, input.FechaDefuncion, input.FechaDefuncion, input.NombreBeneficiario);
                                break;
                            case 4:
                                result.ValorRespuesta = string.Format(mRespuesta, input.FolioSio, input.NombreAsegurado, input.FechaNacimiento, input.FechaDefuncion, input.Poliza, input.NombreBeneficiario);
                                break;
                        }
                    }
                    else
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }
            }
            return result;
        }

        public async Task<BusquedaRequerimientoSiabOutput> BusquedaRequerimientoSiab(BusquedaRequerimientoSiabInput input)
        {
            var result = new BusquedaRequerimientoSiabOutput();


            EnsureConnectionOpen();
            var mCommand = CreateCommand("spSiabBusquedaRequerimiento", CommandType.StoredProcedure,
                new SqlParameter("@fechaElaboracionDesde", input.FechaElaboracionInicio),
                new SqlParameter("@fechaElaboracionHasta", input.FechaElaboracionFin),
                new SqlParameter("@numeroFolio", input.NumeroRegistro),
                new SqlParameter("@fechaValida", DateTime.Now));

            using (var reader = await mCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var mRespuesta = !reader.IsDBNull("ValorRespuesta") ? reader["ValorRespuesta"].As<string>() : null;
                    var idRespuesta = !reader.IsDBNull("idRespuesta") ? reader["idRespuesta"].To<int>() : (int?)null;
                    var folioSio = !reader.IsDBNull("folioSio") ? reader["folioSio"].As<string>() : "";
                    var nombreAsegurado = !reader.IsDBNull("nombreAsegurado") ? reader["nombreAsegurado"].As<string>() : "";
                    var fechaNacimiento = !reader.IsDBNull("fechaNacimiento") ? reader["fechaNacimiento"].As<string>() : "";
                    var fechaDefuncion = !reader.IsDBNull("fechaDefuncion") ? reader["fechaDefuncion"].As<string>() : "";
                    var nombreBeneficiario = !reader.IsDBNull("nombreBeneficiario") ? reader["nombreBeneficiario"].As<string>() : "";
                    var poliza = !reader.IsDBNull("poliza") ? reader["poliza"].As<string>() : "";
                    string valorRespuesta = "";

                    if (idRespuesta != null)
                    {
                        switch (idRespuesta)
                        {
                            case 1:
                                valorRespuesta = string.Format(mRespuesta, folioSio, nombreAsegurado, fechaNacimiento, fechaDefuncion, nombreBeneficiario);
                                break;
                            case 2:
                                valorRespuesta = string.Format(mRespuesta, folioSio, nombreAsegurado, fechaNacimiento, fechaDefuncion, poliza, nombreBeneficiario);
                                break;
                            case 3:
                                valorRespuesta = string.Format(mRespuesta, folioSio, nombreAsegurado, fechaNacimiento, fechaDefuncion, fechaDefuncion, nombreBeneficiario);
                                break;
                            case 4:
                                valorRespuesta = string.Format(mRespuesta, folioSio, nombreAsegurado, fechaNacimiento, fechaDefuncion, poliza, nombreBeneficiario);
                                break;
                        }
                    }


                    result.Requerimientos.Add(new BusquedaRequerimientoSiabOutput.RequerimientoDto
                    {
                        IdRegistro = !reader.IsDBNull("IdRegistro") ? reader["IdRegistro"].To<int>() : (int?)null,
                        NumeroSiab = !reader.IsDBNull("numeroSiab") ? reader["numeroSiab"].As<string>() : null,
                        FechaRegistro = !reader.IsDBNull("FechaRegistro") ? reader["FechaRegistro"].To<DateTime>() : (DateTime?)null,
                        Estatus = !reader.IsDBNull("Estatus") ? reader["Estatus"].As<string>() : null,
                        FechaElaboracion = !reader.IsDBNull("FechaElaboracion") ? reader["FechaElaboracion"].To<DateTime>() : (DateTime?)null,
                        IdRespuesta = idRespuesta,
                        FolioSio = folioSio,
                        NombreAsegurado = nombreAsegurado,
                        FechaNacimiento = fechaNacimiento,
                        FechaDefuncion = fechaDefuncion,
                        NombreBeneficiario = nombreBeneficiario,
                        Poliza = poliza,
                        ValorRespuesta = valorRespuesta,
                        FechaRecepcion = !reader.IsDBNull("fechaRecepcion") ? reader["fechaRecepcion"].To<DateTime>() : (DateTime?)null,
                        FechaRespuesta = !reader.IsDBNull("fechaRespuesta") ? reader["fechaRespuesta"].To<DateTime>() : (DateTime?)null,
                        RespEmsVidaInd = !reader.IsDBNull("respEmsVidaInd") ? reader["respEmsVidaInd"].To<bool>() : (bool?)null,
                        FechaTermino = !reader.IsDBNull("fechaTermino") ? reader["fechaTermino"].To<DateTime>() : (DateTime?)null,
                        Entidad = !reader.IsDBNull("Entidad") ? reader["Entidad"].As<string>() : null,
                        Sexo = !reader.IsDBNull("sexo") ? reader["sexo"].To<bool>() : (bool?)null,
                        cp = !reader.IsDBNull("cp") ? reader["cp"].As<string>() : null,
                        RespuestaSinVida = !reader.IsDBNull("respuestaSinVida") ? reader["respuestaSinVida"].To<bool>() : (bool?)null,
                        RespEmsVidaCol = !reader.IsDBNull("respEmsVidaCol") ? reader["respEmsVidaCol"].To<bool>() : (bool?)null,

                    });
                }
            }
            return result;

        }

        public async Task<GuardarSeguimientoRegistroSiabOutput> GuardarSeguimientoRegistroSiab(GuardarSeguimientoRegistroSiabInput input)
        {
            var result = new GuardarSeguimientoRegistroSiabOutput();

            try
            {
                EnsureConnectionOpen();
                var mCommand = CreateCommand("spSiabSeguimientoRegistro", CommandType.StoredProcedure,
                    new SqlParameter("@numeroRegistro", input.NumeroRegistro),
                    new SqlParameter("@cp", input.cp),
                    new SqlParameter("@editarRespuesta", input.EditarRespuesta),
                    new SqlParameter("@idRespuesta", input.IdRespuesta),
                    new SqlParameter("@respuestaSinVida", input.RespuestaSinVida),
                    new SqlParameter("@comentarioRespSinVida", input.ComentarioRespSinVida),
                    new SqlParameter("@respEmsVidaInd", input.RespEmsVidaInd),
                    new SqlParameter("@respEmsVidaCol", input.RespEmsVidaCol),
                    new SqlParameter("@concluir", input.Concluir),
                    new SqlParameter("@fechaNacimiento", input.FechaNacimiento),
                    new SqlParameter("@comentarioRespVidaInd", input.ComentarioRespVidaInd),
                    new SqlParameter("@comentarioRespVidaCol", input.ComentarioRespVidaCol));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string msj = reader["Mensaje"].As<string>();
                        if (reader["CodigoMensaje"].To<int>() != 0)
                        {
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                        else
                        {
                            result.NumeroRegistro = !reader.IsDBNull("numeroRegistro") ? reader["numeroRegistro"].As<string>() : null;
                            result.Estatus = !reader.IsDBNull("Estatus") ? reader["Estatus"].As<string>() : null;
                            result.FechaElaboracion = !reader.IsDBNull("FechaElaboracion") ? reader["FechaElaboracion"].To<DateTime>() : (DateTime?)null;
                            result.ValorRespuesta = !reader.IsDBNull("ValorRespuesta") ? reader["ValorRespuesta"].As<string>() : null;
                        }
                    }
                }
                return result;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<ObtenerIdRegExpSiabOutput> ObtenerIdRegExpSiab(ObtenerIdRegExpSiabInput input)
        {
            var result = new ObtenerIdRegExpSiabOutput();

            string texto = "";
            if (input.NumeroRegistro != null)
                texto = $"SELECT idRegExp, id FROM OpeSiab WHERE numeroRegistro = '{input.NumeroRegistro}'";
            if (input.IdRegistro != null)
                texto = $"SELECT idRegExp, numeroRegistro FROM OpeSiab WHERE id = {input.IdRegistro}";

            try
            {
                EnsureConnectionOpen();
                var mCommand = CreateCommand(texto, CommandType.Text);

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (input.NumeroRegistro != null)
                        {
                            result.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                            result.IdRegistro = !reader.IsDBNull("id") ? reader["id"].To<int>() : (int?)null;
                        }
                        if (input.IdRegistro != null)
                        {
                            result.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                            result.NumeroRegistro = !reader.IsDBNull("numeroRegistro") ? reader["numeroRegistro"].As<string>() : null;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ReporteRequerimientoSiabOutput> ReporteRequerimientoSiab(ReporteRequerimientoSiabInput input)
        {
            var result = new ReporteRequerimientoSiabOutput();

            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabReporteRequerimiento", CommandType.StoredProcedure,
                    new SqlParameter("@idEstatus", input.IdEstatus),
                    new SqlParameter("@idrespuestaVida", input.IdRespuestaVida),
                    new SqlParameter("@fechaElaboracionDesde", input.FechaElaboracionInicio),
                    new SqlParameter("@fechaElaboracionHasta", input.FechaElaboracionFin));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var mRespuesta = !reader.IsDBNull("ValorRespuesta") ? reader["ValorRespuesta"].As<string>() : null;
                        var idRespuesta = !reader.IsDBNull("idRespuesta") ? reader["idRespuesta"].To<int>() : (int?)null;
                        var folioSio = !reader.IsDBNull("folioSio") ? reader["folioSio"].As<string>() : "";
                        var nombreAsegurado = !reader.IsDBNull("nombreAsegurado") ? reader["nombreAsegurado"].As<string>() : "";
                        var fechaNacimiento = !reader.IsDBNull("fechaNacimiento") ? reader["fechaNacimiento"].As<string>() : "";
                        var fechaDefuncion = !reader.IsDBNull("fechaDefuncion") ? reader["fechaDefuncion"].As<string>() : "";
                        var nombreBeneficiario = !reader.IsDBNull("nombreBeneficiario") ? reader["nombreBeneficiario"].As<string>() : "";
                        var poliza = !reader.IsDBNull("poliza") ? reader["poliza"].As<string>() : "";
                        string valorRespuesta = "";

                        if (idRespuesta != null)
                        {
                            switch (idRespuesta)
                            {
                                case 1:
                                    valorRespuesta = string.Format(mRespuesta, folioSio, nombreAsegurado, fechaNacimiento, fechaDefuncion, nombreBeneficiario);
                                    break;
                                case 2:
                                    valorRespuesta = string.Format(mRespuesta, folioSio, nombreAsegurado, fechaNacimiento, fechaDefuncion, poliza, nombreBeneficiario);
                                    break;
                                case 3:
                                    valorRespuesta = string.Format(mRespuesta, folioSio, nombreAsegurado, fechaNacimiento, fechaDefuncion, fechaDefuncion, nombreBeneficiario);
                                    break;
                                case 4:
                                    valorRespuesta = string.Format(mRespuesta, folioSio, nombreAsegurado, fechaNacimiento, fechaDefuncion, poliza, nombreBeneficiario);
                                    break;
                            }
                        }


                        result.Requerimientos.Add(new ReporteRequerimientoSiabOutput.ReporteDto
                        {
                            NumeroRegistro = !reader.IsDBNull("numeroRegistro") ? reader["numeroRegistro"].As<string>() : null,
                            FechaRecepcion = !reader.IsDBNull("fechaRecepcion") ? reader["fechaRecepcion"].To<DateTime>() : (DateTime?)null,
                            FechaTermino = !reader.IsDBNull("fechaTermino") ? reader["fechaTermino"].To<DateTime>() : (DateTime?)null,
                            NombreAsegurado = nombreAsegurado,
                            FechaNacimiento = fechaNacimiento,
                            FechaDefuncion = fechaDefuncion,
                            Sexo = !reader.IsDBNull("sexo") ? reader["sexo"].As<string>() : null,
                            FolioSIO = folioSio,
                            Entidad = !reader.IsDBNull("Entidad") ? reader["Entidad"].As<string>() : null,
                            cp = !reader.IsDBNull("cp") ? reader["cp"].As<string>() : null,
                            NombreBeneficiario = nombreBeneficiario,
                            RespuestaPrevia = !reader.IsDBNull("respuestaPrevia") ? reader["respuestaPrevia"].As<string>() : null,
                            IdRespuesta = idRespuesta,
                            FechaRespuesta = !reader.IsDBNull("fechaRespuesta") ? reader["fechaRespuesta"].To<DateTime>() : (DateTime?)null,
                            EditarRespuesta = !reader.IsDBNull("editarRespuesta") ? reader["editarRespuesta"].To<bool>() : (bool?)null,
                            RespuestaSinVida = !reader.IsDBNull("respuestaSinVida") ? reader["respuestaSinVida"].As<string>() : null,
                            ComentarioRespSinVida = !reader.IsDBNull("comentarioRespSinVida") ? reader["comentarioRespSinVida"].As<string>() : null,
                            RespEmsVidaInd = !reader.IsDBNull("respEmsVidaInd") ? reader["respEmsVidaInd"].As<string>() : null,
                            RespEmsVidaCol = !reader.IsDBNull("respEmsVidaCol") ? reader["respEmsVidaCol"].As<string>() : null,
                            Estatus = !reader.IsDBNull("Estatus") ? reader["Estatus"].As<string>() : null,
                            Poliza = poliza,
                            ValorRespuesta = valorRespuesta
                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ObtenerDetalleRequerimientoSiabOutput> ObtenerDetalleRequerimientoSiab(ObtenerDetalleRequerimientoSiabInput input)
        {
            var result = new ObtenerDetalleRequerimientoSiabOutput();

            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabDetalle", CommandType.StoredProcedure,
                    new SqlParameter("@numeroRegistro", input.NumeroRegistro));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    var mError = HasColumn(reader, "CodigoMensaje");
                    while (await reader.ReadAsync())
                    {
                        if (!mError)
                        {
                            result.Id = !reader.IsDBNull("id") ? reader["id"].To<int>() : (int?)null;
                            result.NumeroRegistro = !reader.IsDBNull("numeroRegistro") ? reader["numeroRegistro"].As<string>() : null;
                            result.FechaRecepcion = !reader.IsDBNull("fechaRecepcion") ? reader["fechaRecepcion"].To<DateTime>() : (DateTime?)null;
                            result.FechaTermino = !reader.IsDBNull("fechaTermino") ? reader["fechaTermino"].To<DateTime>() : (DateTime?)null;
                            result.NombreAsegurado = !reader.IsDBNull("nombreAsegurado") ? reader["nombreAsegurado"].As<string>() : null;
                            result.FechaNacimiento = !reader.IsDBNull("fechaNacimiento") ? reader["fechaNacimiento"].As<string>() : null;
                            result.FechaDefuncion = !reader.IsDBNull("fechaDefuncion") ? reader["fechaDefuncion"].As<string>() : null;
                            result.Sexo = !reader.IsDBNull("sexo") ? reader["sexo"].To<bool>() : (bool?)null;
                            result.FolioSIO = !reader.IsDBNull("folioSio") ? reader["folioSio"].As<string>() : null;
                            result.IdEntidad = !reader.IsDBNull("idEntidad") ? reader["idEntidad"].To<int>() : (int?)null;
                            result.cp = !reader.IsDBNull("cp") ? reader["cp"].As<string>() : null;
                            result.NombreBeneficiario = !reader.IsDBNull("nombreBeneficiario") ? reader["nombreBeneficiario"].As<string>() : null;
                            result.RespuestaPrevia = !reader.IsDBNull("respuestaPrevia") ? reader["respuestaPrevia"].To<bool>() : (bool?)null;
                            result.IdRespuesta = !reader.IsDBNull("idRespuesta") ? reader["idRespuesta"].To<int>() : (int?)null;
                            result.FechaRespuesta = !reader.IsDBNull("fechaRespuesta") ? reader["fechaRespuesta"].To<DateTime>() : (DateTime?)null;
                            result.Concluir = !reader.IsDBNull("concluir") ? reader["concluir"].To<bool>() : (bool?)null;
                            result.EditarRespuesta = !reader.IsDBNull("editarRespuesta") ? reader["editarRespuesta"].To<bool>() : (bool?)null;
                            result.RespuestaSinVida = !reader.IsDBNull("respuestaSinVida") ? reader["respuestaSinVida"].To<bool>() : (bool?)null;
                            result.ComentarioRespuestaSinVida = !reader.IsDBNull("comentarioRespSinVida") ? reader["comentarioRespSinVida"].As<string>() : null;
                            result.RespEmsVidaInd = !reader.IsDBNull("respEmsVidaInd") ? reader["respEmsVidaInd"].To<bool>() : (bool?)null;
                            result.ComentarioRespVidaInd = !reader.IsDBNull("comentarioRespVidaInd") ? reader["comentarioRespVidaInd"].As<string>() : null;
                            result.respEmsVidaCol = !reader.IsDBNull("respEmsVidaCol") ? reader["respEmsVidaCol"].To<bool>() : (bool?)null;
                            result.ComentarioRespVidaCol = !reader.IsDBNull("comentarioRespVidaCol") ? reader["comentarioRespVidaCol"].As<string>() : null;
                            result.Poliza = !reader.IsDBNull("poliza") ? reader["poliza"].As<string>() : null;
                            result.Documentos = !reader.IsDBNull("Documentos") ? JsonConvert.DeserializeObject<Object>(reader["Documentos"].As<string>()) : null;
                        }
                        else
                        {
                            var msj = reader["Mensaje"].As<string>();
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task GuardarComentarioSiab(GuardarComentarioSiabInput input)
        {
            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabRegistroComentario", CommandType.StoredProcedure,
                    new SqlParameter("@numeroSiab", input.NumeroRegistro),
                    new SqlParameter("@fechaRegistro", input.FechaRegistro),
                    new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                    new SqlParameter("@comentario", input.Comentario));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string msj = reader["Mensaje"].As<string>();
                        if (reader["CodigoMmensaje"].To<int>() != 0)
                        {
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ObtenerObjectIdSiabOutput> ObtenerObjectIdSiab(ObtenerObjectIdSiabInput input)
        {
            string texto = $"SELECT objeto FROM OpeSiabDocumentos WHERE idRegistro = {input.IdDocumento}";
            var result = new ObtenerObjectIdSiabOutput();

            try
            {
                EnsureConnectionOpen();
                var mCommand = CreateCommand(texto, CommandType.Text);
                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.ObjectId = !reader.IsDBNull("objeto") ? reader["objeto"].As<string>() : null;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ObtenerTrackingsSiabOutput> ObtenerTrackingsSiab(ObtenerTrackingsSiabInput input)
        {
            var result = new ObtenerTrackingsSiabOutput();

            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabConsultaDemandaTracking", CommandType.StoredProcedure,
                    new SqlParameter("@numeroSiab", input.NumeroRegistro));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    var mError = HasColumn(reader, "CodigoMmensaje");
                    while (await reader.ReadAsync())
                    {
                        if (mError)
                        {
                            string msj = reader["Mensaje"].As<string>();
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                        else
                        {
                            result.Trackings.Add(new ObtenerTrackingsSiabOutput.TrackingDto
                            {
                                Id = !reader.IsDBNull("idDemandaTracking") ? reader["idDemandaTracking"].To<int>() : (int?)null,
                                NumeroRegistro = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null,
                                FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : (DateTime?)null,
                                UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                                Movimiento = !reader.IsDBNull("movimiento") ? reader["movimiento"].As<string>() : null
                            });
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ObtenerComentariosSiabOutput> ObtenerComentariosSiab(ObtenerComentariosSiabInput input)
        {
            var result = new ObtenerComentariosSiabOutput();

            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabConsultaComentario", CommandType.StoredProcedure,
                    new SqlParameter("@numerSiab", input.NumeroRegistro));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    var mError = HasColumn(reader, "CodigoMmensaje");
                    while (await reader.ReadAsync())
                    {
                        if (mError)
                        {
                            string msj = reader["Mensaje"].As<string>();
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                        else
                        {
                            result.Comentarios.Add(new ObtenerComentariosSiabOutput.ComentarioDto
                            {
                                Id = !reader.IsDBNull("idDemandaComentario") ? reader["idDemandaComentario"].To<int>() : (int?)null,
                                NumeroRegistro = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null,
                                FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : (DateTime?)null,
                                UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                                Comentario = !reader.IsDBNull("comentario") ? reader["comentario"].As<string>() : null,
                            });
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ObtenerDocumentoSiabOutput> ObtenerDocumentoSiab(ObtenerDocumentoSiabInput input)
        {
            var result = new ObtenerDocumentoSiabOutput();

            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabOtenerDocumentos", CommandType.StoredProcedure,
                    new SqlParameter("@idDocumento", input.IdDocumento));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    var mError = HasColumn(reader, "CodigoMensaje");
                    while (await reader.ReadAsync())
                    {
                        if (mError)
                        {
                            string msj = reader["Mensaje"].As<string>();
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                        else
                        {
                            result.IdDocumento = !reader.IsDBNull(reader.GetOrdinal("idDocumento")) ? reader["idDocumento"].To<int>() : 0;
                            result.IdRegistro = !reader.IsDBNull(reader.GetOrdinal("idRegistro")) ? reader["idRegistro"].To<int>() : 0;
                            result.Nombre = !reader.IsDBNull(reader.GetOrdinal("nombre")) ? reader["nombre"].As<string>() : null;
                            result.ObjectId = !reader.IsDBNull(reader.GetOrdinal("objeto")) ? reader["objeto"].As<string>() : null;
                            result.FechaRegistro = !reader.IsDBNull(reader.GetOrdinal("fechaRegistro")) ? reader["fechaRegistro"].To<DateTime>() : DateTime.MinValue;
                            result.TipoMime = !reader.IsDBNull(reader.GetOrdinal("tipoMime")) ? reader["tipoMime"].As<string>() : null;
                            result.UsuarioRegistro = !reader.IsDBNull(reader.GetOrdinal("usuarioRegistro")) ? reader["usuarioRegistro"].As<string>() : null;
                            result.TipoDocumento = !reader.IsDBNull(reader.GetOrdinal("tipoDocumento")) ? reader["tipoDocumento"].As<string>() : null;
                            result.SubTipoDocumento = !reader.IsDBNull(reader.GetOrdinal("subTipoDocumento")) ? reader["subTipoDocumento"].As<string>() : null;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ValidacionSiabOutput> ValidacionSiab(ValidacionSiabInput input)
        {
            var result = new ValidacionSiabOutput();

            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spSiabValida", CommandType.StoredProcedure,
                    new SqlParameter("@fechaValidacion", DateTime.Now),
                    new SqlParameter("@usuario", input.Usuario));

                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string msj = reader["Mensaje"].As<string>();
                        if (reader["CodigoMensaje"].To<int>() == 0)
                        {
                            result.Mensaje = msj;
                            result.Validacion = reader["Validacion"].To<bool>();
                        }
                        else
                        {
                            reader.Close();
                            throw new UserFriendlyException(400, msj);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
