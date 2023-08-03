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
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal.Siniestros
{
    public class SiniestrosRepository : LegalRepositoryBase<Siniestro, int>, ISiniestrosRepository
    {
        public SiniestrosRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider, transactionProvider)
        {

        }

        public void InsertOrUpdateSiniestro(SiniestroInput input)
        {
            if (input.Bitacora.NumeroReporteProveedorLegal == null || input.Siniestralidad.NumeroSiniestroAtlas == null)
                throw new UserFriendlyException(400, "Es se rquiere el NumeroReporteProveedorLegal y NumeroSiniestroAtlas");

            EnsureConnectionOpen();
            var mCommand = CreateCommand(
                    "spguardaprovedor",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idEstatus", input.Siniestralidad.IdEstatus),
                    new SqlParameter("@idSiniestro", input.Siniestralidad.Id),
                    new SqlParameter("@fechaAcreditamientoPropiedad", input.Siniestralidad.FechaDeAcreditamiendoDePropiedad),
                    new SqlParameter("@fechaLiberacionVehiculo", input.Siniestralidad.FechaDeLiberacionVehiculo),
                    new SqlParameter("@diasLiberacion", input.Siniestralidad.DíasLiberacion),
                    new SqlParameter("@idAutoridadConoce", input.Siniestralidad.IdAutoridadQueConoce),
                    new SqlParameter("@idAgravante", input.Siniestralidad.IdAgravante),
                    new SqlParameter("@numeroSiniestroAtlas", input.Siniestralidad.NumeroSiniestroAtlas),
                    new SqlParameter("@idResponsabilidadDictamen", input.Siniestralidad.IdResponsabilidadDictamen),
                    new SqlParameter("@avaluoOficial", input.Siniestralidad.AvaluoOficial),
                    new SqlParameter("@idTipoRecuperacion", input.Recuperaciones.IdTipoDeRecuperacion),
                    new SqlParameter("@parcialidad", input.Recuperaciones.Parcialidad),
                    new SqlParameter("@fechaDeConvenio", input.Recuperaciones.FechaDeConvenio),
                    new SqlParameter("@fechaDictamen", input.DictamenesFavorables.FechaDictamen),
                    new SqlParameter("@avaluoDIctamen", input.DictamenesFavorables.AvaluoDictamen),
                    new SqlParameter("@autoridadConoce", input.DictamenesFavorables.AutoridadQueConoce),
                    new SqlParameter("@especificarRecuperacion", input.DictamenesFavorables.EspecificarRecuperacion),
                    new SqlParameter("@faseProcesal", input.DictamenesFavorables.FaseProcesal),
                    new SqlParameter("@coordinador", input.DictamenesFavorables.Coordinador),
                    new SqlParameter("@gerencia", input.DictamenesFavorables.Gerencia),
                    new SqlParameter("@tipoRecuperacion", input.DictamenesFavorables.TipoRecuperacion),
                    new SqlParameter("@idDelito", input.HomicidiosYLesiones.IdDelitoFinal),
                    new SqlParameter("@fechaAcreditacionPropiedad", input.LiberacionVehiculos.FechaAcreditacion),
                    new SqlParameter("@idEstatusHL", input.HomicidiosYLesiones.IdEstatus),
                    new SqlParameter("@idEtapaProcesal", input.HomicidiosYLesiones.IdEtapaProcesal),
                    new SqlParameter("@idTipoLesionados", input.HomicidiosYLesiones.IdTipoDeLesionados),
                    new SqlParameter("@idQuienFallece", input.HomicidiosYLesiones.IdQuienFallece),
                    new SqlParameter("@liderACargo", input.VehiculosDetenidos.LiderACargo),
                    new SqlParameter("@fechaLiberacion", input.VehiculosDetenidos.FechaDeLiberacion),
                    new SqlParameter("@numeroReporteProveedorLegal", input.Bitacora.NumeroReporteProveedorLegal),
                    new SqlParameter("@numeroFallecidos", input.HomicidiosYLesiones.NumeroFallecidos),
                    new SqlParameter("@idParteAccidente", input.HomicidiosYLesiones.IdParteAccidente),
                    new SqlParameter("@idResponsabilidadDictamenHL", input.HomicidiosYLesiones.IdResponsabilidadDictamen),
                    new SqlParameter("@idRegExp", input.IdRegExp));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                if (!(reader["Mensaje"].To<int>() == 0 || reader["Mensaje"].To<int>() == 1))
                {

                    if (reader["Mensaje"].To<int>() == 2)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Numero de reporte ya existe");
                    }
                    else if (reader["Mensaje"].To<int>() == 3)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Se necesita Numero de Siniestro");
                    }
                    else if (reader["Mensaje"].To<int>() == 4)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Se necesita Numero de reporte");
                    }
                    else if (reader["Mensaje"].To<int>() == 5)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "El reporte y siniestro no coinciden");
                    }
                    else if (reader["Mensaje"].To<int>() == 6)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "No existe el siniestro");
                    }
                }

                input.IdRegExp = !reader.IsDBNull(reader.GetOrdinal("idRegExp")) ? reader["idRegExp"].To<int>() : (int?)null;
                input.Siniestralidad.Id = reader["IdSiniestro"].To<int>();
            }

            reader.Close();

        }

        public void InserOrUpdateSiniestroSisa(JuridicoOutput input, int? IdRegExp)
        {
            EnsureConnectionOpen();

            if (input.Siniestralidad.MontoRecuperacion.ToString() == "") input.Siniestralidad.MontoRecuperacion = "0.0";
            if (input.Deducible.MontoPagado.ToString() == "") input.Deducible.MontoPagado = "0.0";
            if (input.DictamenesFavorables.MontoDeducible.ToString() == "") input.DictamenesFavorables.MontoDeducible = "0.0";
            if (input.LiberacionVehiculos.EstimacionDanos.ToString() == "") input.LiberacionVehiculos.EstimacionDanos = "0.0";
            if (input.HomicidiosYLesiones.MontoPagadoHomicidio.ToString() == "") input.HomicidiosYLesiones.MontoPagadoHomicidio = "0.0";
            if (input.HomicidiosYLesiones.MontoPagadoLesiones.ToString() == "") input.HomicidiosYLesiones.MontoPagadoLesiones = "0.0";
            if (input.DictamenesFavorables.EstimadoDaniosAjusteAsegurado.ToString() == "") input.DictamenesFavorables.EstimadoDaniosAjusteAsegurado = "0.0";
            if (input.DictamenesFavorables.EstimadoDaniosAjusteTercero.ToString() == "") input.DictamenesFavorables.EstimadoDaniosAjusteTercero = "0.0";
            int? anioPagado = input.Deducible.AnioPagado != null && input.Deducible.AnioPagado.FechaPago.HasValue ? int.Parse(input.Deducible.AnioPagado.FechaPago.Value.ToString("yyyy")) : (int?)null; 

            var mCommand = CreateCommand("spguardasisa", CommandType.StoredProcedure,
            new SqlParameter("@idSiniestro", input.Siniestralidad.Id),
            new SqlParameter("@idCausa", input.Siniestralidad.IdCausa),
            new SqlParameter("@mesRecuperacion", input.Siniestralidad.MesRecuperacion),
            new SqlParameter("@idTipoRecuperacion", input.Siniestralidad.IdTipoRecuperacion),
            new SqlParameter("@montoRecuperacion", Convert.ToDouble(input.Siniestralidad.MontoRecuperacion)),
            new SqlParameter("@fechaRecuperacion", input.Siniestralidad.FechaRecuperacion),
            new SqlParameter("@numeroSiniestroAtlas", input.Siniestralidad.NumeroSiniestroAtlas),
            new SqlParameter("@anioPagado", anioPagado),
            new SqlParameter("@MontoPagado", Convert.ToDouble(input.Deducible.MontoPagado)),
            new SqlParameter("@idEstatus", input.Deducible.IdEstatus),
            new SqlParameter("@fechaReembolso", input.Deducible.FechaReembolso),
            new SqlParameter("@fechaRecuperacio", input.DictamenesFavorables.FechaRecuperacion),
            new SqlParameter("@montoDeducible", Convert.ToDouble(input.DictamenesFavorables.MontoDeducible)),
            new SqlParameter("@fechaSolicitud", input.DictamenesFavorables.FechaSolicitud),
            new SqlParameter("@fechaPagoDeducible", input.DictamenesFavorables.FechaPagoDeducible),
            new SqlParameter("@idTipoPoliza", input.DictamenesFavorables.IdTipoPoliza),
            new SqlParameter("@oficinaRegional", input.LiberacionVehiculos.OficinaRegional),
            new SqlParameter("@estimacionDaños", Convert.ToDouble(input.LiberacionVehiculos.EstimacionDanos)),
            new SqlParameter("@fechaEmisionCheque", input.HomicidiosYLesiones.FechaEmisionCheque),
            new SqlParameter("@montoPagadoHomicidios", Convert.ToDouble(input.HomicidiosYLesiones.MontoPagadoHomicidio)),
            new SqlParameter("@montoPagadoLecionados", Convert.ToDouble(input.HomicidiosYLesiones.MontoPagadoLesiones)),
            new SqlParameter("@idOficinaRegional", input.VehiculosDetenidos.IdOficinaRegional),
            new SqlParameter("@fechaAcreditaPropiedad", input.VehiculosDetenidos.FechaAcreditaPropiedad),
            new SqlParameter("@estimadoDaniosAjusteAsegurado", Convert.ToDouble(input.DictamenesFavorables.EstimadoDaniosAjusteAsegurado)),
            new SqlParameter("@estimadoDaniosAjusteTercero", Convert.ToDouble(input.DictamenesFavorables.EstimadoDaniosAjusteTercero)),
            new SqlParameter("@numeroReporteProveedorLegal", input.Siniestralidad.NumeroReporteProveedorLegal),
            new SqlParameter("@idRegExp", IdRegExp));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                if (!(reader["Mensaje"].To<int>() == 0 || reader["Mensaje"].To<int>() == 1))
                {
                    if (reader["Mensaje"].To<int>() == 2)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Numero de reporte ya existe");
                    }
                    else if (reader["Mensaje"].To<int>() == 3)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Se necesita Numero de Siniestro");
                    }
                    else if (reader["Mensaje"].To<int>() == 4)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Se necesita Numero de reporte");
                    }
                    else if (reader["Mensaje"].To<int>() == 5)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "El reporte y siniestro no coinciden");
                    }
                }

                IdRegExp = !reader.IsDBNull(reader.GetOrdinal("idRegExp")) ? reader["idRegExp"].To<int>() : (int?)null;
                input.Siniestralidad.Id = reader["IdSiniestro"].To<int>();
            }

            reader.Close();

        }

        public void InsertOrUpdateRecuperacionSisa(RecuperacionesInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spguardaSisaRecuperacion", CommandType.StoredProcedure,
                 new SqlParameter("@numeroReporte", input.NumeroReporte),
                 new SqlParameter("@regional", input.Regional),
                 new SqlParameter("@deducibleEstimado", input.DeducibleEstimado),
                 new SqlParameter("@montoRecuperado", input.MontoRecuperado),
                 new SqlParameter("@honorarioComision", input.HonorarioComision),
                 new SqlParameter("@iva", input.Iva),
                 new SqlParameter("@total", input.Total),
                 new SqlParameter("@fechaRecuperacionR", input.FechaRecuperacionR),
                 new SqlParameter("@deducibleValuacion", input.DeducibleValuacion),
                 new SqlParameter("@folioFactura", input.FolioFactura),
                 new SqlParameter("@numeroSiniestroAtlas", input.NumeroSiniestroAtlas));

            var reader = mCommand.ExecuteReader();

            reader.Close();
        }

        public void InsertOrUpdatePagoTercerosSisa(PagoTercerosInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spguardaSisaPagoTercero", CommandType.StoredProcedure,
                new SqlParameter("@numeroReporte", input.NumeroReporte),
                new SqlParameter("@regional", input.Regional),
                new SqlParameter("@beneficiario", input.Beneficiario),
                new SqlParameter("@montoPago", input.MontoPago),
                new SqlParameter("@fechaComprobacion", input.FechaComprobacion),
                new SqlParameter("@fechaPago", input.FechaPago),
                new SqlParameter("@numeroSiniestroAtlas", input.NumeroSiniestroAtlas));

            var reader = mCommand.ExecuteReader();

            reader.Close();
        }

        public void InsertOrUpdateSiniestroPrometeo(SiniestroPrometeoInput input)
        {
            EnsureConnectionOpen();
            var mCommand = CreateCommand(
                    "spguardaprometeo",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idSiniestro", input.Siniestralidad.Id),
                    new SqlParameter("@idproveedor", input.Siniestralidad.IdProveedor),
                    new SqlParameter("@idmesTurn", input.Siniestralidad.IdMesTurnado),
                    new SqlParameter("@numeroReporteProveedorLegal", input.Siniestralidad.NumeroReporteProveedorLegal),
                    new SqlParameter("@fechaReporte", input.Siniestralidad.FechaReporte),
                    new SqlParameter("@abogado", input.Siniestralidad.Abogado),
                    new SqlParameter("@numeroSiniestroAtlas", input.Siniestralidad.NumeroSiniestroAtlas),
                    new SqlParameter("@fechaSiniestro", input.Siniestralidad.FechaSiniestro),
                    new SqlParameter("@ajustador", input.Siniestralidad.Ajustador),
                    new SqlParameter("@asegurado", input.Siniestralidad.Asegurado),
                    new SqlParameter("@idResponsabilidadAjuste", input.Siniestralidad.IdResponsabilidadAjuste),
                    new SqlParameter("@idEstado", input.Siniestralidad.IdEstado),
                    new SqlParameter("@anioSiniestro", input.PagoTerceros.AnioSiniestro),
                    new SqlParameter("@cobertura", input.PagoTerceros.Cobertura),
                    new SqlParameter("@coberturaTexto", input.PagoTerceros.CoberturaTexto),
                    new SqlParameter("@localidad", input.Deducible.Localidad),
                    new SqlParameter("@fechaSolicitud", input.Deducible.FechaSolicitud),
                    new SqlParameter("@tipoVehiculoAsegurado", input.DictamenesFavorables.TipoVehiculoAsegurado),
                    new SqlParameter("@tipoVehiculoTercero", input.DictamenesFavorables.TipoVehiculoTercero),
                    new SqlParameter("@idSeguroContratadoTer", input.DictamenesFavorables.IdSeguroContratado),
                    new SqlParameter("@idVehiculoAsegurado", input.DictamenesFavorables.IdVehiculoAsegurado),
                    new SqlParameter("@idTipoPersona", input.DictamenesFavorables.IdTipoPersona),
                    new SqlParameter("@idDelito", input.LiberacionVehiculos.IdDelitoInicial),
                    new SqlParameter("@tipoVehiculo", input.LiberacionVehiculos.TipoVehiculo),
                    new SqlParameter("@idSucusalSeguroAtlas", input.Siniestralidad.IdSucursalSegurosAtlas),
                    new SqlParameter("@reservaRCLesiones", input.HomicidiosYLesiones.ReservaRCLesiones),
                    new SqlParameter("@reservaHomicidios", input.HomicidiosYLesiones.ReservaHomicidios),
                    new SqlParameter("@vehiculo", input.VehiculosDetenidos.Vehiculo),
                    new SqlParameter("@fechaLiberacion", input.VehiculosDetenidos.FechaLiberacion),
                    new SqlParameter("@idPagoRecuperacion", input.Siniestralidad.IdPagoRecuperacion),
                    new SqlParameter("@fechaTurnado", input.Siniestralidad.FechaTurnado),
                    new SqlParameter("@idCircunstancia", input.HomicidiosYLesiones.IdCircunstancia),
                    new SqlParameter("@idDeslindeResponsabilidad", input.HomicidiosYLesiones.IdDeslindeResponsabilidad),
                    new SqlParameter("@NumeroReporteProveedoCabina", input.Siniestralidad.NumeroReporteCabina),
                    new SqlParameter("@idRegExp", input.IdRegExp));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                if (!(reader["Mensaje"].To<int>() == 0 || reader["Mensaje"].To<int>() == 1))
                {
                    if (reader["Mensaje"].To<int>() == 2)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Numero de reporte ya existe");
                    }
                    else if (reader["Mensaje"].To<int>() == 3)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Se necesita Numero de Siniestro");
                    }
                    else if (reader["Mensaje"].To<int>() == 4)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "Se necesita Numero de reporte");
                    }
                    else if (reader["Mensaje"].To<int>() == 5)
                    {
                        reader.Close();
                        throw new UserFriendlyException(400, "El reporte y siniestro no coinciden");
                    }
                }

                input.IdRegExp = !reader.IsDBNull(reader.GetOrdinal("idRegExp")) ? reader["idRegExp"].To<int>() : (int?)null;
                input.Siniestralidad.Id = reader["IdSiniestro"].To<int>();
            }
            reader.Close();



        }

        public void GuardaBitacora(string noSiniestroAtlas, string noReporte, string usuario, int idModificacionIntegracion)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                    "spGuardaBitacoraSolicitud",
                    CommandType.StoredProcedure,
                    new SqlParameter("@noSiniestroAtlas", noSiniestroAtlas),
                    new SqlParameter("@noReporte", noReporte),
                    new SqlParameter("@usuario", usuario),
                    new SqlParameter("@idModificacionIntegracion", idModificacionIntegracion),
                    new SqlParameter("@fecha", DateTime.Now)
                    );

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                if (reader["Operacion"].To<int>() != 0)
                {
                    //if(reader["Operacion"].To<int>() == 1)
                    //    throw new UserFriendlyException(400, "El siniestro no existe");
                    //else if (reader["Operacion"].To<int>() == 2)
                    //    throw new UserFriendlyException(400, "El reporte no existe");
                    //else if (reader["Operacion"].To<int>() == 3)
                    //    throw new UserFriendlyException(400, "El siniestro y reporte no coinciden");
                    switch (reader["Operacion"].To<int>())
                    {
                        case 1:
                            reader.Close();
                            throw new UserFriendlyException(400, "El siniestro no existe");
                        case 2:
                            reader.Close();
                            throw new UserFriendlyException(400, "El reporte no existe");
                        case 3:
                            reader.Close();
                            throw new UserFriendlyException(400, "El siniestro y reporte no coinciden");
                    }
                }
            }
            reader.Close();
        }

        public ConsultaBitacoraOutput ConsultaBitacora(ConsultaBitacoraInput input)
        {
            var result = new ConsultaBitacoraOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaBitacoraSolicitud",
                CommandType.StoredProcedure,
                new SqlParameter("@noSiniestroAtlas", input.NoSiniestroAtlas));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                result.Bitacora.Add(new ConsultaBitacoraOutput.ConsultaBitacoraDto
                {
                    Usuario = !reader.IsDBNull(reader.GetOrdinal("Usuario")) ? reader["Usuario"].As<string>() : null,
                    IdSiniestro = !reader.IsDBNull(reader.GetOrdinal("idSiniestro")) ? reader["idSiniestro"].To<int>() : (int?)null,
                    NumeroSiniestro = !reader.IsDBNull(reader.GetOrdinal("NumeroSiniestro")) ? reader["NumeroSiniestro"].As<string>() : null,
                    NumeroReporte = !reader.IsDBNull(reader.GetOrdinal("NumeroReporte")) ? reader["NumeroReporte"].As<string>() : null,
                    Asegurado = !reader.IsDBNull(reader.GetOrdinal("Asegurado")) ? reader["Asegurado"].As<string>() : null,
                    ModificacionIntegracion = !reader.IsDBNull(reader.GetOrdinal("ModificacionIntegracion")) ? reader["ModificacionIntegracion"].As<string>() : null,
                    TiempoEstatus = !reader.IsDBNull(reader.GetOrdinal("TiempoEstatus")) ? reader["TiempoEstatus"].To<TimeSpan>() : (TimeSpan?)null,
                    Consecutivo = !reader.IsDBNull(reader.GetOrdinal("Cconsecutivo")) ? reader["Cconsecutivo"].To<int>() : (int?)null,
                    FechaHoraAcceso = !reader.IsDBNull(reader.GetOrdinal("FechaHoraAcceso")) ? reader["FechaHoraAcceso"].To<DateTime>() : (DateTime?)null
                });
            }
            reader.Close();

            return result;

        }

        public BusquedaSolicitudOutput BusquedaSolicitud(BusquedaSolicitudInput input)
        {

            var result = new BusquedaSolicitudOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                    "spBusquedaSolisitud",
                    CommandType.StoredProcedure,
                    new SqlParameter("@idprovedor", input.IdProveedor),
                    new SqlParameter("@noRreporteProvedor", input.NoReporteProveedor),
                    new SqlParameter("@fechaReporte", input.FechaReporte),
                    new SqlParameter("@fechaReporteFinal", input.FechaReporteFinal),
                    new SqlParameter("@idEstatus", input.IdEstatus),
                    new SqlParameter("@idDelito", input.IdDelito),
                    new SqlParameter("@noSiniestroAtlas", input.NoSiniestroAtlas),
                    new SqlParameter("@idPagoRecuperacion", input.IdPagoRecuperacion));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                result.BusquedaSolicitud.Add(new BusquedaSolicitudDto
                {
                    Proveedor = !reader.IsDBNull(reader.GetOrdinal("Provedor")) ? reader["Provedor"].As<string>().Trim() : null,
                    Nombre = !reader.IsDBNull(reader.GetOrdinal("Nombre")) ? reader["Nombre"].As<string>() : null,
                    Estado = !reader.IsDBNull(reader.GetOrdinal("Estado")) ? reader["Estado"].As<string>() : null,
                    NumeroReporteProveedorLegal = !reader.IsDBNull(reader.GetOrdinal("NumeroReporte")) ? reader["NumeroReporte"].As<string>() : null,
                    SucursalSegurosAtlas = !reader.IsDBNull(reader.GetOrdinal("Sucursal")) ? reader["Sucursal"].As<string>() : null,
                    FechaReporte = !reader.IsDBNull(reader.GetOrdinal("FechaReporte")) ? reader["FechaReporte"].To<DateTime>() : (DateTime?)null,
                    Abogado = !reader.IsDBNull(reader.GetOrdinal("Abogado")) ? reader["Abogado"].As<string>() : null,
                    NumeroSiniestroAtlas = !reader.IsDBNull(reader.GetOrdinal("NumeroSiniestroAtlas")) ? reader["NumeroSiniestroAtlas"].As<string>() : null,
                    FechaSiniestro = !reader.IsDBNull(reader.GetOrdinal("FechaSiniestro")) ? reader["FechaSiniestro"].To<DateTime>() : (DateTime?)null,
                    Ajustador = !reader.IsDBNull(reader.GetOrdinal("Ajustador")) ? reader["Ajustador"].As<string>() : null,
                    Asegurado = !reader.IsDBNull(reader.GetOrdinal("Asegurado")) ? reader["Asegurado"].As<string>() : null,
                    Estatus = !reader.IsDBNull(reader.GetOrdinal("Estatus")) ? reader["Estatus"].As<string>() : null,
                    Delito = !reader.IsDBNull(reader.GetOrdinal("Delito")) ? reader["Delito"].As<string>() : null,
                    Causa = !reader.IsDBNull(reader.GetOrdinal("Causa")) ? reader["Causa"].As<string>() : null,
                    ResponsabilidadAjuste = !reader.IsDBNull(reader.GetOrdinal("ResponsabilidadAjuste")) ? reader["ResponsabilidadAjuste"].As<string>() : null,
                    PagoRecuperacion = !reader.IsDBNull(reader.GetOrdinal("PagoRecuperacion")) ? reader["PagoRecuperacion"].As<string>() : null,
                    FechaAcreditamientoPropiedad = !reader.IsDBNull(reader.GetOrdinal("FechaAcreditamientoPropiedad")) ? reader["FechaAcreditamientoPropiedad"].To<DateTime>() : (DateTime?)null,
                    FechaLiberacionVehiculo = !reader.IsDBNull(reader.GetOrdinal("FechaLiberacionVehiculo")) ? reader["FechaLiberacionVehiculo"].To<DateTime>() : (DateTime?)null,
                    DiasLiberacion = !reader.IsDBNull(reader.GetOrdinal("DiasLiberacion")) ? reader["DiasLiberacion"].To<int>() : (int?)null,
                    ResponsabilidadDictamen = !reader.IsDBNull(reader.GetOrdinal("ResponsabilidadDictamen")) ? reader["ResponsabilidadDictamen"].As<string>() : null,
                    AvaluoOficial = !reader.IsDBNull(reader.GetOrdinal("avaluoOficial")) ? reader["avaluoOficial"].To<decimal>() : (decimal?)null,
                    AutoridadConoce = !reader.IsDBNull(reader.GetOrdinal("AutoridadConoce")) ? reader["AutoridadConoce"].As<string>() : null,
                    EtapaProceso = !reader.IsDBNull(reader.GetOrdinal("EstapaProceso")) ? reader["EstapaProceso"].As<string>() : null,
                    MesRecuperacion = !reader.IsDBNull(reader.GetOrdinal("MesRecuperacion")) ? reader["MesRecuperacion"].As<string>() : null,
                    TipoRecuperacion = !reader.IsDBNull(reader.GetOrdinal("TipoRecuperacion")) ? reader["TipoRecuperacion"].As<string>() : null,
                    Monto = !reader.IsDBNull(reader.GetOrdinal("Monto")) ? reader["Monto"].To<decimal>() : (decimal?)null,
                    FechaRecuperacion = !reader.IsDBNull(reader.GetOrdinal("FechaRecuperacion")) ? reader["FechaRecuperacion"].To<DateTime>() : (DateTime?)null
                });

            }
            reader.Close();
            return result;
        }

        public DetalleSolicitudOutput DetalleSolicitud(DetalleSolicitudInput input)
        {
            var result = new DetalleSolicitudOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spDetalleSolicitud",
                CommandType.StoredProcedure,
                new SqlParameter("@noSiniestroAtlas", input.NoSiniestroAtlas),
                new SqlParameter("@numeroReporte", input.NumeroReporteProveedorLegal));


            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {

                result.DetalleSolicitud.Add(new DetalleSolicitudDto
                {

                    DictamenesFavorables = !reader.IsDBNull(reader.GetOrdinal("DictamenesFaborables")) ? JsonConvert.DeserializeObject<Object>(reader["DictamenesFaborables"].As<string>()) : null,
                    Recuperaciones = !reader.IsDBNull(reader.GetOrdinal("Recuperaciones")) ? JsonConvert.DeserializeObject<Object>(reader["Recuperaciones"].As<string>()) : null,
                    Deducibles = !reader.IsDBNull(reader.GetOrdinal("Deducibles")) ? JsonConvert.DeserializeObject<Object>(reader["Deducibles"].As<string>()) : null,
                    VehiculosDetenidos = !reader.IsDBNull(reader.GetOrdinal("VehiculoDetenidos")) ? JsonConvert.DeserializeObject<Object>(reader["VehiculoDetenidos"].As<string>()) : null,
                    PagoTerceros = !reader.IsDBNull(reader.GetOrdinal("PagoTerceros")) ? JsonConvert.DeserializeObject<Object>(reader["PagoTerceros"].As<string>()) : null,
                    Documentos = !reader.IsDBNull(reader.GetOrdinal("Documentos")) ? JsonConvert.DeserializeObject<Object>(reader["Documentos"].As<string>()) : null
                });
            }

            reader.Close();
            return result;
        }

        public List<Dictionary<string, object>> BusquedaReporte(BusquedaReporteInput input)
        {
            var result = new List<Dictionary<string, object>>();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spBusquedaReporte",
                CommandType.StoredProcedure,
                new SqlParameter("@idproveedor", input.IdProveedor),
                new SqlParameter("@fechaReporte", input.FechaReporteInicio),
                new SqlParameter("@fechaReporteFinal", input.FechaReporteFin),
                new SqlParameter("@idTipoReporte", input.IdTipoReporte));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                var mDic = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var mLlave = reader.GetName(i);
                    var mValor = reader[i];

                    if (input.IdTipoReporte.Equals(3))
                    {
                        mValor = !reader.IsDBNull(i) ? JsonConvert.DeserializeObject<Object>(reader[i].ToString()) : null;
                    }
                    mDic.Add(mLlave, mValor);
                }

                result.Add(mDic);
            }

            reader.Close();

            return result;
        }

        public void GuardarActualizarDocumento(int? idDocumento, int idSiniestro, string nombre, string objectId, string tipoMime, string tipoDocumento, string subTipoDocumento, string usuario, bool borrar)
        {
            EnsureConnectionOpen();

            if (idSiniestro == null)
                throw new UserFriendlyException("El Se requiere el IdSiniestro");

            var mCommand = CreateCommand("spAtlaActualizaDocumentosSiniestro",
                CommandType.StoredProcedure,
                new SqlParameter("@idDocumento", idDocumento),
                new SqlParameter("@idSiniestro", idSiniestro),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@objeto", objectId),
                new SqlParameter("@fechaRegistro", DateTime.Now),
                new SqlParameter("@tipoMime", tipoMime),
                new SqlParameter("@tipoDocumento", tipoDocumento),
                new SqlParameter("@usuarioRegistro", usuario),
                new SqlParameter("@subTipoDocumento", subTipoDocumento),
                new SqlParameter("@borrar", borrar));

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

        public ObtenerDocumentoSiniestroOutput ObtenerDocumentoSiniestro(ObtenerDocumentoSiniestroInput input)
        {
            var result = new ObtenerDocumentoSiniestroOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaDocumentoSinietro",
                CommandType.StoredProcedure,
                new SqlParameter("idDocumento", input.IdDocumento));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                result.IdDocumento = !reader.IsDBNull(reader.GetOrdinal("idDocumento")) ? reader["idDocumento"].To<int>() : 0;
                result.IdSiniestro = !reader.IsDBNull(reader.GetOrdinal("idSiniestro")) ? reader["idSiniestro"].To<int>() : 0;
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

        public void ConsultarIdRegExp(int idSiniestro, out int idRegExp, out string numSiniestro)
        {
            EnsureConnectionOpen();
            string texto = string.Format("SELECT idRegExp, NumeroSiniestroAtlas FROM OpeSiniestros WHERE IdSiniestro = {0}", idSiniestro);
            var mCommand = CreateCommand(texto, CommandType.Text);

            var reader = mCommand.ExecuteReader();
            int r = 0;
            string s = "";
            while (reader.Read())
            {
                r = reader["idRegExp"].To<int>();
                s = reader["NumeroSiniestroAtlas"].As<string>();
            }
            idRegExp = r;
            numSiniestro = s;
            reader.Close();
        }

        public void ObtenerIdRegExp(string numeroSiniestroAtlas, out int idRegExp)
        {
            string texto = string.Format("SELECT idRegExp FROM OpeSiniestros WHERE NumeroSiniestroAtlas = '{0}' AND idRegExp > 0", numeroSiniestroAtlas);

            EnsureConnectionOpen();
            var mCommand = CreateCommand(texto, CommandType.Text);

            var reader = mCommand.ExecuteReader();
            int r = 0;
            while (reader.Read())
            {
                r = !reader.IsDBNull(reader.GetOrdinal("idRegExp")) ? reader["idRegExp"].To<int>() : 0;
            }
            idRegExp = r;
            reader.Close();
        }

        public async Task<DatosNotificacionesOutput> ConsultaDatosNotificacion(DatosNotificacionesInput input)
        {
            var result = new DatosNotificacionesOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spEnviarNotificacion", CommandType.StoredProcedure,
                new SqlParameter("@numeroReporte", input.NumeroReporte),
                new SqlParameter("@notificacion", false));

            var reader = await mCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.NumeroSiniestro = !reader.IsDBNull(reader.GetOrdinal("NumeroSiniestro")) ? reader["NumeroSiniestro"].As<string>() : "";
                result.Entidad = !reader.IsDBNull(reader.GetOrdinal("Entidad")) ? reader["Entidad"].As<string>() : "";
                result.Municipio = !reader.IsDBNull(reader.GetOrdinal("Municipio")) ? reader["Municipio"].As<string>() : "";                
                result.FechaApertura = !reader.IsDBNull(reader.GetOrdinal("FechaApertura")) ? reader["FechaApertura"].To<DateTime>() : DateTime.MinValue;
                result.Abogado = !reader.IsDBNull(reader.GetOrdinal("Abogado")) ? reader["Abogado"].As<string>() : "";
                result.NumeroLesionados = !reader.IsDBNull(reader.GetOrdinal("NumeroLesionados")) ? reader["NumeroLesionados"].To<int>() : 0;
                result.NumeroFallecidos = !reader.IsDBNull(reader.GetOrdinal("NumeroFallecidos")) ? reader["NumeroFallecidos"].To<int>() : 0;
                result.Notificacion = !reader.IsDBNull("Notificacion") ? reader["Notificacion"].To<bool>() : (bool?)null;
                result.NumeroReporte = !reader.IsDBNull(reader.GetOrdinal("NumeroReporte")) ? reader["NumeroReporte"].As<string>() : "";
                result.Asegurado = !reader.IsDBNull(reader.GetOrdinal("Asegurado")) ? reader["Asegurado"].As<string>() : "";
                result.Abogado = !reader.IsDBNull(reader.GetOrdinal("Abogado")) ? reader["Abogado"].As<string>() : "";
                result.Documentos = !reader.IsDBNull("Documentos") ? JsonConvert.DeserializeObject<Object>(reader["Documentos"].As<string>()) : null;
            }

            reader.Close();
            return result;
        }

        public async Task<InformacionAsistenciaNotificacionOutput> InformacionAsistenciaNotificacion(InformacionAsistenciaNotificacionInput input)
        {
            var output = new InformacionAsistenciaNotificacionOutput();

            EnsureConnectionOpen();

            using (var mCommand = CreateCommand("spInformacionAsistenciaNotificacion"
                ,CommandType.StoredProcedure
                ,new SqlParameter("@idSiniestro", input.IdSiniestro)))
            {
                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        output.NumeroSiniestroAtlas = !reader.IsDBNull(reader.GetOrdinal("NumeroSiniestroAtlas")) ? reader["NumeroSiniestroAtlas"].As<string>() : null;
                        output.Entidad = !reader.IsDBNull(reader.GetOrdinal("Entidad")) ? reader["Entidad"].As<string>() : null;
                        output.FechaCreacion = !reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? reader["FechaCreacion"].To<DateTime>() : (DateTime?)null;
                        output.Abogado = !reader.IsDBNull(reader.GetOrdinal("Abogado")) ? reader["Abogado"].As<string>() : null;
                        output.Asegurado = !reader.IsDBNull(reader.GetOrdinal("Asegurado")) ? reader["Asegurado"].As<string>() : null;
                        output.PersonasLesionadas = !reader.IsDBNull(reader.GetOrdinal("PersonasLesionadas")) ? reader["PersonasLesionadas"].To<int>() : 0;
                        output.NumeroFallecidos = !reader.IsDBNull(reader.GetOrdinal("NumeroFallecidos")) ? reader["NumeroFallecidos"].To<int>() : 0;
                        output.NumeroReporte = !reader.IsDBNull(reader.GetOrdinal("NumeroReporte")) ? reader["NumeroReporte"].As<string>() : null;
                        output.IdProveedor = !reader.IsDBNull(reader.GetOrdinal("IdProveedor")) ? reader["IdProveedor"].To<int>() : 0;
                    }
                }
            }

            return output;
        }

        public async Task<DatosVehiculosDetenidosAlertaOutput> ObtenerDatosVehiculosDetenidosAlerta(int idSiniestro)
        {
            var output = new DatosVehiculosDetenidosAlertaOutput();

            EnsureConnectionOpen();

            using (var mCommand = CreateCommand("spDatosVehiculosDetenidosAlerta"
                , CommandType.StoredProcedure
                ,new SqlParameter("@idSiniestro", idSiniestro)))
            {
                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        output.NumeroReporte = !reader.IsDBNull(reader.GetOrdinal("NumeroReporte")) ? reader["NumeroReporte"].As<string>() : null;
                        output.FechaLiberacion = !reader.IsDBNull(reader.GetOrdinal("FechaLiberacion")) ? reader["FechaLiberacion"].To<DateTime>() : (DateTime?)null;
                        output.NumeroSiniestroAtlas = !reader.IsDBNull(reader.GetOrdinal("NumeroSiniestroAtlas")) ? reader["NumeroSiniestroAtlas"].As<string>() : null;
                        output.IdProveedor = !reader.IsDBNull(reader.GetOrdinal("IdProveedor")) ? reader["IdProveedor"].To<int>() : 0;
                        output.DiasVencimiento = !reader.IsDBNull(reader.GetOrdinal("DiasVencimiento")) ? reader["DiasVencimiento"].To<int>() : 0;
                    }
                }
            }

            return output;
        }

        public async Task<string> ObtenerNumeroReporteProveedor(string numeroReporteCabina)
        {
            string output = null;
            string texto = $"SELECT NumeroReporte FROM OpeSiniestros WHERE NumeroReporteProveedoCabina = '{numeroReporteCabina}'";
            EnsureConnectionOpen();

            using (var mCommand = CreateCommand(texto,CommandType.Text))
            {
                using (var reader = await mCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        output = !reader.IsDBNull(reader.GetOrdinal("NumeroReporte")) ? reader["NumeroReporte"].As<string>() : null;
                    }
                }
            }

            return output;
        }

        public async Task GuardaBitacoraEventos(BitacoraEventosInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spBitacoraJson",
                CommandType.StoredProcedure,
                new SqlParameter("@Numeroreporte", input.NumeroReporte),
                new SqlParameter("@jsonentrada", input.JsonEntrada),
                new SqlParameter("@Respuesta", input.Respuesta),
                new SqlParameter("@fecha", input.Fecha),
                new SqlParameter("@idmodulo", input.IdModulo));

            using (var reader = await mCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader["CodigoMensaje"].As<string>() != "1")
                    {
                        var mensaje = reader["Mensaje"].As<string>();
                        reader.Close();
                        throw new UserFriendlyException(400, mensaje);
                    }
                }
            }
        }

    }
}
