using Abp.Data;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Atlas.Legal.EntityFrameworkCore;
using Atlas.Legal.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Abp.UI;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Atlas.Legal
{
    public class DemandasRepository : LegalRepositoryBase<OpeDemanda, int>, IDemandasRepository
    {
        public DemandasRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            :base(dbContextProvider, transactionProvider)
        {

        }

        public GuardarActualizarRegistroDemandaOutput GuardarActualizarRegistroDemanda(GuardarActualizarRegistroDemandaInput input)
        {
            var output = new GuardarActualizarRegistroDemandaOutput();
            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spGuardaRegistroDemanda",
                CommandType.StoredProcedure,
                new SqlParameter("@idTipoMateria", input.IdTipoMateria),
                new SqlParameter("@EspecifiqueMateria", input.EspecifiqueMateria),
                new SqlParameter("@actor", input.Actor),
                new SqlParameter("@idNotificacionCalidad", input.IdNotificacionCalidad),
                new SqlParameter("@expedienteJuicio", input.ExpedienteJuicio),
                new SqlParameter("@coodemandados", input.Coodemandados),
                new SqlParameter("@terminoContestacion", input.TerminoContestacion),
                new SqlParameter("@fechaVencimientoContestacion", input.FechaVencimientoContestacion),
                new SqlParameter("@juzgado", input.Juzgado),
                new SqlParameter("@idTipoJuicio", input.IdTipoJuicio),
                new SqlParameter("@especifiqueCual", input.EspecifiqueCual),
                new SqlParameter("@motivoDemanda", input.MotivoDemanda),
                new SqlParameter("@juicioRelevante", input.JuicioRelevante),
                new SqlParameter("@nombreJusgado", input.NombreJuzgado),
                new SqlParameter("@idUbicacionJuicio", input.IdUbicacionJuicio),
                new SqlParameter("@montoReclamado", input.MontoReclamado),
                new SqlParameter("@MontoReclamadoSinDeterminar", input.MontoReclamadoSinDeterminar),
                new SqlParameter("@ramoBusqueda", input.SiniestroSeleccionado.RamoBusqueda),
                new SqlParameter("@poliza", input.SiniestroSeleccionado.Poliza),
                new SqlParameter("@siniestro", input.SiniestroSeleccionado.Siniestro),
                new SqlParameter("@nombreasegurado", input.SiniestroSeleccionado.NombreAsegurado),
                new SqlParameter("@reservaConducefPesos", input.SiniestroSeleccionado.ReservaCondusefPesos),
                new SqlParameter("@reservaConducefDolares", input.SiniestroSeleccionado.ReservaCondusefDolares),
                new SqlParameter("@reservaPesos", input.SiniestroSeleccionado.ReservaPesos),
                new SqlParameter("@reservaDolares", input.SiniestroSeleccionado.ReservaDolares),
                new SqlParameter("@otrosGastos", input.SiniestroSeleccionado.OtrosGastos),
                new SqlParameter("@fechaPagoHonorarios", input.SiniestroSeleccionado.FechaPagoHonorarios),
                new SqlParameter("@condena", input.SiniestroSeleccionado.Condena),
                new SqlParameter("@fecha", input.SiniestroSeleccionado.FechaPago),
                new SqlParameter("@honorarioTabulador", input.SiniestroSeleccionado.HonorarioTabulador),
                new SqlParameter("@honorarioAutorisado", input.SiniestroSeleccionado.HonorarioAutorizado),
                new SqlParameter("@numDemanda", input.IdDemanda),
                new SqlParameter("@tipoMoneda", input.SiniestroSeleccionado.TipoMoneda),
                new SqlParameter("@honorariosPagados", input.SiniestroSeleccionado.HonorariosPagados),
                new SqlParameter("@fechaCreacion", DateTime.Now),

                new SqlParameter("@reaseguroFacultativo", input.SiniestroSeleccionado.ReaseguroFacultativo),
                new SqlParameter("@porcentajeParticipacionAtla", input.SiniestroSeleccionado.PorcentajeParticipacionAtlas),
                new SqlParameter("@coaseguro", input.SiniestroSeleccionado.Coaseguro),
                new SqlParameter("@colocacionEnContratos", input.SiniestroSeleccionado.ColocacionEnContratos),
                new SqlParameter("@retencionAtlas", input.SiniestroSeleccionado.RetencionAtlas),
                new SqlParameter("@sumaAseguradaPesos", input.SiniestroSeleccionado.SumaAseguradaPesos),
                new SqlParameter("@sumaAseguradaDolaes", input.SiniestroSeleccionado.SumaAseguradaDolares),
                new SqlParameter("@totalSumaAseguradaMonedaNac", input.SiniestroSeleccionado.TotalSumaAseguradaMonedaNac),
                new SqlParameter("@resultado", input.SiniestroSeleccionado.Resultado),
                new SqlParameter("@autoridadJudicial", input.AutoridadJudicial),
                new SqlParameter("@contestacionDeDemanda", input.ContestacionDeDemanda),
                new SqlParameter("@idRegExp", input.IdRegExp));

            var reader = mCommand.ExecuteReader();
            string msj = "";
            while (reader.Read())
            {
                msj = reader["Mensaje"].As<string>();

                if (!(reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                {
                    reader.Close();
                    throw new UserFriendlyException(400, msj);
                }
                output.NumDemanda = reader["NumeroDemanda"].As<string>();
                input.IdRegExp = !reader.IsDBNull(reader.GetOrdinal("idRegExp")) ? reader["idRegExp"].To<int>() : (int?)null;
                input.IdDemanda = !reader.IsDBNull(reader.GetOrdinal("NumeroDemanda")) ? reader["NumeroDemanda"].As<string>() : null;
                output.Mensaje = "Operación Exitosa";
            }
            reader.Close();
            return output;
        }

        public EliminarAsignacionOutput EliminarAsignacion(EliminarAsignacionInput input)
        {
            var output = new EliminarAsignacionOutput();
            EnsureConnectionOpen();

            var mCommand = CreateCommand(
            "EliminaRegistroAsigna",
            CommandType.StoredProcedure,
            new SqlParameter("@numeroDdemanda", input.IdDemanda),
            new SqlParameter("@idAsigna", input.IdAsigna));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Mensaje = reader["Mensaje"].As<string>();
                if (reader["CodigoMensaje"].To<int>() != 0 || reader["CodigoMensaje"].To<int>() != 1)
                {
                    reader.Close();
                    throw new UserFriendlyException(400, output.Mensaje);
                }
                else
                {
                    output.Mensaje = "Operación Exitosa";
                }
            }
            return output;
        }

        public AsignarActualizarAbogadoOutput AsignarActualizarAbogado(AsignarActualizarAbogadoInput input)
        {
            var output = new AsignarActualizarAbogadoOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spGuardaActualizaRegistroAsigna",
                CommandType.StoredProcedure,
                new SqlParameter("@idAsigna", input.IdAsigna),
                new SqlParameter("@fechaEmplazamiento", input.FechaEmplazamiento),
                new SqlParameter("@NotificacionEmplazamiento", input.NotificacionEmplazamiento),
                new SqlParameter("@idAbogadoDesignadoAtlas", input.IdAbogadoDesignadoAtlas),
                new SqlParameter("@idAbogadoDesignadoInvolucrado", input.IdAbogadoDesignadoInvolucrado),
                new SqlParameter("@numeroDemanda", input.IdDemanda),
                new SqlParameter("@idTipoInvolucrado", input.IdTipoInvolucrado)
                );

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Mensaje = reader["Mensaje"].As<string>();
                if (!(reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                {
                    reader.Close();
                    throw new UserFriendlyException(400, output.Mensaje);
                }
                else
                {
                    output.Mensaje = "Operación Exitosa";
                }
            }
            reader.Close();
            return output;
        }

        public ConsultaRegistroDemandaOutput ConsultaRegistroDemanda(ConsultarRegistroDemandaInput input)
        {
            var output = new ConsultaRegistroDemandaOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaLitigio",
                CommandType.StoredProcedure,
                new SqlParameter("@numSiniestro", input.NumeroSiniestro),
                new SqlParameter("@expedienteJuicio", input.ExpedienteJuicio),
                new SqlParameter("@fechaRegistroInicial", input.FechaRegistroInicial),
                new SqlParameter("@fechaRegistroFinal", input.FechaRegistroFinal));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.ConsultaDemanda.Add(new ConsultaDemandaDto
                {                    
                    IdDemanda = !reader.IsDBNull(reader.GetOrdinal("IdDemanda")) ? reader["IdDemanda"].To<int>() : 0,
                    Actor = !reader.IsDBNull(reader.GetOrdinal("Actor")) ? reader["Actor"].As<string>().Trim() : null,
                    NumeroSiniestro = !reader.IsDBNull(reader.GetOrdinal("Siniestro")) ? reader["Siniestro"].As<string>().Trim() : null,
                    Ramo = !reader.IsDBNull(reader.GetOrdinal("Ramo")) ? reader["Ramo"].As<string>().Trim() : null,
                    EtapaProcesal = !reader.IsDBNull(reader.GetOrdinal("EtapaProcesal")) ? reader["EtapaProcesal"].As<string>().Trim() : null,
                    Ubicacion = !reader.IsDBNull(reader.GetOrdinal("UbicacionJuicio")) ? reader["UbicacionJuicio"].As<string>().Trim() : null,
                    Juzgado = !reader.IsDBNull(reader.GetOrdinal("Jusgado")) ? reader["Jusgado"].As<string>().Trim() : null,
                    //AbogadoDesignadoAtlas = !reader.IsDBNull(reader.GetOrdinal("AbogadoDesignadoAtlas")) ? reader["AbogadoDesignadoAtlas"].As<string>().Trim() : null,
                    //AbogadoDesignadoInvolucrado = !reader.IsDBNull(reader.GetOrdinal("AbogadoDesignadoInvolucrado")) ? reader["AbogadoDesignadoInvolucrado"].As<string>().Trim() : null,
                    Asignacion = !reader.IsDBNull("asignacion") ? JsonConvert.DeserializeObject<Object>(reader["asignacion"].As<string>()) : null,
                    TipoJuicio = !reader.IsDBNull(reader.GetOrdinal("TipoJuicio")) ? reader["TipoJuicio"].As<string>().Trim() : null,
                    Materia = !reader.IsDBNull(reader.GetOrdinal("Materia")) ? reader["Materia"].As<string>().Trim() : null,
                    Expediente = !reader.IsDBNull(reader.GetOrdinal("Expediente")) ? reader["Expediente"].As<string>().Trim() : null,
                    Codemandados = !reader.IsDBNull(reader.GetOrdinal("Codemandados")) ? reader["Codemandados"].As<string>().Trim() : null,
                    TerminoContestacion = !reader.IsDBNull(reader.GetOrdinal("TerminoContestacion")) ? reader["TerminoContestacion"].To<int>() : (int?)null,
                    AutoridadJudicial = !reader.IsDBNull(reader.GetOrdinal("AutoridadJudicial")) ? reader["AutoridadJudicial"].As<string>().Trim() : null,
                    MotivoDemanda = !reader.IsDBNull(reader.GetOrdinal("motivoDemanda")) ? reader["motivoDemanda"].As<string>().Trim() : null,
                    JuicioRelevante = !reader.IsDBNull(reader.GetOrdinal("juicioRelevante")) ? reader["juicioRelevante"].To<bool>(): (bool?)null,
                    NombreJuzgado = !reader.IsDBNull(reader.GetOrdinal("NombreJusgado")) ? reader["NombreJusgado"].As<string>().Trim() : null,
                    Monto = !reader.IsDBNull(reader.GetOrdinal("Monto")) ? reader["Monto"].As<string>().Trim() : null,
                    Reclamo = !reader.IsDBNull(reader.GetOrdinal("Reclamo")) ? reader["Reclamo"].As<string>().Trim() : null,
                    MontoReclamadoSinDeterminar = !reader.IsDBNull(reader.GetOrdinal("MontoReclamadoSinDeterminar")) ? reader["MontoReclamadoSinDeterminar"].As<string>().Trim() : null
                });
            }
            reader.Close();

            return output;

        }

        public void GuardarTracking(string numeroDemanda, string usuario, DateTime fechaRegistro, string movimiento)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spRegistroDemandaTracking",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", numeroDemanda),
                new SqlParameter("@fechaRegistro", fechaRegistro),
                new SqlParameter("@usuarioRegistro", usuario),
                new SqlParameter("@movimiento", movimiento));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                if (reader["CodigoMmensaje"].To<int>() != 0)
                {
                    reader.Close();
                    throw new UserFriendlyException(400, "No existe el numero de Demanda");
                }
            }
            reader.Close();
        }

        public void GuardarComentario(GuardarComentarioInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spRegistroDemandaComentario",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.IdDemanda),
                new SqlParameter("@fechaRegistro", DateTime.Now),
                new SqlParameter("@usuarioRegistro", "UusarioPruebas"),
                new SqlParameter("@comentario", input.Comentario));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                string mensaje = reader["Mensaje"].As<string>();

                if (reader["CodigoMmensaje"].To<int>() != 0)
                {
                    reader.Close();
                    throw new UserFriendlyException(400, mensaje);
                }

            }
            reader.Close();
        }

        public ConsultaDemandaTrackingOutput ConsultaDemandaTracking(ConsultaDemandaTrackingInput input)
        {
            var output = new ConsultaDemandaTrackingOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaDemandaTracking",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroConducef", input.IdDemanda));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Trackings.Add(new ConsultaTrackingDto
                {
                    Id = !reader.IsDBNull(reader.GetOrdinal("idDemandaTracking")) ? reader["idDemandaTracking"].To<int>() : 0,
                    IdDemanda = !reader.IsDBNull(reader.GetOrdinal("numeroDemanda")) ? reader["numeroDemanda"].As<string>() : null,
                    FechaRegistro = !reader.IsDBNull(reader.GetOrdinal("fechaRegistro")) ? reader["fechaRegistro"].To<DateTime>() : DateTime.MinValue,
                    UsuarioRegistro = !reader.IsDBNull(reader.GetOrdinal("usuarioRegistro")) ? reader["usuarioRegistro"].As<string>() : null,
                    Movimiento = !reader.IsDBNull(reader.GetOrdinal("movimiento")) ? reader["movimiento"].As<string>() : null,
                });
            }
            reader.Close();
            return output;

        }

        public ConsultaDemandaComentarioOutput ConsultaDemandaComentario(ConsultaDemandaComentarioInput input)
        {
            var output = new ConsultaDemandaComentarioOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaDemandaComentario",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDemanda", input.IdDemanda));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Comentarios.Add(new ConsultarComentarioDto
                {
                    Id = !reader.IsDBNull(reader.GetOrdinal("idDemandaComentario")) ? reader["idDemandaComentario"].To<int>() : 0,
                    IdDemanda = !reader.IsDBNull(reader.GetOrdinal("numeroDemanda")) ? reader["numeroDemanda"].As<string>() : null,
                    FechaRegistro = !reader.IsDBNull(reader.GetOrdinal("fechaRegistro")) ? reader["fechaRegistro"].To<DateTime>() : DateTime.MinValue,
                    UsuarioRegistro = !reader.IsDBNull(reader.GetOrdinal("usuarioRegistro")) ? reader["usuarioRegistro"].As<string>() : null,
                    Comentario = !reader.IsDBNull(reader.GetOrdinal("comentario")) ? reader["comentario"].As<string>() : null,
                });
            }
            reader.Close();
            return output;
        }

        public ObtenerDetalleDemandaOutput ObtenerDetalleDemanda(ObtenerDetalleDemandaInput input)
        {
            var output = new ObtenerDetalleDemandaOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spVerDetalle",
                CommandType.StoredProcedure,
                new SqlParameter("@numeroDdemanda", input.IdDemanda),
                new SqlParameter("@numSiniestro", input.NumeroSiniestro));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Detalle = !reader.IsDBNull(reader.GetOrdinal("VerDetalle")) ? JsonConvert.DeserializeObject<Object>(reader["VerDetalle"].As<string>()) : null;
            }
            reader.Close();

            return output;
        }

        public ConsultaReporteJuiciosOutput ConsultaReporteJuicios(ConsultaReporteJuiciosInput input)
        {
            var output = new ConsultaReporteJuiciosOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaReporte",
                CommandType.StoredProcedure,
                new SqlParameter("@idTipoReporte", input.IdTipoReporte),
                new SqlParameter("@fechaRegistroInicial", input.FechaRegistroInicial),
                new SqlParameter("@fechaRegistroFinal", input.FechaRegistroFinal),
                new SqlParameter("@idproveedor", input.IdProveedor));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Reporte.Add(new ConsultaReporteDto
                {
                    Id = !reader.IsDBNull(reader.GetOrdinal("idDemanda")) ? reader["idDemanda"].To<int>() : 0,
                    Actor = !reader.IsDBNull(reader.GetOrdinal("actor")) ? reader["actor"].As<string>() : null,
                    Poliza = !reader.IsDBNull(reader.GetOrdinal("poliza")) ? reader["poliza"].As<string>() : null,
                    Siniestro = !reader.IsDBNull(reader.GetOrdinal("siniestro")) ? reader["siniestro"].As<string>() : null,
                    RamoBusqueda = !reader.IsDBNull(reader.GetOrdinal("ramoBusqueda")) ? reader["ramoBusqueda"].As<string>() : null,
                    Juzgado = !reader.IsDBNull(reader.GetOrdinal("juzgado")) ? reader["juzgado"].As<string>() : null,
                    TipoJuicio = !reader.IsDBNull(reader.GetOrdinal("TipoJuicio")) ? reader["TipoJuicio"].As<string>() : null,
                    EspecifiqueCual = !reader.IsDBNull(reader.GetOrdinal("especifiqueCual")) ? reader["especifiqueCual"].As<string>() : null,
                    ExpedienteJuicio = !reader.IsDBNull(reader.GetOrdinal("expedienteJuicio")) ? reader["expedienteJuicio"].As<string>() : null,
                    AbogadoDesignadoAtlas = !reader.IsDBNull(reader.GetOrdinal("abogadoDesignadoAtlas")) ? reader["abogadoDesignadoAtlas"].As<string>() : null,
                    AbogadoDesignadoInvolucrado = !reader.IsDBNull(reader.GetOrdinal("AbogadoDesignadoInvolucrado")) ? reader["AbogadoDesignadoInvolucrado"].As<string>() : null,
                    MontoReclamado = !reader.IsDBNull(reader.GetOrdinal("montoReclamado")) ? reader["montoReclamado"].As<string>() : null,
                    TipoMoneda = !reader.IsDBNull(reader.GetOrdinal("tipoMoneda")) ? reader["tipoMoneda"].As<string>() : null,
                    ReservaConducefPesos = !reader.IsDBNull(reader.GetOrdinal("reservaConducefPesos")) ? reader["reservaConducefPesos"].As<string>() : null,
                    ReservaConducefDolares = !reader.IsDBNull(reader.GetOrdinal("reservaConducefDolares")) ? reader["reservaConducefDolares"].As<string>() : null,
                    ReservaPesos = !reader.IsDBNull(reader.GetOrdinal("reservaPesos")) ? reader["reservaPesos"].As<string>() : null,
                    ReservaDolares = !reader.IsDBNull(reader.GetOrdinal("reservaDolares")) ? reader["reservaDolares"].As<string>() : null,
                    OtrosHonorarios = !reader.IsDBNull(reader.GetOrdinal("OtrosHonorarios")) ? reader["OtrosHonorarios"].As<string>() : null,
                    HonorarioTabulador = !reader.IsDBNull(reader.GetOrdinal("honorarioTabulador")) ? reader["honorarioTabulador"].As<string>() : null,
                    HonorarioAutorizado = !reader.IsDBNull(reader.GetOrdinal("honorarioAutorisado")) ? reader["honorarioAutorisado"].As<string>() : null,
                    HonorariosPagados = !reader.IsDBNull(reader.GetOrdinal("honorariosPagados")) ? reader["honorariosPagados"].As<string>() : null,
                    Condena = !reader.IsDBNull(reader.GetOrdinal("condena")) ? reader["condena"].As<string>() : null,
                    Inicio = !reader.IsDBNull(reader.GetOrdinal("Inicio")) ? reader["Inicio"].To<DateTime>() : DateTime.MinValue,
                    LugarJuicio = !reader.IsDBNull(reader.GetOrdinal("lugarJuicio")) ? reader["lugarJuicio"].As<string>() : null,
                    FechaPagoHonorarios = !reader.IsDBNull(reader.GetOrdinal("fechaPagoHonorarios")) ? reader["fechaPagoHonorarios"].To<DateTime>() : DateTime.MinValue,
                    FechaCondena = !reader.IsDBNull(reader.GetOrdinal("fechaCondena")) ? reader["fechaCondena"].To<DateTime>() : DateTime.MinValue,
                    Comentario = !reader.IsDBNull(reader.GetOrdinal("comentario")) ? reader["comentario"].As<string>() : null,
                    Etapa = !reader.IsDBNull("etapa") ? JsonConvert.DeserializeObject<Object>(reader["etapa"].As<string>()) : null,
                });
            }
            reader.Close();
            return output;
        }

        public ConsultaReporteJuiciosRelevantesOutput ConsultaReporteJuiciosRelevantes(ConsultaReporteJuiciosRelevantesInput input)
        {
            var output = new ConsultaReporteJuiciosRelevantesOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaReporteJuiciosRelevantes",
                CommandType.StoredProcedure,
                new SqlParameter("@idTipoReporte", input.IdTipoReporte),
                new SqlParameter("@fechaRegistroInicial", input.FechaRegistroInicial),
                new SqlParameter("@fechaRegistroFinal", input.FechaRegistroFinal),
                new SqlParameter("@idproveedor", input.IdProveedor));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Reporte.Add(new ReporteRelevanteDto
                {
                    Asegurado = !reader.IsDBNull(reader.GetOrdinal("Asegurado")) ? reader["Asegurado"].As<string>() : null,
                    Poliza = !reader.IsDBNull(reader.GetOrdinal("poliza")) ? reader["poliza"].As<string>() : null,
                    Siniestro = !reader.IsDBNull(reader.GetOrdinal("siniestro")) ? reader["siniestro"].As<string>() : null,
                    Fecha = !reader.IsDBNull(reader.GetOrdinal("fecha")) ? reader["fecha"].To<DateTime>() : DateTime.MinValue,
                    Plaza = !reader.IsDBNull(reader.GetOrdinal("Plaza")) ? reader["Plaza"].As<string>() : null,
                    Abogado = !reader.IsDBNull(reader.GetOrdinal("Abogado")) ? reader["Abogado"].As<string>() : null,
                    EtapaProcesal = !reader.IsDBNull(reader.GetOrdinal("idEtapaProcesal")) ? (reader["idEtapaProcesal"].To<int>()).ToString() : null,
                    SumaAseguradaPesos = !reader.IsDBNull(reader.GetOrdinal("sumaAseguradaPesos")) ? reader["sumaAseguradaPesos"].As<string>() : null,
                    SumaAseguradaDolares = !reader.IsDBNull(reader.GetOrdinal("sumaAseguradaDolaes")) ? reader["sumaAseguradaDolaes"].As<string>() : null,
                    TotalSumaAseguradaMonedaNac = !reader.IsDBNull(reader.GetOrdinal("totalSumaAseguradaMonedaNac")) ? reader["totalSumaAseguradaMonedaNac"].As<string>() : null,
                    MontoReclamado = !reader.IsDBNull(reader.GetOrdinal("montoReclamado")) ? reader["montoReclamado"].As<string>() : null,
                    TipoMoneda = !reader.IsDBNull(reader.GetOrdinal("tipoMoneda")) ? reader["tipoMoneda"].As<string>() : null,
                    ReservaConducefPesos = !reader.IsDBNull(reader.GetOrdinal("reservaConducefPesos")) ? reader["reservaConducefPesos"].As<string>() : null,
                    ReservaConducefDolares = !reader.IsDBNull(reader.GetOrdinal("reservaConducefDolares")) ? reader["reservaConducefDolares"].As<string>() : null,
                    ReservaPesos = !reader.IsDBNull(reader.GetOrdinal("reservaPesos")) ? reader["reservaPesos"].As<string>() : null,
                    ReservaDolares = !reader.IsDBNull(reader.GetOrdinal("reservaDolares")) ? reader["reservaDolares"].As<string>() : null,
                    ReaseguroFacultativo = !reader.IsDBNull(reader.GetOrdinal("reaseguroFacultativo")) ? reader["reaseguroFacultativo"].As<string>() : null,
                    PorcentajeParticipacionAtlas = !reader.IsDBNull(reader.GetOrdinal("porcentajeParticipacionAtla")) ? reader["porcentajeParticipacionAtla"].As<string>() : null,
                    Coaseguro = !reader.IsDBNull(reader.GetOrdinal("coaseguro")) ? reader["coaseguro"].As<string>() : null,
                    ColocacionEnContratos = !reader.IsDBNull(reader.GetOrdinal("colocacionEnContratos")) ? reader["colocacionEnContratos"].As<string>() : null,
                    RetencionAtlas = !reader.IsDBNull(reader.GetOrdinal("retencionAtlas")) ? reader["retencionAtlas"].As<string>() : null,
                    Comentario = !reader.IsDBNull(reader.GetOrdinal("comentario")) ? reader["comentario"].As<string>() : null
                });
            }
            reader.Close();
            return output;
        }

        public ConsultaReporteJuiciosEnContraAseguradosOutput ConsultaReporteJuiciosEnContraAsegurados(ConsultaReporteJuiciosEnContraAseguradosInput input)
        {
            var output = new ConsultaReporteJuiciosEnContraAseguradosOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaReporteJuiciosEnContraAsegurados",
                CommandType.StoredProcedure,
                new SqlParameter("@idTipoReporte", input.IdTipoReporte),
                new SqlParameter("@fechaRegistroInicial", input.FechaRegistroInicial),
                new SqlParameter("@fechaRegistroFinal", input.FechaRegistroFinal),
                new SqlParameter("@idproveedor", input.IdProveedor));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Reporte.Add(new ReporteEnContraAseguradoDto
                {
                    Asegurado = !reader.IsDBNull(reader.GetOrdinal("Asegurado")) ? reader["Asegurado"].As<string>() : null,
                    Poliza = !reader.IsDBNull(reader.GetOrdinal("poliza")) ? reader["poliza"].As<string>() : null,
                    Siniestro = !reader.IsDBNull(reader.GetOrdinal("siniestro")) ? reader["siniestro"].As<string>() : null,
                    Fecha = !reader.IsDBNull(reader.GetOrdinal("fecha")) ? reader["fecha"].To<DateTime>() : DateTime.MinValue,
                    Plaza = !reader.IsDBNull(reader.GetOrdinal("Plaza")) ? reader["Plaza"].As<string>() : null,
                    Abogado = !reader.IsDBNull(reader.GetOrdinal("Abogado")) ? reader["Abogado"].As<string>() : null,
                    EtapaProcesal = !reader.IsDBNull(reader.GetOrdinal("idEtapaProcesal")) ? (reader["idEtapaProcesal"].To<int>()).ToString() : null,
                    MontoReclamado = !reader.IsDBNull(reader.GetOrdinal("montoReclamado")) ? reader["montoReclamado"].As<string>() : null,
                    ReservaConducefPesos = !reader.IsDBNull(reader.GetOrdinal("reservaConducefPesos")) ? reader["reservaConducefPesos"].As<string>() : null,
                    ReservaConducefDolares = !reader.IsDBNull(reader.GetOrdinal("reservaConducefDolares")) ? reader["reservaConducefDolares"].As<string>() : null,
                    ReservaPesos = !reader.IsDBNull(reader.GetOrdinal("reservaPesos")) ? reader["reservaPesos"].As<string>() : null,
                    ReservaDolares = !reader.IsDBNull(reader.GetOrdinal("reservaDolares")) ? reader["reservaDolares"].As<string>() : null,
                    Comentario = !reader.IsDBNull(reader.GetOrdinal("comentario")) ? reader["comentario"].As<string>() : null
                });
            }
            reader.Close();
            return output;
        }

        public ConsultaReporteConcluidosOutput ConsultaReporteConcluidos(ConsultaReporteConcluidosInput input)
        {
            var output = new ConsultaReporteConcluidosOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaReporteConclusion"
                ,CommandType.StoredProcedure,
                new SqlParameter("@idTipoReporte", input.IdTipoReporte),
                new SqlParameter("@fechaRegistroInicial", input.FechaRegistroInicial),
                new SqlParameter("@fechaRegistroFinal", input.FechaRegistroFinal),
                new SqlParameter("@idproveedor", input.IdProveedor));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                output.Reporte.Add(new ReporteConcluidosDto
                {
                    Asegurado = !reader.IsDBNull(reader.GetOrdinal("Asegurado")) ? reader["Asegurado"].As<string>() : null,
                    Poliza = !reader.IsDBNull(reader.GetOrdinal("poliza")) ? reader["poliza"].As<string>() : null,
                    Siniestro = !reader.IsDBNull(reader.GetOrdinal("siniestro")) ? reader["siniestro"].As<string>() : null,
                    Ramo = !reader.IsDBNull(reader.GetOrdinal("Ramo")) ? reader["Ramo"].As<string>() : null,
                    Resultado = !reader.IsDBNull(reader.GetOrdinal("resultado")) ? reader["resultado"].To<bool>() : (bool?)null,
                    FechaInicial = !reader.IsDBNull(reader.GetOrdinal("FechaInicial")) ? reader["FechaInicial"].To<DateTime>() : DateTime.MinValue,
                    FechaFinal = !reader.IsDBNull(reader.GetOrdinal("FechaFinal")) ? reader["FechaFinal"].To<DateTime>() : DateTime.MinValue,
                    Plaza = !reader.IsDBNull(reader.GetOrdinal("Plaza")) ? reader["Plaza"].As<string>() : null,
                    TipoJuicio = !reader.IsDBNull(reader.GetOrdinal("TpoJuicio")) ? reader["TpoJuicio"].As<string>() : null,
                    Juzgado = !reader.IsDBNull(reader.GetOrdinal("juzgado")) ? reader["juzgado"].As<string>() : null,
                    Abogado = !reader.IsDBNull(reader.GetOrdinal("Abogado")) ? reader["Abogado"].As<string>() : null,
                    MontoReclamado = !reader.IsDBNull(reader.GetOrdinal("montoReclamado")) ? reader["montoReclamado"].As<string>() : null,
                    TipoMoneda = !reader.IsDBNull(reader.GetOrdinal("tipoMoneda")) ? reader["tipoMoneda"].As<string>() : null,
                    ReservaConducefPesos = !reader.IsDBNull(reader.GetOrdinal("reservaConducefPesos")) ? reader["reservaConducefPesos"].As<string>() : null,
                    ReservaConducefDolares = !reader.IsDBNull(reader.GetOrdinal("reservaConducefDolares")) ? reader["reservaConducefDolares"].As<string>() : null,
                    ReservaPesos = !reader.IsDBNull(reader.GetOrdinal("reservaPesos")) ? reader["reservaPesos"].As<string>() : null,
                    ReservaDolares = !reader.IsDBNull(reader.GetOrdinal("reservaDolares")) ? reader["reservaDolares"].As<string>() : null,
                    HonorariosPagados = !reader.IsDBNull(reader.GetOrdinal("honorariosPagados")) ? reader["honorariosPagados"].As<string>() : null,
                    HonorarioAutorizado = !reader.IsDBNull(reader.GetOrdinal("honorarioAutorisado")) ? reader["honorarioAutorisado"].As<string>() : null,
                    HonorarioTabulador = !reader.IsDBNull(reader.GetOrdinal("honorarioTabulador")) ? reader["honorarioTabulador"].As<string>() : null,
                    PagoCondena = !reader.IsDBNull(reader.GetOrdinal("PagoCondena")) ? reader["PagoCondena"].As<string>() : null,
                    FechaPagoCondena = !reader.IsDBNull(reader.GetOrdinal("FechaPagoCondena")) ? reader["FechaPagoCondena"].To<DateTime>() : DateTime.MinValue,
                    ResumenDemanda = !reader.IsDBNull(reader.GetOrdinal("ResumenDemanda")) ? reader["ResumenDemanda"].As<string>() : null,
                    Comentario = !reader.IsDBNull(reader.GetOrdinal("comentario")) ? reader["comentario"].As<string>() : null

                });
            }
            reader.Close();
            return output;
        }

        public void AgregarActualizarDocumentoLitigio(AgregarActualizarDocumentoLitigioInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spAtlaActualizaDocumentosLitigio",
                CommandType.StoredProcedure,
                new SqlParameter("@idDocumento", input.IdDocumento),
                new SqlParameter("@idDemanda", input.IdDemanda),
                new SqlParameter("@nombre", input.Nombre),
                new SqlParameter("@objeto", input.ObjectId),
                new SqlParameter("@fechaRegistro", DateTime.Now),
                new SqlParameter("@tipoMime", input.TipoMime),
                new SqlParameter("@tipoDocumento", input.TipoDocumento),
                new SqlParameter("@usuarioRegistro", input.UsuarioRegistro),
                new SqlParameter("@subTipoDocumento", input.SubTipoDocumento),
                new SqlParameter("@sentenciaIncumplimiento", input.SentenciaIncumplimiento),
                new SqlParameter("@subNombre", input.SubNombre),
                new SqlParameter("@borrar", input.Borrar));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                string msj = reader["Mensaje"].As<string>();
                if (!(reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1 || reader["CodigoMensaje"].To<int>() == 2))
                {
                    reader.Close();
                    throw new UserFriendlyException(400, msj);
                }
            }
            reader.Close();

        }

        public ObtenerRegExpOutput ObtenerRegExp(ObtenerRegExpInput input)
        {
            string texto = string.Format("SELECT idRegExp, numeroDemanda FROM OpeMIIDemanda WHERE idDemanda = {0}", input.IdDemanda);
            var result = new ObtenerRegExpOutput();

            EnsureConnectionOpen();            

            var mCommand = CreateCommand(texto, CommandType.Text);

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                result.IdRegExp = reader["idRegExp"].To<int>();
                result.NumDemanda = reader["numeroDemanda"].As<string>();
            }
            reader.Close();
            return result;
        }

        public ObtenerObjectIdOutput ObtenerObjectId(ObtenerObjectIdInput input)
        {
            var result = new ObtenerObjectIdOutput();
            string texto = string.Format("SELECT objeto FROM OpeLitigioDocumentos WHERE idDocumento = {0}", input.IdDocumento);

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                result.ObjectId = reader["objeto"].As<string>();
            }
            reader.Close();
            return result;
        }

        public ObtenerDocumentoLitigioOuput ObtenerDocumentoLitigio(ObtenerDocumentoLitigioInput input)
        {
            var result = new ObtenerDocumentoLitigioOuput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaDocumentoLitigio", CommandType.StoredProcedure, new SqlParameter("@idDocumento", input.IdDocumento));

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
            return result;
        }

        public async Task ConcluirDemandaLitigio(ConcluirDemandaLitigioInput input)
        {
            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spConcluirDemandaLitigio", CommandType.StoredProcedure,
                    new SqlParameter("@numeroDdemanda", input.NumeroDemanda),
                    new SqlParameter("@concluir", input.Concluir));

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
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
