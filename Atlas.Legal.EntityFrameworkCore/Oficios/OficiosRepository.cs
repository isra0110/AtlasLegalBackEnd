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
    public class OficiosRepository : LegalRepositoryBase<Entity, int>, IOficiosRepository
    {
        public OficiosRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider, transactionProvider)
        {

        }

        public async Task<RegistraOficioOutput> RegistraOficio(RegistraOficioInput input)
        {
            EnsureConnectionOpen();
            var result = new RegistraOficioOutput();
            using (var mCommand = CreateCommand(
                    "spAltaModificaOficios",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idTipoOficio", (object)input.IdTipoOficio ?? DBNull.Value),
                    new SqlParameter("@fecgaEmision", (object)input.FechaEmisionOficio?? DBNull.Value),
                    new SqlParameter("@dependenciaQuenvia", (object)input.DependenciaQueEnvia ?? DBNull.Value),
                    new SqlParameter("@asunt", (object)input.Asunto ?? DBNull.Value),
                    new SqlParameter("@expediente", (object)input.Expediente ?? DBNull.Value),
                    new SqlParameter("@oficio", (object)input.Oficio ?? DBNull.Value),
                    new SqlParameter("@fechaNotificacionSeguroAtlas", (object)input.FechaNotificacionSegurosAtlas ?? DBNull.Value),
                    new SqlParameter("@hojasOficio", (object)input.HojasOficio ?? DBNull.Value),
                    new SqlParameter("@idAreaInvolucrada", (object)input.IdAreaInvolucrada ?? DBNull.Value),
                    new SqlParameter("@numeroOficio", (object)input.NumeroOficio ?? DBNull.Value),
                    new SqlParameter("@idRegExp", (object)input.IdRegExp ?? DBNull.Value)
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
                            result.IdOficio = !mReader.IsDBNull("IdOficio") ? mReader["IdOficio"].To<int>() : (int?)null;
                            result.NumeroOficio = !mReader.IsDBNull("NumeroOfiocio") ? mReader["NumeroOfiocio"].As<string>() : null;
                            result.IdRegExp = !mReader.IsDBNull("IdRegExp") ? mReader["IdRegExp"].To<int>() : (int?)null;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public async Task<RegistraOficioDocumentoOutput> RegistraOficioDocumento(RegistraOficioDocumentoInput input)
        {
            EnsureConnectionOpen();
            var result = new RegistraOficioDocumentoOutput();
            using (var mCommand = CreateCommand(
                    "spAltaModificaOficiosDocumentos",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idDocumentos", (object)input.IdDocumento ?? DBNull.Value),
                    new SqlParameter("@idOficio", (object)input.IdOficio ?? DBNull.Value),
                    new SqlParameter("@nombre", (object)input.Nombre ?? DBNull.Value),
                    new SqlParameter("@objeto", (object)input.Objeto ?? DBNull.Value),
                    new SqlParameter("@fechaRegistro", (object)input.FechaRegistro ?? DBNull.Value),
                    new SqlParameter("@usuarioRegistro", (object)input.UsuarioRegistro ?? DBNull.Value),
                    new SqlParameter("@subTipoDocumento", (object)input.SubTipoDocumento ?? DBNull.Value),
                    new SqlParameter("@tipoMime", (object)input.TipoMime ?? DBNull.Value),
                    new SqlParameter("@tipoDocumento", (object)input.TipoDocumento ?? DBNull.Value),
                    new SqlParameter("@borrar", input.Borrar)
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
                            result.IdOficios = !mReader.IsDBNull("IdOficios") ? mReader["IdOficios"].To<int>() : (int?)null;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public async Task<AsignarAbogadoOficiosOutput> AsignarAbogadoOficios(AsignarAbogadoOficiosInput.AsignacionModel input)
        {
            EnsureConnectionOpen();
            var result = new AsignarAbogadoOficiosOutput();
            using (var mCommand = CreateCommand(
                    "spAltaModificaOficiosAsigna",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idDemanda", (object)input.IdDemanda ?? DBNull.Value),
                    new SqlParameter("@numeroOficio", (object)input.NumeroOficio ?? DBNull.Value),
                    new SqlParameter("@esInterno", (object)input.EsInterno ?? DBNull.Value),
                    new SqlParameter("@idProveedor", (object)input.IdProveedor ?? DBNull.Value),
                    new SqlParameter("@idAbogadoInterno", (object)input.IdAbogadoInterno ?? DBNull.Value),
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
                            result.IdDemanda = !mReader.IsDBNull("IdDemanda") ? mReader["IdDemanda"].To<int>() : (int?)null;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public async Task<ConsultaOficioOutput> ConsultaOficio(ConsultaOficioInput input)
        {
            EnsureConnectionOpen();
            var result = new ConsultaOficioOutput();
            using (var mCommand = CreateCommand(
                    "spConsultaOficios",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idTipoOficio", input.IdTipoOficio),
                    new SqlParameter("@idAbogadoInterno", input.IdAbogadoInterno),
                    new SqlParameter("@idProveedor", input.IdProveedor),
                    new SqlParameter("@fechaRegistroInicial", input.FechaRegistroInicial),
                    new SqlParameter("@fechaRegistroFinal", input.FechaRegistroFinal)
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
                        result.Detalles.Add(new ConsultaOficioOutput.DetalleModel
                        {
                            IdOficio = !mReader.IsDBNull("idOficios") ? mReader["idOficios"].To<int>() : (int?)null,
                            NumOficio = !mReader.IsDBNull("numeroOficio") ? mReader["numeroOficio"].As<string>() : null,
                            IdTipoOficio = !mReader.IsDBNull("IdTipoOficio") ? mReader["IdTipoOficio"].To<int>() : (int?)null,
                            FechaEmision = !mReader.IsDBNull("FechaEmision") ? mReader["FechaEmision"].To<DateTime>() : (DateTime?)null,
                            DependenciaQueEnvia = !mReader.IsDBNull("DependenciaQueEnvia") ? mReader["DependenciaQueEnvia"].As<string>() : null,
                            Asunto = !mReader.IsDBNull("Asunto") ? mReader["Asunto"].As<string>() : null,
                            Expediente = !mReader.IsDBNull("Expediente") ? mReader["Expediente"].As<string>() : null,
                            Oficio = !mReader.IsDBNull("Oficio") ? mReader["Oficio"].As<string>() : null,
                            FechaNotificacionSeguroAtlas = !mReader.IsDBNull("FechaNotificacionSeguroAtlas") ? mReader["FechaNotificacionSeguroAtlas"].To<DateTime>() : (DateTime?)null,
                            HojasOficio = !mReader.IsDBNull("HojasOficio") ? mReader["HojasOficio"].To<int>() : (int?)null,
                            IdAreaInvolucrada = !mReader.IsDBNull("IdAreaInvolucrada") ? mReader["IdAreaInvolucrada"].To<int>() : (int?)null,
                            Asignacion = !mReader.IsDBNull("Asignacion") ? JsonConvert.DeserializeObject<Object>(mReader["Asignacion"].As<string>()) : null
                        });
                    }
                }
            }
            return result;
        }

        public async Task<DetalleOficioOutput> DetalleOficio(DetalleOficioInput input)
        {
            EnsureConnectionOpen();
            var result = new DetalleOficioOutput();
            using (var mCommand = CreateCommand(
                    "spConsultaDetalleOficios",
                    CommandType.StoredProcedure,
                    new SqlParameter("@numeroOficio", (object)input.NumeroOficio ?? DBNull.Value)
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
                            result.NumeroOficio = !mReader.IsDBNull("NumeroOficio") ? mReader["NumeroOficio"].As<string>() : null;
                            return result;
                        }
                        result.VerDetalle = !mReader.IsDBNull("VerDetalle") ? JsonConvert.DeserializeObject<Object>(mReader["VerDetalle"].As<string>()) : null;
                        result.Asignacion = !mReader.IsDBNull("Asignacion") ? JsonConvert.DeserializeObject<Object>(mReader["Asignacion"].As<string>()) : null;
                    }
                }
            }
            return result;
        }

        public async Task<ConcluirOficioOutput> ConcluirOficio(ConcluirOficioInput input)
        {
            EnsureConnectionOpen();
            var result = new ConcluirOficioOutput();
            using (var mCommand = CreateCommand(
                    "spConcluirOficios",
                    CommandType.StoredProcedure,
                    new SqlParameter("@numeroOficio", (object)input.NumeroOficio ?? DBNull.Value),
                    new SqlParameter("@concluir", (object)input.Concluir ?? DBNull.Value),
                    new SqlParameter("@pendiente", (object)input.Pendiente ?? DBNull.Value)
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
                            result.NumeroOficio = !mReader.IsDBNull("NumeroOficio") ? mReader["NumeroOficio"].As<string>() : null;
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

        public void GuardaTrackingOficio(GuardaTrackingOficioInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistrooficiosDemandaTracking", CommandType.StoredProcedure,
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

        public async Task<ObtenerTrackingsOficioOutput> ObtenerTrackingsOficio(ObtenerTrackingsOficioInput input)
        {
            var result = new ObtenerTrackingsOficioOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaOficioDemandaTracking",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumOficio));

            var reader = mCommand.ExecuteReader();
            var mError = HasColumn(reader, "CodigoMmensaje");
            while (reader.Read())
            {
                if (mError)
                {
                    var msj = reader["Mensaje"].As<String>();
                    reader.Close();
                    throw new UserFriendlyException(400, msj);
                }
                result.Trackings.Add(new ObtenerTrackingsOficioOutput.TrackingDto
                {
                    IdDemandaTracking = !reader.IsDBNull("idDemandaTracking") ? reader["idDemandaTracking"].To<int>() : 0,
                    NumOficio = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null,
                    FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : DateTime.MinValue,
                    UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                    Movimiento = !reader.IsDBNull("movimiento") ? reader["movimiento"].As<string>() : null,
                });
            }
            reader.Close();
            return result;
        }

        public void GuardaComentarioOficios(GuardaComentarioOficiosInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroOficioDemandaComentario",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumOficio),
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

        public async Task<ObtenerComentariosOficiosOutput> ObtenerComentariosOficios(ObtenerComentariosOficiosInput input)
        {
            var result = new ObtenerComentariosOficiosOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaOficioDemandaComentario",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumOficio));

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
                else
                {
                    result.Comentarios.Add(new ObtenerComentariosOficiosOutput.ComentarioDto
                    {
                        IdDemandaComentario = !reader.IsDBNull("idDemandaComentario") ? reader["idDemandaComentario"].To<int>() : 0,
                        NumOficio = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null,
                        FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : DateTime.MinValue,
                        UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                        Comentario = !reader.IsDBNull("comentario") ? reader["comentario"].As<string>() : null,
                    });
                }
                    
            }
            reader.Close();

            return result;
        }

        public string ObtenerObjectIdOficios(ObtenerObjectIdOficiosInput input)
        {
            string texto = $"SELECT objeto FROM OpeOficiosDocumentos WHERE idDocumento = {input.IdDocumento}";
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

        public ObtenerIdRegExpOficiosOutput ObtenerIdRegExp(int idOficio)
        {
            var output = new ObtenerIdRegExpOficiosOutput();
            string texto = $"SELECT idRegExp, numeroOficio FROM OpeOficios WHERE idOficios = {idOficio}";

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);
            using (var reader = mCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    output.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                    output.NumOficio = !reader.IsDBNull("numeroOficio") ? reader["numeroOficio"].As<string>() : null;
                }
            }
            return output;
        }

        public async Task<ObtenerDocumentoOficiosOutput> ObtenerDocumentoOficios(ObtenerDocumentoOficiosInput input)
        {
            var result = new ObtenerDocumentoOficiosOutput();
            EnsureConnectionOpen();
            using (var mCommand = CreateCommand("spConsultaDocumentoOficios", CommandType.StoredProcedure, new SqlParameter("@idDocumento", input.IdDocumento)))
            {
                var reader = mCommand.ExecuteReader();
                while (reader.Read())
                {
                    result.IdDocumento = !reader.IsDBNull(reader.GetOrdinal("idDocumento")) ? reader["idDocumento"].To<int>() : 0;
                    result.IdOficio = !reader.IsDBNull(reader.GetOrdinal("idOficio")) ? reader["idOficio"].To<int>() : 0;
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
    }
}

