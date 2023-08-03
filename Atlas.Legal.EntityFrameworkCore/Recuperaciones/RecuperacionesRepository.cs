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
    public class RecuperacionesRepository : LegalRepositoryBase<Entity, int>, IRecuperacionesRepository
    {
        public RecuperacionesRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider, transactionProvider)
        {

        }

        public async Task<BuscarSiniestroRecuperacionesOutput> BusquedaRecuperacionSiniestroTransporte(BuscarSiniestroRecuperacionesInput input)
        {
            EnsureConnectionOpen();
            var result = new BuscarSiniestroRecuperacionesOutput();
            using (var mCommand = CreateCommand(
                    "spBusquedaRecuperacionSiniestroTransporte",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idTipoRecuperacion", (object) input.IdTipoRecuperacion ?? DBNull.Value),
                    new SqlParameter("@fechaRegistroInicial", (object) input.FechaRegistroInicial ?? DBNull.Value),
                    new SqlParameter("@fechaRegistroFinal", (object) input.FechaRegistroFinal ?? DBNull.Value),
                    new SqlParameter("@numeroSiniestro", (object) input.NumeroSiniestro ?? DBNull.Value)                    
                    )
                )
            {
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : (int?)null;
                            result.Mensaje = !mReader.IsDBNull("Mensaje") ? mReader["Mensaje"].As<string>() : null;
                            return result;
                        }
                        result.ConsultaSiniestro.Add(new BuscarSiniestroRecuperacionesOutput.DetalleModel
                        {
                            NumeroSiniestro = !mReader.IsDBNull("NumeroSiniestro") ? mReader["NumeroSiniestro"].As<string>() : null,
                            OrdenTrabajo = !mReader.IsDBNull("OrdenTrabajo") ? mReader["OrdenTrabajo"].As<string>() : null,
                            FechaOcurrido = !mReader.IsDBNull("FechaOcurrido") ? mReader["FechaOcurrido"].To<DateTime>() : (DateTime?)null,
                            Analista = !mReader.IsDBNull("Analista") ? mReader["Analista"].As<string>() : null,
                            Moneda = !mReader.IsDBNull("Moneda") ? mReader["Moneda"].As<string>() : null,
                            NombreAsegurado = !mReader.IsDBNull("NombreAsegurado") ? mReader["NombreAsegurado"].As<string>() : null,
                            Estimacion = !mReader.IsDBNull("Estimacion") ? mReader["Estimacion"].As<string>() : null,
                            Ajustador = !mReader.IsDBNull("Ajustador") ? mReader["Ajustador"].As<string>() : null,
                            Estado = !mReader.IsDBNull("Estado") ? mReader["Estado"].As<string>() : null,
                            Gastos = !mReader.IsDBNull("Gastos") ? mReader["Gastos"].As<string>() : null,
                            PagoSiniestro = !mReader.IsDBNull("PagoSiniestro") ? mReader["PagoSiniestro"].As<string>() : null,
                            FechaPagoSiniestro = !mReader.IsDBNull("FechaPagoSiniestro") ? mReader["FechaPagoSiniestro"].To<DateTime>() : (DateTime?)null,
                            AperturaRechazar = !mReader.IsDBNull("AperturaRechazar") ? mReader["AperturaRechazar"].To<bool>() : (bool?)null,
                            VerDetalleBase64 = !mReader.IsDBNull("VerDetalleBase64") ? mReader["VerDetalleBase64"].As<string>() : null,
                        });
                    }
                }
            }
            return result;
        }

        public async Task<RegistroDemandaRecuperacionesOutput> RegistroDemandaRecuperaciones(RegistroDemandaRecuperacionesInput input)
        {
            EnsureConnectionOpen();
            var result = new RegistroDemandaRecuperacionesOutput();
            using (var mCommand = CreateCommand(
                    "spGuardaActualizaRecuperacionDemanda",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idTipoRecuperacion", (object)input.IdTipoRecuperacion ?? DBNull.Value),
                    new SqlParameter("@Subrogado", (object)input.Subrogado ?? DBNull.Value),
                    new SqlParameter("@FechaRecepcionExpediente", (object)input.FechaRecepcionExpediente ?? DBNull.Value),
                    new SqlParameter("@Preinscripcion", (object)input.Preinscripcion ?? DBNull.Value),
                    new SqlParameter("@Antiguedad", (object)input.Antiguedad ?? DBNull.Value),
                    new SqlParameter("@Pendiente", (object)input.Pendiente ?? DBNull.Value),
                    new SqlParameter("@TiempoTurno", (object)input.TiempoTurno ?? DBNull.Value),
                    new SqlParameter("@Gastos", (object)input.Gastos ?? DBNull.Value),
                    new SqlParameter("@Recordatorio", (object)input.Recordatorio ?? DBNull.Value),
                    new SqlParameter("@IdCausa", (object)input.IdCausa ?? DBNull.Value),
                    new SqlParameter("@IdTerceroInvolucrado", (object)input.IdTerceroInvolucrado ?? DBNull.Value),
                    new SqlParameter("@IdEstatus", (object)input.IdEstatus ?? DBNull.Value),
                    new SqlParameter("@Observaciones", (object)input.Observaciones ?? DBNull.Value),
                    new SqlParameter("@EsDemanda", (object)input.EsDemanda ?? DBNull.Value),
                    new SqlParameter("@FechaPagoSiniestro", (object)input.FechaPagoSiniestro ?? DBNull.Value),
                    new SqlParameter("@PagoSiniestro", (object)input.PagoSiniestro ?? DBNull.Value),
                    new SqlParameter("@Actualizacion", (object)input.Actualizacion ?? DBNull.Value),
                    new SqlParameter("@Comentarios", (object)input.Comentarios ?? DBNull.Value),
                    new SqlParameter("@IdAbogado", (object)input.IdAbogado ?? DBNull.Value),
                    new SqlParameter("@NumDemanda", (object)input.NumDemanda ?? DBNull.Value),
                    new SqlParameter("@Fechacreacion", (object)input.FechaCreacion ?? DBNull.Value),
                    new SqlParameter("@Siniestro", (object)input.SiniestroSeleccionado.Siniestro ?? DBNull.Value),
                    new SqlParameter("@OrdenTrabajo", (object)input.SiniestroSeleccionado.OrdenTrabajo ?? DBNull.Value),
                    new SqlParameter("@NombreAsegurado", (object)input.SiniestroSeleccionado.NombreAsegurado ?? DBNull.Value),
                    new SqlParameter("@FechaOcurrido", (object)input.SiniestroSeleccionado.FechaOcurrido ?? DBNull.Value),
                    new SqlParameter("@Analista", (object)input.SiniestroSeleccionado.Analista ?? DBNull.Value),
                    new SqlParameter("@Moneda", (object)input.SiniestroSeleccionado.Moneda ?? DBNull.Value),
                    new SqlParameter("@IdEstado", (object)input.SiniestroSeleccionado.IdEstado ?? DBNull.Value),
                    new SqlParameter("@Recuperacion", (object)input.SiniestroSeleccionado.Recuperacion ?? DBNull.Value),
                    new SqlParameter("@Poliza", (object)input.SiniestroSeleccionado.Poliza ?? DBNull.Value),
                    new SqlParameter("@Vehiculo", (object)input.SiniestroSeleccionado.Vehiculo ?? DBNull.Value),
                    new SqlParameter("@IdCausab", (object)input.SiniestroSeleccionado.IdCausab ?? DBNull.Value),
                    new SqlParameter("@Tercero", (object)input.SiniestroSeleccionado.Tercero ?? DBNull.Value),
                    new SqlParameter("@FechaIngreso", (object)input.SiniestroSeleccionado.FechaIngreso ?? DBNull.Value),
                    new SqlParameter("@Honorarios", (object)input.SiniestroSeleccionado.Honorarios ?? DBNull.Value),
                    new SqlParameter("@IdRegExp", (object)input.IdRegExp ?? DBNull.Value),
                    new SqlParameter("@Subrrogacion", (object)input.Subrrogacion ?? DBNull.Value)
                    )
                )
            {
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : (int?)null;
                            result.Mensaje = !mReader.IsDBNull("Mensaje") ? mReader["Mensaje"].As<string>() : null;
                            result.Id = !mReader.IsDBNull("Id") ? mReader["Id"].To<int>() : (int?)null;
                            if (result.CodigoMensaje == 0 || result.CodigoMensaje == 1 || result.CodigoMensaje == null)
                            {
                                result.IdRegExp = !mReader.IsDBNull("IdRegExp") ? mReader["IdRegExp"].To<int>() : (int?)null;
                                result.NumeroDemanda = !mReader.IsDBNull("NumeroDemanda") ? mReader["NumeroDemanda"].As<string>() : null;
                            }
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public async Task<RegistroDemandaRecuperacionesDocumentoOutput> RegistroDemandaRecuperacionesDocumento(RegistroDemandaRecuperacionesDocumentoInput input)
        {
            EnsureConnectionOpen();
            var result = new RegistroDemandaRecuperacionesDocumentoOutput();
            using (var mCommand = CreateCommand(
                    "spAtlaActualizaDocumentosRecuperacion",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idDocumento", (object)input.IdDocumento ?? DBNull.Value),
                    new SqlParameter("@idDemanda", (object)input.IdDemanda ?? DBNull.Value),
                    new SqlParameter("@nombre", (object)input.Nombre ?? DBNull.Value),
                    new SqlParameter("@objeto", (object)input.Objeto ?? DBNull.Value),
                    new SqlParameter("@fechaRegistro", (object)input.FechaRegistro ?? DBNull.Value),
                    new SqlParameter("@tipoMime", (object)input.TipoMime ?? DBNull.Value),
                    new SqlParameter("@tipoDocumento", (object)input.TipoDocumento ?? DBNull.Value),
                    new SqlParameter("@usuarioRegistro", (object)input.UsuarioRegistro ?? DBNull.Value),
                    new SqlParameter("@subTipoDocumento", (object)input.SubTipoDocumento ?? DBNull.Value),
                    new SqlParameter("@borrar", (object)input.Borrar ?? DBNull.Value)
                    )
                )
            {
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : (int?)null;
                            result.Mensaje = !mReader.IsDBNull("Mensaje") ? mReader["Mensaje"].As<string>() : null;
                            result.IdDocumento = !mReader.IsDBNull("IdDocumento") ? mReader["IdDocumento"].To<int>() : (int?)null;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public async Task<AsignarAbogadoRecuperacionesOutput> AsignarAbogadoRecuperaciones(AsignarAbogadoRecuperacionesInput.AsignacionModel input)
        {
            EnsureConnectionOpen();
            var result = new AsignarAbogadoRecuperacionesOutput();
            using (var mCommand = CreateCommand(
                    "spGuardaActualizaRecuperacionAsigna",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idDemanda", (object)input.IdDemanda ?? DBNull.Value),
                    new SqlParameter("@numeroDemanda", (object)input.NumeroDemanda ?? DBNull.Value),
                    new SqlParameter("@tipoAsignacion", (object)input.TipoAsignacion ?? DBNull.Value),
                    new SqlParameter("@idProveedor", (object)input.IdProveedor ?? DBNull.Value),
                    new SqlParameter("@recuperacion", (object)input.Recuperacion ?? DBNull.Value),
                    new SqlParameter("@idturnadoDespacho", (object)input.IdTurnadoDespacho ?? DBNull.Value),
                    new SqlParameter("@fechaIngreso", (object)input.FechaIngreso ?? DBNull.Value),
                    new SqlParameter("@fechaCreacion", (object)input.FechaCreacion ?? DBNull.Value)
                    )
                )
            {
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : (int?)null;
                            result.Mensaje = !mReader.IsDBNull("Mensaje") ? mReader["Mensaje"].As<string>() : null;
                            result.Id = !mReader.IsDBNull("Id") ? mReader["Id"].To<int>() : (int?)null;
                            result.NumeroDemanda = !mReader.IsDBNull("NumeroDemanda") ? mReader["NumeroDemanda"].As<string>() : null;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public async Task<ConsultaRecuperacionOutput> ConsultaRecuperacion(ConsultaRecuperacionInput input)
        {
            EnsureConnectionOpen();
            var result = new ConsultaRecuperacionOutput();
            using (var mCommand = CreateCommand(
                    "spConsultaRecuperacion",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idTipoRecuperacion", (object)input.IdTipoRecuperacion ?? DBNull.Value),
                    new SqlParameter("@fechaRegistroInicial", (object)input.FechaRegistroInicial ?? DBNull.Value),
                    new SqlParameter("@fechaRegistroFinal", (object)input.FechaRegistroFinal ?? DBNull.Value),
                    new SqlParameter("@numeroSiniestro", (object)input.NumeroSiniestro ?? DBNull.Value),
                    new SqlParameter("@idProveedor", (object)input.IdProveedor ?? DBNull.Value),
                    new SqlParameter("@asegurado", (object)input.Asegurado ?? DBNull.Value)
                    )
                )
            {
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : (int?)null;
                            result.Mensaje = !mReader.IsDBNull("Mensaje") ? mReader["Mensaje"].As<string>() : null;
                            return result;
                        }
                        result.Detalles.Add(new ConsultaRecuperacionOutput.DetalleModel
                        {
                            IdDemanda = !mReader.IsDBNull("IdDemanda") ? mReader["IdDemanda"].To<int>() : (int?)null,
                            NumeroSiniestro = !mReader.IsDBNull("NumeroSiniestro") ? mReader["NumeroSiniestro"].As<string>() : null,
                            Asegurado = !mReader.IsDBNull("Asegurado") ? mReader["Asegurado"].As<string>() : null,
                            FechaOcurrido = !mReader.IsDBNull("FechaOcurrido") ? mReader["FechaOcurrido"].To<DateTime>() : (DateTime?)null,
                            Preinscripcion = !mReader.IsDBNull("Preinscripcion") ? mReader["Preinscripcion"].To<int>() : (int?)null,
                            Abogado = !mReader.IsDBNull("Abogado") ? mReader["Abogado"].As<string>() : null,
                            Recuperacion = !mReader.IsDBNull("Recuperacion") ? mReader["Recuperacion"].As<string>() : null,
                            FechaRecuperacion = !mReader.IsDBNull("FechaRecuperacion") ? mReader["FechaRecuperacion"].To<DateTime>() : (DateTime?)null,
                            TipoDemanda = !mReader.IsDBNull("TipoDemanda") ? mReader["TipoDemanda"].To<bool>() : (bool?)null,
                            NumDemanda = !mReader.IsDBNull("NumeroDemanda") ? mReader["NumeroDemanda"].As<string>() : null,
                        });
                    }
                }
            }
            return result;
        }

        public async Task<DetalleRecuperacionOutput> DetalleRecuperacion(DetalleRecuperacionInput input)
        {
            EnsureConnectionOpen();
            var result = new DetalleRecuperacionOutput();
            using (var mCommand = CreateCommand(
                    "spDetalleRecuperacion",
                    CommandType.StoredProcedure,
                    new SqlParameter("@numeroDemanda", (object)input.IdDemanda ?? DBNull.Value)
                    )
                )
            {
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : (int?)null;
                            result.Mensaje = !mReader.IsDBNull("Mesaje") ? mReader["Mesaje"].As<string>() : null;
                            return result;
                        }
                        result.Detalles.Add(new DetalleRecuperacionOutput.DetalleModel
                        {
                            IdDemanda = !mReader.IsDBNull("idReg") ? mReader["idReg"].To<int>() : (int?)null,
                            TipoRecuperacion = !mReader.IsDBNull("TipoRecuperacion") ? mReader["TipoRecuperacion"].As<string>() : null,
                            SiniestroSeleccionado = !mReader.IsDBNull("SiniestroSeleccionado") ? JsonConvert.DeserializeObject<Object>(mReader["SiniestroSeleccionado"].As<string>()) : null,
                            Subrogado = !mReader.IsDBNull("Subrogado") ? mReader["Subrogado"].As<string>() : null,
                            FechaRecepcionExpediente = !mReader.IsDBNull("FechaRecepcionExpediente") ? mReader["FechaRecepcionExpediente"].To<DateTime>() : (DateTime?)null,
                            Preinscripcion = !mReader.IsDBNull("Preinscripcion") ? mReader["Preinscripcion"].To<int>() : (int?)null,
                            Antiguedad = !mReader.IsDBNull("Antiguedad") ? mReader["Antiguedad"].As<string>() : null,
                            Pendiente = !mReader.IsDBNull("Pendiente") ? mReader["Pendiente"].As<string>() : null,
                            TiempoTurno = !mReader.IsDBNull("TiempoTurno") ? mReader["TiempoTurno"].To<int>() : (int?)null,
                            Gastos = !mReader.IsDBNull("Gastos") ? mReader["Gastos"].As<string>() : null,
                            Recordatorio = !mReader.IsDBNull("Recordatorio") ? mReader["Recordatorio"].As<string>() : null,
                            Causa = !mReader.IsDBNull("Causa") ? mReader["Causa"].As<string>() : null,
                            Estatus = !mReader.IsDBNull("Estatus") ? mReader["Estatus"].As<string>() : null,
                            EsDemanda = !mReader.IsDBNull("EsDemanda") ? mReader["EsDemanda"].To<int>() : (int?)null,
                            FechaPagoSiniestro = !mReader.IsDBNull("FechaPagoSiniestro") ? mReader["FechaPagoSiniestro"].To<DateTime>() : (DateTime?)null,
                            PagoSiniestro = !mReader.IsDBNull("PagoSiniestro") ? mReader["PagoSiniestro"].As<string>() : null,
                            Actualizacion = !mReader.IsDBNull("Actualizacion") ? mReader["Actualizacion"].As<string>() : null,
                            Observaciones = !mReader.IsDBNull("Observaciones") ? mReader["Observaciones"].As<string>() : null,
                            Abogado = !mReader.IsDBNull("Abogado") ? mReader["Abogado"].As<string>() : null,
                            Comentarios = !mReader.IsDBNull("Comentarios") ? mReader["Comentarios"].As<string>() : null,
                            Documentos = !mReader.IsDBNull("Documentos") ? JsonConvert.DeserializeObject<Object>(mReader["Documentos"].As<string>()) : null,
                            Asignacion = !mReader.IsDBNull("Asignacion") ? JsonConvert.DeserializeObject<Object>(mReader["Asignacion"].As<string>()) : null,
                            EsConcluido = !mReader.IsDBNull("EsConcluido") ? mReader["EsConcluido"].To<bool>() : (bool?)null
                        });
                    }
                }
            }
            return result;
        }

        public async Task<ConcluirRecuperacionOutput> ConcluirRecuperacion(ConcluirRecuperacionInput input)
        {
            EnsureConnectionOpen();
            var result = new ConcluirRecuperacionOutput();
            using (var mCommand = CreateCommand(
                    "spConcluirRecuperacion",
                    CommandType.StoredProcedure,
                    new SqlParameter("@numeroDdemanda", (object)input.IdDemanda ?? DBNull.Value),
                    new SqlParameter("@Concluir", (object)input.Concluir ?? DBNull.Value)
                    )
                )
            {
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.IdDemanda = !mReader.IsDBNull("NumeroDemanda") ? mReader["NumeroDemanda"].As<string>() : null;
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : int.MinValue;
                            result.Mensaje = !mReader.IsDBNull("Mensaje") ? mReader["Mensaje"].As<string>() : null;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public async Task<AperturarRechazarSiniestroRecuperacionesOut> AperturarRechazarSiniestroRecuperacion(AperturarRechazarSiniestroRecuperacionesInput input)
        {
            EnsureConnectionOpen();
            var result = new AperturarRechazarSiniestroRecuperacionesOut();
            using (var mCommand = CreateCommand(
                    "spAperuraRechazarSiniestroRecuperacion",
                    CommandType.StoredProcedure,
                    new SqlParameter("@numeroSiniestro", (object)input.NumeroSiniestro ?? DBNull.Value),
                    new SqlParameter("@aperturaRecharzar", (object)input.AperturarRechazar ?? DBNull.Value)
                    )
                )
            {
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.Siniestro = !mReader.IsDBNull("Siniestro") ? mReader["Siniestro"].As<string>() : null;
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : int.MinValue;
                            result.Mensaje = !mReader.IsDBNull("Mensaje") ? mReader["Mensaje"].As<string>() : null;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public async Task<ObtenerReporteOut> ObtenerReporte<T>(string storedProcedureName, T input)
        {
            EnsureConnectionOpen();
            var result = new ObtenerReporteOut();
            using (var mCommand = CreateCommand(
                    storedProcedureName,
                    CommandType.StoredProcedure
                    )
                )
            {
                foreach (var item in input.GetType().GetProperties())
                    mCommand.Parameters.Add(new SqlParameter($"@{item.Name}", (object)item.GetValue(input) ?? DBNull.Value));
                using (var mReader = await mCommand.ExecuteReaderAsync())
                {
                    var mExisteColumnaError = HasColumn(mReader, "CodigoMensaje");
                    while (await mReader.ReadAsync())
                    {
                        if (mExisteColumnaError)
                        {
                            result.CodigoMensaje = !mReader.IsDBNull("CodigoMensaje") ? mReader["CodigoMensaje"].To<int>() : (int?)null;
                            result.Mensaje = !mReader.IsDBNull("Mensaje") ? mReader["Mensaje"].As<string>() : null;
                            return result;
                        }
                        var mRow = new Dictionary<string, object>();
                        for (int i = 0; i < mReader.FieldCount; i++)
                        {
                            var mLlave = Char.ToLowerInvariant(mReader.GetName(i)[0]) + mReader.GetName(i).Substring(1);
                            var mValor = mReader[i];
                            mRow.Add(mLlave, mValor);
                        }
                        result.Reporte.Add(mRow);
                    }
                }
            }
            return result;
        } 
        
        public string ObtenerObjectIdRecuperaciones(ObtenerObjectIdRecuperacionesInput input)
        {
            string texto = $"SELECT objeto FROM OpeRecuperacionDocumentos WHERE idDocumento = {input.IdDocumento}";
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

        public ObtenerIdRegExpRecuperacionesOutput ObtenerIdRegExp(int idDemanda)
        {
            var output = new ObtenerIdRegExpRecuperacionesOutput();
            string texto = $"SELECT idRegExp, numeroDemanda FROM OpeRecuperacionDemanda WHERE idRegistro = {idDemanda}";

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);
            using(var reader = mCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    output.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                    output.NumDemanda = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null;
                }
            }
            return output;
        }

        public async Task<ObtenerDocumentoRecuperacionesOutput> ObtenerDocumentoRecuperaciones(ObtenerDocumentoRecuperacionesInput input)
        {
            var result = new ObtenerDocumentoRecuperacionesOutput();
            EnsureConnectionOpen();
            using (var mCommand = CreateCommand("spConsultaDocumentoRecuperacion", CommandType.StoredProcedure, new SqlParameter("@idDocumento", input.IdDocumento)))
            {
                var reader = mCommand.ExecuteReader();
                while (reader.Read())
                {
                    result.IdDocumento = !reader.IsDBNull(reader.GetOrdinal("idDocumento")) ? reader["idDocumento"].To<int>() : 0;
                    result.IdDemanda = !reader.IsDBNull(reader.GetOrdinal("idDemanda")) ? reader["idDemanda"].To<int>() : 0;
                    result.Nombre = !reader.IsDBNull(reader.GetOrdinal("nombre")) ? reader["nombre"].As<string>() : null;
                    result.ObjectId = !reader.IsDBNull(reader.GetOrdinal("objeto")) ? reader["objeto"].As<string>() : null;
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

        public void GuardaTrackingRecuperaciones(GuardaTrackingRecuperacionesInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroRecuperacionDemandaTracking", CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumDemanda),
                new SqlParameter("@fechaRegistro", input.FechaRegistro),
                new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                new SqlParameter("@movimiento", input.Movimiento));

            var reader = mCommand.ExecuteReader();
            var mExisteColumnaError = HasColumn(reader, "CodigoMensaje");

            while (reader.Read())
            {
                if (mExisteColumnaError)
                {
                    var msj = reader["Mensaje"].As<string>();
                    if (reader["CodigoMmensaje"].To<int>() != 0)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }
            }
            reader.Close();
        }

        public async Task<ObtenerTrackingsRecuperacionesOutput> ObtenerTrackingsRecuperaciones(ObtenerTrackingsRecuperacionesInput input)
        {
            var result = new ObtenerTrackingsRecuperacionesOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaRecuperacionDemandaTracking",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumDemanda));

            var reader = await mCommand.ExecuteReaderAsync();
            var mError = HasColumn(reader, "CodigoMensaje");
            while (reader.Read())
            {                
                if (mError)
                {
                    var msj = reader["Mensaje"].As<String>();
                    reader.Close();
                    throw new UserFriendlyException(400, msj);
                }
                result.Trackings.Add(new ObtenerTrackingsRecuperacionesOutput.TrackingDto
                {
                    IdDemandaTracking = !reader.IsDBNull("idDemandaTracking") ? reader["idDemandaTracking"].To<int>() : 0,
                    NumDemanda = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null,
                    FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : DateTime.MinValue,
                    UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                    Movimiento = !reader.IsDBNull("movimiento") ? reader["movimiento"].As<string>() : null,
                });
            }
            reader.Close();
            return result;
        }

        public void GuardarComentarioRecuperaciones(GuardarComentarioRecuperacionesInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroRecuperacionDemandaComentario",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumDemanda),
                new SqlParameter("@fechaRegistro", input.FechaRegistro),
                new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                new SqlParameter("@comentario", input.Comentario));

            var reader = mCommand.ExecuteReader();
            var mError = HasColumn(reader, "CodigoMmensaje");
            while (reader.Read())
            {
                if (mError)
                {
                    var msj = reader["Mensaje"].As<string>();
                    if (reader["CodigoMmensaje"].To<int>() != 0)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, msj);
                    }
                }
            }
            reader.Close();
        }

        public async Task<ObtenerComentariosRecuperacionesOutput> ObtenerComentariosRecuperaciones(ObtenerComentariosRecuperacionesInput input)
        {
            var result = new ObtenerComentariosRecuperacionesOutput();
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaRecuperacionDemandaComentario",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumDemanda));

            var reader = await mCommand.ExecuteReaderAsync();
            var mError = HasColumn(reader, "CodigoMmensaje");
            while (reader.Read())
            {
                if (mError)
                {
                    var msj = reader["Mensaje"].As<String>();
                    reader.Close();
                    throw new UserFriendlyException(400, msj);
                }
                result.Comentarios.Add(new ObtenerComentariosRecuperacionesOutput.ComentarioDto
                {
                    IdDemandaComentario = !reader.IsDBNull("idDemandaComentario") ? reader["idDemandaComentario"].To<int>() : 0,
                    NumDemanda = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null,
                    FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : DateTime.MinValue,
                    UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                    Comentario = !reader.IsDBNull("comentario") ? reader["comentario"].As<string>() : null,
                });
            }
            reader.Close();

            return result;


        }
    }
}

