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
    public class ConducefRepository : LegalRepositoryBase<Entity, int>, IConducefRepository
    {
        public ConducefRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base (dbContextProvider, transactionProvider)
        {

        }

        public async Task<ConsultaConducefOutput> ConsultaConducef(ConsultaConducefInput input)
        {
            var result = new ConsultaConducefOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConducefConsulta", CommandType.StoredProcedure,
                new SqlParameter("@idExpedienteProvieneDe", input.IdExpedienteProvieneDe),
                new SqlParameter("@idDependencia", input.IdDependencia),
                new SqlParameter("@idNaturalezaActor", input.IdNaturalezaActor),
                new SqlParameter("@idArea", input.IdArea),
                new SqlParameter("@idRamoBusqueda", input.IdRamoBusqueda),
                new SqlParameter("@idDepartamentoUno", input.IdDepartamento1),
                new SqlParameter("@idDepartamentoDos", input.IdDepartamento2),
                new SqlParameter("@solantec", input.Solantec),
                new SqlParameter("@fechaTermino", input.FechaTermino),
                new SqlParameter("@fechaRecepcion", input.FechaRecepcion));

            var reader = await mCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Conducef.Add(new ConsultaConducefOutput.ConducefDto
                {
                    NumeroConducef = !reader.IsDBNull("numeroConducef") ? reader["numeroConducef"].As<string>() : null,
                    NumeroExpediente = !reader.IsDBNull("numeroExpediente") ? reader["numeroExpediente"].As<string>() : null,
                    FechaTermino = !reader.IsDBNull("fechaTermino") ? reader["fechaTermino"].To<DateTime>() : (DateTime?)null,
                    Dependencia = !reader.IsDBNull("dependencia") ? reader["dependencia"].As<string>() : null,
                    Partes = !reader.IsDBNull("partes") ? reader["partes"].As<string>() : null,
                    NaturalezaActor = !reader.IsDBNull("naturalezaActor") ? reader["naturalezaActor"].As<string>() : null,
                    Poliza = !reader.IsDBNull("poliza") ? reader["poliza"].As<string>() : null,
                    Siniestro = !reader.IsDBNull("siniestro") ? reader["siniestro"].As<string>() : null,
                    RamoBusqueda = !reader.IsDBNull("ramoBusqueda") ? reader["ramoBusqueda"].As<string>() : null,
                    Area = !reader.IsDBNull("area") ? reader["area"].As<string>() : null,
                    MontoReclamado = !reader.IsDBNull("montoReclamado") ? reader["montoReclamado"].As<string>() : null,
                    Comentario = !reader.IsDBNull("comentario") ? reader["comentario"].As<string>() : null,
                    Caracteristicas = !reader.IsDBNull("catacteristicas") ? reader["catacteristicas"].As<string>() : null,
                    Particularidad = !reader.IsDBNull("particularidad") ? reader["particularidad"].As<string>() : null,
                    ProductoServicio = !reader.IsDBNull("ProductoServicio") ? reader["ProductoServicio"].As<string>() : null,
                    Solantec = !reader.IsDBNull("solantec") ? reader["solantec"].To<DateTime>() : (DateTime?)null,
                    Reversagie = !reader.IsDBNull("reversagie") ? reader["reversagie"].To<bool>() : (bool?)null,
                    Pori = !reader.IsDBNull("pori") ? reader["pori"].To<bool>() : (bool?)null,
                    FechaRespuestaArea = !reader.IsDBNull("fechaRespuestaArea") ? reader["fechaRespuestaArea"].To<DateTime>() : (DateTime?)null,
                    FechaResolucionConclusion = !reader.IsDBNull("fechaResolucionConclusion") ? reader["fechaResolucionConclusion"].To<DateTime>() : (DateTime?)null,
                    EstadoConcluidoPendiente = !reader.IsDBNull("EstadoConcLuidoPendiente") ? reader["EstadoConcLuidoPendiente"].As<string>() : null,
                    FechaNotificacionUsuario = !reader.IsDBNull("fechaNotificacionAlUsuario") ? reader["fechaNotificacionAlUsuario"].To<DateTime>() : (DateTime?)null,
                    ImporteQueSeRestituyoAlUsuario = !reader.IsDBNull("importeQueSeRestituyoAlUsuario") ? reader["importeQueSeRestituyoAlUsuario"].As<string>() : null,
                    ClaveEntidadFederativa = !reader.IsDBNull("claveEntidadFederativa") ? reader["claveEntidadFederativa"].To<int>() : (int?)null
                });
            }
            reader.Close();
            return result;           
        }

        public async Task<DetalleConducefOutput> DetalleConducef(DetalleConducefInput input)
        {
            var result = new DetalleConducefOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConducefDetalle", CommandType.StoredProcedure,
                new SqlParameter("@numeroConducef", input.NumeroConducef));

            var reader = await mCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.IdConducef = !reader.IsDBNull("idConducef") ? reader["idConducef"].To<int>() : (int?)null;
                result.IdExpedienteProvieneDe = !reader.IsDBNull("idExpedienteProvieneDe") ? reader["idExpedienteProvieneDe"].To<int>() : (int?)null;
                result.NumeroExpediente = !reader.IsDBNull("numeroExpediente") ? reader["numeroExpediente"].As<string>() : null;
                result.FechaRecepcion = !reader.IsDBNull("fechaRecepcion") ? reader["fechaRecepcion"].To<DateTime>() : (DateTime?)null;
                result.TrimestreInforma = !reader.IsDBNull("trimestreInforma") ? reader["trimestreInforma"].To<int>() : (int?)null;
                result.FechaTermino = !reader.IsDBNull("fechaTermino") ? reader["fechaTermino"].To<DateTime>() : (DateTime?)null;
                result.CuentaConSiniestro = !reader.IsDBNull("cuenntaConSiniestro") ? reader["cuenntaConSiniestro"].To<bool>() : (bool?)null;
                result.Siniestro = !reader.IsDBNull("siniestro") ? JsonConvert.DeserializeObject<Object>(reader["siniestro"].As<string>()) : null;
                result.IdDependencia = !reader.IsDBNull("idDependencia") ? reader["idDependencia"].To<int>() : (int?)null;
                result.CP = !reader.IsDBNull("cp") ? reader["cp"].As<string>() : null;
                result.IdEntidad = !reader.IsDBNull("idEntidad") ? reader["idEntidad"].To<int>() : (int?)null;
                result.ClaveEntidadFederativa = !reader.IsDBNull("claveEntidadFederativa") ? reader["claveEntidadFederativa"].To<int>() : (int?)null;
                result.Partes = !reader.IsDBNull("partes") ? reader["partes"].As<string>() : null;
                result.IdNaturalezaActor = !reader.IsDBNull("idNaturalezaActor") ? reader["idNaturalezaActor"].To<int>() : (int?)null;
                result.Ramo = !reader.IsDBNull("ramo") ? reader["ramo"].As<string>() : null;
                result.IdArea = !reader.IsDBNull("idArea") ? reader["idArea"].To<int>() : (int?)null;
                result.Caracteristicas = !reader.IsDBNull("catacteristicas") ? reader["catacteristicas"].As<string>() : null;
                result.IdDepartamento1 = !reader.IsDBNull("idDepartamento1") ? reader["idDepartamento1"].To<int>() : (int?)null;
                result.IdDepartamento2 = !reader.IsDBNull("idDepartamento2") ? reader["idDepartamento2"].To<int>() : (int?)null;
                result.Particularidad = !reader.IsDBNull("particularidad") ? reader["particularidad"].As<string>() : null;
                result.MontoReclamado = !reader.IsDBNull("montoReclamado") ? reader["montoReclamado"].As<string>() : null;
                result.SinCuantificar = !reader.IsDBNull("sinCuantificar") ? reader["sinCuantificar"].To<bool>() : (bool?)null;
                result.IdDescripcionDelMedioRecepcion = !reader.IsDBNull("idDescripcionDelMedioRecepcion") ? reader["idDescripcionDelMedioRecepcion"].To<int>() : (int?)null;
                result.ClaveMedioRecepcion = !reader.IsDBNull("claveMedioRecepcion") ? reader["claveMedioRecepcion"].To<int>() : (int?)null;
                result.Solantec = !reader.IsDBNull("solantec") ? reader["solantec"].To<DateTime>() : (DateTime?)null;
                result.FechaRespuestaArea = !reader.IsDBNull("fechaRespuestaArea") ? reader["fechaRespuestaArea"].To<DateTime>() : (DateTime?)null;
                result.FechaResolucionConclusion = !reader.IsDBNull("fechaResolucionConclusion") ? reader["fechaResolucionConclusion"].To<DateTime>() : (DateTime?)null;
                result.IdEstadoConcluidoPendiente = !reader.IsDBNull("idEstadoConcluidoPendiente") ? reader["idEstadoConcluidoPendiente"].To<int>() : (int?)null;
                result.DescripcionEstadoConcluidoPendiente = !reader.IsDBNull("desctripcionEstadoConcluidoPendiente") ? reader["desctripcionEstadoConcluidoPendiente"].As<string>() : null;
                result.FechaNotificacionAlUsuario = !reader.IsDBNull("fechaNotificacionAlUsuario") ? reader["fechaNotificacionAlUsuario"].To<DateTime>() : (DateTime?)null;
                result.UltimoMovimiento = !reader.IsDBNull("ultimoMovimiento") ? reader["ultimoMovimiento"].As<string>() : null;
                result.Ubicacion = !reader.IsDBNull("ubicacion") ? reader["ubicacion"].As<string>() : null;
                result.Revesagie = !reader.IsDBNull("reversagie") ? reader["reversagie"].To<bool>() : (bool?)null;
                result.Pori = !reader.IsDBNull("pori") ? reader["pori"].To<bool>() : (bool?)null;
                result.ImporteQueSeRestituyoAlUsuario = !reader.IsDBNull("importeQueSeRestituyoAlUsuario") ? reader["importeQueSeRestituyoAlUsuario"].As<string>() : null;
                result.FechaEnQueSeRestituyoAlUsuario = !reader.IsDBNull("fechaEnQueSeRestituyoAlUsuario") ? reader["fechaEnQueSeRestituyoAlUsuario"].To<DateTime>() : (DateTime?)null;
                result.FechaCreacion = !reader.IsDBNull("fechaCreacion") ? reader["fechaCreacion"].To<DateTime>() : (DateTime?)null;
                result.EspecifiqueCual = !reader.IsDBNull("especifiquecual") ? reader["especifiquecual"].As<string>() : null;
                result.TipoMoneda = !reader.IsDBNull("TipoMoneda") ? reader["TipoMoneda"].As<string>() : null;
                result.IdProductoServicio = !reader.IsDBNull("idProductoServicio") ? reader["idProductoServicio"].To<int>() : (int?)null;
                result.IdCausaConducef = !reader.IsDBNull("idCausa") ? reader["idCausa"].To<int>() : (int?)null;
                result.NumeroReclamaciones = !reader.IsDBNull("numeroReclamaciones") ? reader["numeroReclamaciones"].To<int>() : (int?)null;
                result.EsConcluido = !reader.IsDBNull("EsConcluido") ? reader["EsConcluido"].To<bool>() : (bool?)null;
                result.Documentos = !reader.IsDBNull("Documentos") ? JsonConvert.DeserializeObject<Object>(reader["Documentos"].As<string>()) : null;
                result.Asignacion = !reader.IsDBNull("Asignacion") ? JsonConvert.DeserializeObject<Object>(reader["Asignacion"].As<string>()) : null;
                result.Observaciones = !reader.IsDBNull("Observaciones") ? JsonConvert.DeserializeObject<Object>(reader["Observaciones"].As<string>()) : null;
            }
            reader.Close();

            return result;
        }

        public async Task<ConducefReporteGeneralOutput> ConducefReporteGeneral(ConducefReporteGeneralInput input)
        {
            var result = new ConducefReporteGeneralOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConducefReporteGeneral", CommandType.StoredProcedure,
                new SqlParameter("@idRamoBusqueda", input.IdRamoBusqueda),
                new SqlParameter("@idArea", input.IdArea),
                new SqlParameter("@idDepartamentoUno", input.IdDepartamento1),
                new SqlParameter("@idDepartamentoDos", input.IdDepartamento2),
                new SqlParameter("@fechaReporteInicial", input.FechaReporteInicial),
                new SqlParameter("@fechaReporteFinal", input.FechaReporteFinal));

            var reader = await mCommand.ExecuteReaderAsync();

            while (reader.Read())
            {
                result.Reportes.Add(new ConducefReporteGeneralOutput.ReporteGeneralDto
                {
                    NumeroExpediente = !reader.IsDBNull("numeroExpediente") ? reader["numeroExpediente"].As<string>() : null,
                    Poliza = !reader.IsDBNull("poliza") ? reader["poliza"].As<string>() : null,
                    Siniestro = !reader.IsDBNull("siniestro") ? reader["siniestro"].As<string>() : null,
                    NombreAsegurado = !reader.IsDBNull("nombreAsegurado") ? reader["nombreAsegurado"].As<string>() : null,
                    ClaveAgente = !reader.IsDBNull("claveAgente") ? reader["claveAgente"].As<string>() : null,
                    NombreAgente = !reader.IsDBNull("nombreAgente") ? reader["nombreAgente"].As<string>() : null,
                    FechaOcurrido = !reader.IsDBNull("fechaOcurrido") ? reader["fechaOcurrido"].To<DateTime>() : (DateTime?)null,
                    Dependencia = !reader.IsDBNull("Dependencia") ? reader["Dependencia"].As<string>() : null,
                    CP = !reader.IsDBNull("cp") ? reader["cp"].As<string>() : null,
                    Entidad = !reader.IsDBNull("Entidad") ? reader["Entidad"].As<string>() : null,
                    ClaveEntidadFederativa = !reader.IsDBNull("claveEntidadFederativa") ? reader["claveEntidadFederativa"].To<int>() : (int?)null,
                    Partes = !reader.IsDBNull("partes") ? reader["partes"].As<string>() : null,
                    NaturalezaActor = !reader.IsDBNull("NaturalezaActor") ? reader["NaturalezaActor"].As<string>() : null,
                    Ramo = !reader.IsDBNull("ramo") ? reader["ramo"].As<string>() : null,
                    Area = !reader.IsDBNull("Area") ? reader["Area"].As<string>() : null,
                    Caracteristicas = !reader.IsDBNull("catacteristicas") ? reader["catacteristicas"].As<string>() : null,
                    Departamento1 = !reader.IsDBNull("idDepartamento1") ? reader["idDepartamento1"].As<string>() : null,
                    Departamento2 = !reader.IsDBNull("idDepartamento2") ? reader["idDepartamento2"].As<string>() : null,
                    Particularidad = !reader.IsDBNull("particularidad") ? reader["particularidad"].As<string>() : null,
                    MontoReclamado = !reader.IsDBNull("montoReclamado") ? reader["montoReclamado"].As<string>() : null,
                    DescripcionMonedaRecepcion = !reader.IsDBNull("DescripcionMedioRecepcion") ? reader["DescripcionMedioRecepcion"].As<string>() : null,
                    ClaveMedioRecepcion = !reader.IsDBNull("claveMedioRecepcion") ? reader["claveMedioRecepcion"].As<string>() : null,
                    Solantec = !reader.IsDBNull("solantec") ? reader["solantec"].To<DateTime>() : (DateTime?)null,
                    FechaRespuestaArea = !reader.IsDBNull("fechaRespuestaArea") ? reader["fechaRespuestaArea"].To<DateTime>() : (DateTime?)null,
                    FechaResolucionConclusion = !reader.IsDBNull("fechaResolucionConclusion") ? reader["fechaResolucionConclusion"].To<DateTime>() : (DateTime?)null,
                    UltimoMovimiento = !reader.IsDBNull("ultimoMovimiento") ? reader["ultimoMovimiento"].As<string>() : null,
                    Ubicacion = !reader.IsDBNull("ubicacion") ? reader["ubicacion"].As<string>() : null,
                    ImporteQueSeRestituyoAlUsuario = !reader.IsDBNull("importeQueSeRestituyoAlUsuario") ? reader["importeQueSeRestituyoAlUsuario"].As<string>() : null,
                    FechaEnQueSeRestituyoAlUsuario = !reader.IsDBNull("fechaEnQueSeRestituyoAlUsuario") ? reader["fechaEnQueSeRestituyoAlUsuario"].To<DateTime>() : (DateTime?)null,
                    TipoMoneda = !reader.IsDBNull("TipoMoneda") ? reader["TipoMoneda"].As<string>() : null,
                    Comentario = !reader.IsDBNull("comentario") ? reader["comentario"].As<string>() : null,
                    MRDolares = !reader.IsDBNull("MRDolares") ? reader["MRDolares"].As<string>() : null,
                    FechaTermino = !reader.IsDBNull("fechaTermino") ? reader["fechaTermino"].To<DateTime>() : (DateTime?)null
                });                
            }
            reader.Close();
            return result;
        }

        public async Task<ConducefReporteTrimestralOutput> ConducefReporteTrimestral(ConducefReporteTrimestralInput input)
        {
            var result = new ConducefReporteTrimestralOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConducefReporteTrimestral", CommandType.StoredProcedure,
                new SqlParameter("@idRamoBusqueda", input.IdRamoBusqueda),
                new SqlParameter("@idArea", input.IdArea),
                new SqlParameter("@fechaReportetrimestralInicial", input.FechaReporteTrimestralInicial),
                new SqlParameter("@fechaReporteTrimestralFinal", input.FechaReporteTrimestralFinal));

            var reader = await mCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Reportes.Add(new ConducefReporteTrimestralOutput.ReporteDto
                {
                    Folio = !reader.IsDBNull("Folio") ? reader["Folio"].As<string>() : null,
                    FechaRecepcion = !reader.IsDBNull("fechaRecepcion") ? reader["fechaRecepcion"].To<DateTime>() : (DateTime?)null,
                    CP = !reader.IsDBNull("cp") ? reader["cp"].As<string>() : null,
                    Entidad = !reader.IsDBNull("Entidad") ? reader["Entidad"].As<string>() : null,
                    RamoBusqueda = !reader.IsDBNull("ramoBusqueda") ? reader["ramoBusqueda"].As<string>() : null,
                    MontoReclamado = !reader.IsDBNull("montoReclamado") ? reader["montoReclamado"].As<string>() : null,
                    ClaveMedioRecepcion = !reader.IsDBNull("claveMedioRecepcion") ? reader["claveMedioRecepcion"].To<int>() : (int?)null,
                    FechaResolucionConclusion = !reader.IsDBNull("fechaResolucionConclusion") ? reader["fechaResolucionConclusion"].To<DateTime>() : (DateTime?)null,
                    EstadoConcluidoPendiente = !reader.IsDBNull("EstadoConcluidoPendiente") ? reader["EstadoConcluidoPendiente"].As<string>() : null,
                    FechaNotificacionUsuario = !reader.IsDBNull("fechaNotificacionAlUsuario") ? reader["fechaNotificacionAlUsuario"].To<DateTime>() : (DateTime?)null,
                    Reversagie = !reader.IsDBNull("reversagie") ? reader["reversagie"].To<bool>() : (bool?)null,
                    Pori = !reader.IsDBNull("pori") ? reader["pori"].To<bool>() : (bool?)null,
                    ImporteRestituyoUsuario = !reader.IsDBNull("importeQueSeRestituyoAlUsuario") ? reader["importeQueSeRestituyoAlUsuario"].As<string>() : null,
                    FechaRestituyoUsuario = !reader.IsDBNull("fechaEnQueSeRestituyoAlUsuario") ? reader["fechaEnQueSeRestituyoAlUsuario"].To<DateTime>() : (DateTime?)null,
                    DenominacionRazonSocial = !reader.IsDBNull("DenominacionRasonSocial") ? reader["DenominacionRasonSocial"].As<string>() : null,
                    Sector = !reader.IsDBNull("Sector") ? reader["Sector"].As<string>() : (string?)null,
                    TrimestreAInformar = !reader.IsDBNull("TrimestreAInformar") ? reader["TrimestreAInformar"].To<DateTime>() : (DateTime?)null,
                    ProductoServicio = !reader.IsDBNull("ProductoServicio") ? reader["ProductoServicio"].As<string>() : (string?)null,
                    NumeroReclamaciones = !reader.IsDBNull("NumeroReclamaciones") ? reader["NumeroReclamaciones"].To<int>() : (int?)null,
                    CausaConducef = !reader.IsDBNull("Causa") ? reader["Causa"].As<string>() : (string?)null
                });
            }
            reader.Close();
            return result;
        }

        public void GuardarTrackingConducef(GuardarTrackingConducefInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroConducefTracking", CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumeroConducef),
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

        public void GuardarSiniestroConducef(GuardarSiniestroConducefInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroConducefSiniestro", CommandType.StoredProcedure,
                new SqlParameter("@idSiniestro", input.IdSiniestro),
                new SqlParameter("@ramoBusqueda", input.IdRamoBusqueda),
                new SqlParameter("@poliza", input.Poliza),
                new SqlParameter("@siniestro", input.Siniestro),
                new SqlParameter("@nombreAsegurado", input.NombreAsegurado),
                new SqlParameter("@claveAgente", input.ClaveAgente),
                new SqlParameter("@nombreAgente", input.NombreAgente),
                new SqlParameter("@fechaOcurrido", input.FechaOcurrido),
                new SqlParameter("@honorariosPagados", input.HonorariosPagados),
                new SqlParameter("@fechaPagoHonorarios", input.FechaHonorariosPagados),
                new SqlParameter("@otrosGastos", input.OtrosGastos),
                new SqlParameter("@numeroConducef", input.NumeroConducef));

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

        public void GuardarObservacionConducef(GuardarObservacionConducefInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroConducefObservacion", CommandType.StoredProcedure,
                new SqlParameter("@idObservacion", input.IdObservacion),
                new SqlParameter("@realizadoPor", input.RealizadoPor),
                new SqlParameter("@fechaCreacion", input.FechaCreacion),
                new SqlParameter("@comentario", input.Comentario),
                new SqlParameter("@numeroConducef", input.NumeroConducef));

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

        public void GuardarComentarioConducef(GuardarComentarioConducefInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroConducefComentario", CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.NumeroConducef),
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

        public void GuardarDocumentoConducef(GuardarDocumentoConducefInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroConducefDocumentos", CommandType.StoredProcedure,
                new SqlParameter("@idDocumento", input.IdDocumento),
                new SqlParameter("@objeto", input.ObjectId),                
                new SqlParameter("@nombre", input.Nombre),
                new SqlParameter("@tipoMime", input.TipoMime),
                new SqlParameter("@tipoDocumento", input.TipoDocumento),
                new SqlParameter("@subtipoDocumento", input.SubTipoDocumento),
                new SqlParameter("@idConducef", input.IdConducef),
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

        public async Task<RegistroConducefOutput> RegistroConducef(RegistroConducefInput input)
        {
            var result = new RegistroConducefOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroConducef", CommandType.StoredProcedure,
                new SqlParameter("@idConducef", input.IdConducef),
                new SqlParameter("@idExpedienteProvieneDe", input.IdExpedienteProvieneDe),
                new SqlParameter("@numeroExpediente", input.NumeroExpediente),
                new SqlParameter("@fechaRecepcion", input.FechaRecepcion),
                new SqlParameter("@trimestreInforma", input.TrimestreInforma),
                new SqlParameter("@fechaTermino", input.FechaTermino),
                new SqlParameter("@cuenntaConSiniestro", input.CuentaConSiniestro),
                new SqlParameter("@idDependencia", input.IdDependencia),
                new SqlParameter("cp", input.CP),
                new SqlParameter("@idEntidad", input.IdEntidad),
                new SqlParameter("@claveEntidadFederativa", input.ClaveEntidadFederativa),
                new SqlParameter("@partes", input.Partes),
                new SqlParameter("@idNaturalezaActor", input.IdNaturalezaActor),
                new SqlParameter("@ramo", input.Ramo),
                new SqlParameter("@idArea", input.IdArea),
                new SqlParameter("@catacteristicas", input.Caracteristicas),
                new SqlParameter("@idDepartamento1", input.IdDepartamento1),
                new SqlParameter("@idDepartamento2", input.IdDepartamento2),
                new SqlParameter("@particularidad", input.Particularidad),
                new SqlParameter("@montoReclamado", input.MontoReclamado),
                new SqlParameter("@sinCuantificar", input.SinCuantificar),
                new SqlParameter("@idDescripcionDelMedioRecepcion", input.IdDescripcionMedioRecepcion),
                new SqlParameter("@claveMedioRecepcion", input.ClaveMedioRecepcion),
                new SqlParameter("@solantec", input.Solantec),
                new SqlParameter("@fechaRespuestaArea", input.FechaRespuestaArea),
                new SqlParameter("@fechaResolucionConclusion", input.FechaResolucionConclusion),
                new SqlParameter("@idEstadoConcluidoPendiente", input.IdEstadoConcluidoPendiente),
                new SqlParameter("@desctripcionEstadoConcluidoPendiente", input.DescripcionEstadoConcluidoPendiente),
                new SqlParameter("@fechaNotificacionAlUsuario", input.FechaNotificacionUsuario),
                new SqlParameter("@ultimoMovimiento", input.UltimoMovimiento),
                new SqlParameter("@ubicacion", input.Ubicacion),
                new SqlParameter("@reversagie", input.Reversagie),
                new SqlParameter("@pori", input.Pori),
                new SqlParameter("@importeQueSeRestituyoAlUsuario", input.ImporteRestituyoUsuario),
                new SqlParameter("@fechaEnQueSeRestituyoAlUsuario", input.FechaRestituyoUsuario),
                new SqlParameter("@fechaCreacion", input.FechaCreacion),
                new SqlParameter("@especifiquecual", input.EspecifiqueCual),
                new SqlParameter("@idTipoMoneda", input.IdTipoMoneda),
                new SqlParameter("@numeroConducef", input.NumeroConducef),
                new SqlParameter("@numeroReclamaciones", input.NumeroReclamaciones),
                new SqlParameter("@idProductoServicio", input.IdProductoServicio),
                new SqlParameter("@idCausa", input.IdCausaConducef),
                new SqlParameter("@idRegExp", input.IdRegExp));

            var reader = await mCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string msj = reader["Mensaje"].As<string>();
                if ((reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                {
                    result.Mensaje = "Operacion Exitosa";
                    result.Id = !reader.IsDBNull("Id") ? reader["Id"].To<int>() : 0;
                    result.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : 0;
                    result.NumeroConducef = !reader.IsDBNull("ExpedienteConducef") ? reader["ExpedienteConducef"].As<string>() : null;
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

        public void ConcluirRegistroConducef(ConcluirRegistroConducefInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConducefConcluir", CommandType.StoredProcedure,
                new SqlParameter("@numeroConducef", input.NumeroConducef),
                new SqlParameter("@concluir", input.Concluir));

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

        public void AsignarAbogadoConducef(AsignarAbogadoConducefInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroConducefAbogado", CommandType.StoredProcedure,
                new SqlParameter("@idAbogadoAsignado", input.IdAbogado),
                new SqlParameter("@numeroConducef", input.NumeroConducef),
                new SqlParameter("@esInterno", input.EsInterno),
                new SqlParameter("@idProveedor", input.IdProveedor),
                new SqlParameter("@fechaCreacion", DateTime.Now));

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

        public string ObtenerObjectIdConducef(ObtenerObjectIdConducefInput input)
        {
            string texto = $"SELECT objeto FROM OpeConducefDocumentos WHERE idDocumento = {input.IdDocumento}";
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

        public ObtenerIdRegExpConducefOutput ObtenerIdRegExpConducef(int IdConducef)
        {
            var output = new ObtenerIdRegExpConducefOutput();
            string texto = $"SELECT idRegExp, NumeroConducef FROM OpeConducef WHERE idConducef = {IdConducef}";

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);
            using (var reader = mCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    output.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                    output.NumeroConducef = !reader.IsDBNull("NumeroConducef") ? reader["NumeroConducef"].As<string>() : null;
                }
            }
            return output;
        }

        public async Task<ObtenerDocumentoConducefOutput> ObtenerDocumentoConducef(ObtenerDocumentoConducefInput input)
        {
            var result = new ObtenerDocumentoConducefOutput();
            EnsureConnectionOpen();
            string texto = $"SELECT * FROM OpeConducefDocumentos WHERE idDocumento = {input.IdDocumento}";
            using (var mCommand = CreateCommand(texto, CommandType.Text))
            {
                var reader = mCommand.ExecuteReader();
                while (reader.Read())
                {
                    result.IdDocumento = !reader.IsDBNull(reader.GetOrdinal("idDocumento")) ? reader["idDocumento"].To<int>() : 0;
                    result.IdConducef = !reader.IsDBNull(reader.GetOrdinal("idConducef")) ? reader["idConducef"].To<int>() : 0;
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

        public async Task<ObtenerTrackingsConducefOutput> ObtenerTrackingsConducef(ObtenerTrackingsConducefInput input)
        {
            var result = new ObtenerTrackingsConducefOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaConducefTracking", CommandType.StoredProcedure,
                new SqlParameter("@numeroConducef", input.NumeroConducef));

            using (var reader = await mCommand.ExecuteReaderAsync())
            {
                var mError = HasColumn(reader, "CodigoMmensaje");
                if (mError)
                {
                    reader.Close();
                    throw new UserFriendlyException(400, reader["CodigoMmensaje"].As<string>());
                }                   

                while (reader.Read())
                {
                    result.Trackings.Add(new ObtenerTrackingsConducefOutput.TrackingDto
                    {
                        IdTracking = !reader.IsDBNull("idDemandaTracking") ? reader["idDemandaTracking"].To<int>() : (int?)null,
                        FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : (DateTime?)null,
                        UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                        Movimiento = !reader.IsDBNull("movimiento") ? reader["movimiento"].As<string>() : null,
                        NumeroConducef = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null
                    });
                }
            }
            return result;
        }

        public async Task<ObtenerComentariosConducefOutput> ObtenerComentariosConducef(ObtenerComentariosConducefInput input)
        {
            var result = new ObtenerComentariosConducefOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaConducefComentario", CommandType.StoredProcedure,
                new SqlParameter("@numeroConducef", input.NumeroConducef));

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
                    result.Comentarios.Add(new ObtenerComentariosConducefOutput.ComentarioDto
                    {
                        IdComentario = !reader.IsDBNull("idDemandaComentario") ? reader["idDemandaComentario"].To<int>() : (int?)null,
                        NumeroConducef = !reader.IsDBNull("numeroDemanda") ? reader["numeroDemanda"].As<string>() : null,
                        FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : (DateTime?)null,
                        UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null,
                        Comentario = !reader.IsDBNull("comentario") ? reader["comentario"].As<string>() : null
                    });
                }
            }
            return result;
        }

        public async Task<ObtenerObservacionesConducefOutput> ObtenerObservacionesConducef(ObtenerObservacionesConducefInput input)
        {
            var result = new ObtenerObservacionesConducefOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaConducefObservacion", CommandType.StoredProcedure,
                new SqlParameter("@numeroConducef", input.NumeroConducef));

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
                    result.Observaciones.Add(new ObtenerObservacionesConducefOutput.ObservacionDto
                    {
                        IdObservacion = !reader.IsDBNull("idObservacion") ? reader["idObservacion"].To<int>() : (int?)null,
                        NumeroConducef = !reader.IsDBNull("numeroConducef") ? reader["numeroConducef"].As<string>() : null,
                        RealizadoPor = !reader.IsDBNull("realizadoPor") ? reader["realizadoPor"].As<string>() : null,
                        FechaCreacion = !reader.IsDBNull("fechaCreacion") ? reader["fechaCreacion"].To<DateTime>() : (DateTime?)null,
                        Comentario = !reader.IsDBNull("comentario") ? reader["comentario"].As<string>() : null
                    });
                }
            }
            return result;
        }

        public void GuardarSiniestroSisa(GuardarSiniestroConducefInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spRegistroSisaConducef", CommandType.StoredProcedure,
                new SqlParameter("@ramoBusqueda", input.IdRamoBusqueda),
                new SqlParameter("@poliza", input.Poliza),
                new SqlParameter("@siniestro", input.Siniestro),
                new SqlParameter("@nombreAsegurado", input.NombreAsegurado),
                new SqlParameter("@claveAgente", input.ClaveAgente),
                new SqlParameter("@nombreAgente", input.NombreAgente),
                new SqlParameter("@fechaOcurrido", input.FechaOcurrido),
                new SqlParameter("@honorariosPagados", input.HonorariosPagados),
                new SqlParameter("@fechaPagoHonorarios", input.FechaHonorariosPagados),
                new SqlParameter("@otrosGastos", input.OtrosGastos),
                new SqlParameter("@numeroConducef", input.NumeroConducef));

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
    }
}
