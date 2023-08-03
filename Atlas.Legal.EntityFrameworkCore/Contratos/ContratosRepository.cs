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
using static Atlas.Legal.ContratosInputOutput;

namespace Atlas.Legal.Contratos
{
    public class ContratosRepository : LegalRepositoryBase<Entity, int>, IContratosRepository
    {
        public ContratosRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider, transactionProvider)
        {

        }

        /*
         * MAHV 31/05/2022
         * WS para el registro de un nuevo contrato
         */
        public async Task<RegistroNuevoContratoOutput> RegistroNuevaSolicitud(RegistroNuevoContratoInput input)
        {
            try
            {
                var result = new RegistroNuevoContratoOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratoRegistro", CommandType.StoredProcedure,
                      new SqlParameter("@numeroContrato", input.NumeroContrato),
                        new SqlParameter("@nuevaSolicitud", input.NuevaSolicitud),
                        new SqlParameter("@perteneceContrato", input.PerteneceContrato),
                        new SqlParameter("@correoSolicitante", input.CorreoSolicitante),
                        new SqlParameter("@numeroExtencion", input.NoExtension),
                        new SqlParameter("@agenteSeguros", input.AgenteSeguros),
                        new SqlParameter("@nombreAgente", input.NombreAgente),
                        new SqlParameter("@claveAgente", input.ClaveAgente),
                        new SqlParameter("@idEstatus", input.IdEstatus),
                        new SqlParameter("@rfc", input.Rfc),
                        new SqlParameter("@calle", input.Calle),
                        new SqlParameter("@noExterior", input.NoExterior),
                        new SqlParameter("@noInterior", input.NoInterior),
                        new SqlParameter("@cp", input.CodigoPostal),
                        new SqlParameter("@colonia", input.Colonia),
                        new SqlParameter("@municipio", input.Municipio),
                        new SqlParameter("@estado", input.Estado),
                        new SqlParameter("@poblacion", input.Poblacion),
                        new SqlParameter("@tipoCedula", input.TipoCedula),
                        new SqlParameter("@cedula", input.Cedula),
                        new SqlParameter("@fechaVtoCedula", input.FechaVtoCedula),
                        new SqlParameter("@claveAgenteB", input.ClaveAgenteB),
                        new SqlParameter("@nombreAgenteB", input.NombreAgenteB),
                        new SqlParameter("@idTipoPersona", input.IdTipoPersona),
                        new SqlParameter("@primerNombre", input.PrimerNombre),
                        new SqlParameter("@SegundoNombre", input.SegundoNombre),
                        new SqlParameter("@aPaterno", input.ApellidoPaterno),
                        new SqlParameter("@aMaterno", input.ApellidoMaterno),
                        new SqlParameter("@razonSocial", input.RazonSocial),
                        new SqlParameter("@representante", input.Representante),
                        new SqlParameter("@rfcP", input.RfcPersona),
                        new SqlParameter("@curp", input.Curp),
                        new SqlParameter("@idPais", input.IdPais),
                        new SqlParameter("@idEntidadFederativa", input.IdEntidadFederativa),
                        new SqlParameter("@municipioP", input.MunicipioPersona),
                        new SqlParameter("@coloniaP", input.ColoniaPersona),
                        new SqlParameter("@calleP", input.Calle),
                        new SqlParameter("@calleNumero", input.CalleNumero),
                        new SqlParameter("@cpP", input.CodigoPostalPersona),
                        new SqlParameter("@telefono", input.Telefono),
                        new SqlParameter("@correo", input.CorreoElectronico),
                        new SqlParameter("@descripcion", input.Descripcion),
                        new SqlParameter("@idArea", input.IdArea),
                        new SqlParameter("@idRegExp", input.IdRegExp),
                        new SqlParameter("@fechaCreacion", input.FechaCreacion));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string msj = reader["Mensaje"].As<string>();
                    if ((reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                    {
                        result.Mensaje = "Se guardo el registro con el No. de Folio: " + reader["FolioContrato"];
                        result.IdContrato = !reader.IsDBNull("IdContrato") ? reader["IdContrato"].To<int>() : 0;
                        result.IdRegExp = !reader.IsDBNull("IdRegExp") ? reader["IdRegExp"].To<int>() : 0;
                        result.FolioContrato = !reader.IsDBNull("FolioContrato") ? reader["FolioContrato"].As<string>() : null;
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
         * MAHV 02/06/2022
         * WS para consultar contrato
         */
        public async Task<ConsultaContratoOutput> ConsultaContrato(ConsultaContratoInput input)
        {
            try
            {
                var result = new ConsultaContratoOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratosConsulta", CommandType.StoredProcedure,
                    new SqlParameter("@numeroContrato",input.NoContrato),
                    new SqlParameter("@rfc", input.Rfc),
                    new SqlParameter("@razonSocial", input.RazonSocial),
                    new SqlParameter("@claveAgente", input.ClaveAgente));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.Contrato.Add(new ConsultaContratoOutput.ContratoDTO 
                    {
                        NoContrato = !reader.IsDBNull("numeroContrato") ? reader["numeroContrato"].As<string>() : null,
                        Rfc = !reader.IsDBNull("rfc") ? reader["rfc"].As<string>() : null,
                        RazonSocial = !reader.IsDBNull("razonSocial") ? reader["razonSocial"].As<string>() : null,
                        ClaveAgente = !reader.IsDBNull("claveAgente") ? reader["claveAgente"].As<string>() : null,
                        Agente = !reader.IsDBNull("nombreAgente") ? reader["nombreAgente"].As<string>() : null,
                        Area = !reader.IsDBNull("area") ? reader["area"].As<string>() : null,
                        TipoContrato = !reader.IsDBNull("tipoContrato") ? reader["tipoContrato"].As<string>() : null,
                        FechaCreacion = !reader.IsDBNull("fechaCreacion") ? reader["fechaCreacion"].To<DateTime>() : (DateTime?)null,
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
         * MAHV 02/06/2022
         * WS para el guardado de documentos en la BD de Atlas-Legal
        */
        public void GuardarDocumentoContrato(GuardarDocumentoContratosInput input)
        {
            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratosRegistraDocumentos", CommandType.StoredProcedure,
                new SqlParameter("@idDocumento", input.IdDocumento),
                new SqlParameter("@objeto", input.ObjectId),
                new SqlParameter("@nombre", input.Nombre),
                new SqlParameter("@tipoMime", input.TipoMime),
                new SqlParameter("@tipoDocumento", input.TipoDocumento),
                new SqlParameter("@subtipoDocumento", input.SubTipoDocumento),
                new SqlParameter("@idContrato", input.IdContrato),
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
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
         * MAHV 02/06/2022
         * WS para obtener el id del documento
         */
        public string ObtenerObjectIdContrato(ObtenerObjectIdContratoInput input)
        {
            string texto = $"SELECT objeto FROM OpeContratosDocumentos WHERE idDocumento = {input.IdDocumento}";
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
         * MAHV 02/06/2022
         * WS para obtener el Id registro expediente del contrato
         */
        public ObtenerIdRegExpContratoOutput ObtenerIdRegExpContrato(int IdContrato)
        {
            var output = new ObtenerIdRegExpContratoOutput();
            string texto = $"SELECT idRegExp, numeroContrato FROM OpeContratos WHERE id = {IdContrato}";

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);
            using (var reader = mCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    output.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                    output.NumeroContrato = !reader.IsDBNull("numeroContrato") ? reader["numeroContrato"].As<string>() : null;
                }
            }
            return output;
        }

        /*
         * MAHV 03/06/2022
         * WS para guardar los archivos que se deben cargar de acuerdo al tipo de contrato
         */
        public void GuardarListaArchivosContrato(RegistroNuevoContratoInput input)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spContratoRegistroDescripcion", CommandType.StoredProcedure,
                new SqlParameter("@numeroContrato", input.NumeroContrato),
                new SqlParameter("@idTipoContrato", input.IdTipoContrato),
                new SqlParameter("@idEspecifiqueCualPrestador", input.IdEspecifiqueCualPrestador),
                new SqlParameter("@defineOtros", input.DefineOtros),
                new SqlParameter("@descripcionOtros", input.DescripcionOtros),
                new SqlParameter("@actaConstitutiva", input.ActaConstitutiva),
                new SqlParameter("@cartaPoder", input.CartaPoder),
                new SqlParameter("@comprobanteDomicilio", input.ComprobanteDomicilio),
                new SqlParameter("@constanciaFiscal", input.ConstanciaFiscal),
                new SqlParameter("@cotizacionServicio", input.CotizacionServicios),
                new SqlParameter("@curriculumEmpresarial", input.CurriculumEmpresarial),
                new SqlParameter("@cuentaBancario", input.CuentaBancario),
                new SqlParameter("@fichaTecnica", input.FichaTecnica),
                new SqlParameter("@identificacion", input.Identificacion),
                new SqlParameter("@identificacionFuncionario", input.IdentificacionFuncionario),
                new SqlParameter("@identificacionRepresentante", input.IdentificacionRepresentante),
                new SqlParameter("@oficioContestar", input.OficioContestar),
                new SqlParameter("@poderNombramiento", input.PoderNombramiento),
                new SqlParameter("@cionServicio", input.CotizacionServicios),
                new SqlParameter("@solicitud", input.Solicitud),
                new SqlParameter("@fichaTecnicaContratos", input.FichaTecnicaContratos),
                new SqlParameter("@expedienteInterno", input.ExpedienteInterno),
                new SqlParameter("@escriturasInmueble", input.EscriturasInmueble),
                new SqlParameter("@usoSuelo", input.UsoSuelo),
                new SqlParameter("@planos", input.Planos),
                new SqlParameter("@perdidaInmueble", input.PredialInmueble),
                new SqlParameter("@subarrendamiento", input.Subarrendamiento),
                new SqlParameter("@contratoArrendamiento", input.ContratoArrendamiento),
                new SqlParameter("@documentoAnalisis", input.DocumentoAnalisis),
                new SqlParameter("@caratulaPoliza", input.CaratulaPoliza),
                new SqlParameter("@propuestaComercial", input.PropuestaComercial),
                new SqlParameter("@flujoOperaciones", input.FlujoOperacion),
                new SqlParameter("@licitacion", input.Licitacion),
                new SqlParameter("@juntasAclaraciones", input.JuntasAclaraciones),
                new SqlParameter("@modeloContrato", input.ModeloContrato),
                new SqlParameter("@docRelacionados",input.DocumentoRelacionados),
                new SqlParameter("@cv", input.Cv),
                new SqlParameter("@propuestaServicio", input.PropuestaServicios),
                new SqlParameter("@planContingencia", input.PlanContingencia),
                new SqlParameter("@siniestrosGastosMedicos", input.SiniestrosGastosMedicos),
                new SqlParameter("@licenciaSanitaria", input.LicenciaSanitaria),
                new SqlParameter("@avisoFuncionamiento", input.AvisoFuncionamiento),
                new SqlParameter("@otros", input.Otros));

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
         * MAHV 03/06/2022
         * WS para consultar sollicitante
         */
        public async Task<ConsultaSolicitanteOutput> ConsultaSolicitante(ConsultaSolicitanteInput input)
        {
            try
            {
                var result = new ConsultaSolicitanteOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratosConsultaSolisitante", CommandType.StoredProcedure,
                    new SqlParameter("@numeroContrato", input.NoContrato),
                    new SqlParameter("@idArea", input.IdArea),
                    new SqlParameter("@idEstatus", input.IdEstatus),
                    new SqlParameter("@FechaDesde", input.FechaRegistroInicio),
                    new SqlParameter("@FechaHasta", input.FechaRegistroFin));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.Solicitante.Add(new ConsultaSolicitanteOutput.SolicitanteDTO
                    {
                        NoContrato = !reader.IsDBNull("numeroContrato") ? reader["numeroContrato"].As<string>() : null,
                        Solicitante = !reader.IsDBNull("Solicitante") ? reader["Solicitante"].As<string>() : null,
                        Area = !reader.IsDBNull("Area") ? reader["Area"].As<string>() : null,
                        Estatus = !reader.IsDBNull("Estatus") ? reader["Estatus"].As<string>() : null,
                        DetalleEstatus = !reader.IsDBNull("DetalleEstatus") ? reader["DetalleEstatus"].As<string>() : null,                       
                        FechaSolicitud = !reader.IsDBNull("FechaSolicitud") ? reader["FechaSolicitud"].To<DateTime>() : (DateTime?)null,
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
         * MAHV 03/06/2022
         * WS para consultar abogado
         */
        public async Task<ConsultaAbogadoOutput> ConsultaAbogado(ConsultaAbogadoInput input)
        {
            try
            {
                var result = new ConsultaAbogadoOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratosConsultaAbogado", CommandType.StoredProcedure,
                    new SqlParameter("@numeroContrato", input.NoContrato),
                    new SqlParameter("@idArea", input.IdArea),
                    new SqlParameter("@idEstatus", input.IdEstatus),
                    new SqlParameter("@FechaDesde", input.FechaRegistroInicio),
                    new SqlParameter("@FechaHasta", input.FechaRegistroFin));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.Abogado.Add(new ConsultaAbogadoOutput.AbogadoDTO
                    {
                        NoContrato = !reader.IsDBNull("numeroContrato") ? reader["numeroContrato"].As<string>() : null,
                        Solicitante = !reader.IsDBNull("Solicitante") ? reader["Solicitante"].As<string>() : null,
                        Area = !reader.IsDBNull("Area") ? reader["Area"].As<string>() : null,
                        Estatus = !reader.IsDBNull("Estatus") ? reader["Estatus"].As<string>() : null,
                        DetalleEstatus = !reader.IsDBNull("DetalleEstatus") ? reader["DetalleEstatus"].As<string>() : null,
                        FechaSolicitud = !reader.IsDBNull("FechaSolicitud") ? reader["FechaSolicitud"].To<DateTime>() : (DateTime?)null,
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
         * MAHV 03/06/2022
         * WS para obtener el detalle
         */
        public async Task<DetalleContratoOutput> DetalleSolicitud(DetalleContratoInput input)
        {
           
                var result = new DetalleContratoOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratosDetalle", CommandType.StoredProcedure,
                    new SqlParameter("@numeroContratos", input.NoFolio));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.NumeroContrato = !reader.IsDBNull("numeroContrato") ? reader["numeroContrato"].As<string>() : null;
                    result.NuevaSolicitud = !reader.IsDBNull("nuevaSolicitud") ? reader["nuevaSolicitud"].To<bool>() : (bool?)null;
                    result.PerteneceContrato = !reader.IsDBNull("perteneceContrato") ? reader["perteneceContrato"].To<bool>() : (bool?)null;
                    result.CorreoSolicitante = !reader.IsDBNull("correoSolicitante") ? reader["correoSolicitante"].As<string>() : null;
                    result.NumeroExtension = !reader.IsDBNull("numeroExtencion") ? reader["numeroExtencion"].As<string>() : null;
                    result.AgenteSeguros = !reader.IsDBNull("agenteSeguros") ? reader["agenteSeguros"].To<bool>() : (bool?)null;
                    result.NombreAgente = !reader.IsDBNull("nombreAgente") ? reader["nombreAgente"].As<string>() : null;
                    result.ClaveAgente = !reader.IsDBNull("claveAgente") ? reader["claveAgente"].As<string>() : null;
                    result.Rfc = !reader.IsDBNull("rfc") ? reader["rfc"].As<string>() : null;
                    result.Calle = !reader.IsDBNull("calle") ? reader["calle"].As<string>() : null;
                    result.NoExterior = !reader.IsDBNull("noExterior") ? reader["noExterior"].As<string>() : null;
                    result.NoInterior = !reader.IsDBNull("noInterior") ? reader["noInterior"].As<string>() : null;
                    result.Colonia = !reader.IsDBNull("colonia") ? reader["colonia"].As<string>() : null;
                    result.Municipio = !reader.IsDBNull("municipio") ? reader["municipio"].As<string>() : null;
                    result.Estado = !reader.IsDBNull("estado") ? reader["estado"].As<string>() : null;
                    result.Poblacion = !reader.IsDBNull("poblacion") ? reader["poblacion"].As<string>() : null;
                    result.IdRegExp = !reader.IsDBNull("idRegExp") ? reader["idRegExp"].To<int>() : (int?)null;
                    result.TipoCedula = !reader.IsDBNull("tipoCedula") ? reader["tipoCedula"].As<string>() : null;
                    result.Cedula = !reader.IsDBNull("cedula") ? reader["cedula"].As<string>() : null;
                    result.FechaVtoCedula = !reader.IsDBNull("fechaVtoCedula") ? reader["fechaVtoCedula"].To<DateTime>() : (DateTime?)null;
                    result.ClaveAgenteB = !reader.IsDBNull("claveAgenteB") ? reader["claveAgenteB"].As<string>() : null;
                    result.NombreAgenteB = !reader.IsDBNull("NombreAgenteB") ? reader["NombreAgenteB"].As<string>() : null;
                    result.Cp = !reader.IsDBNull("cp") ? reader["cp"].As<string>() : null;
                    result.FechaCreacion = !reader.IsDBNull("fechaCreacion") ? reader["fechaCreacion"].To<DateTime>() : (DateTime?)null;
                    result.IdEstatus = !reader.IsDBNull("IdEstatus") ? reader["IdEstatus"].To<int>() : (int?)null;
                    result.IdArea = !reader.IsDBNull("IdArea") ? reader["IdArea"].To<int>() : (int?)null;
                    result.DefinicionContratos = !reader.IsDBNull("DefinicionContratos") ? JsonConvert.DeserializeObject<Object>(reader["DefinicionContratos"].As<string>()) : null;
                    result.Documentos = !reader.IsDBNull("Documentos") ? JsonConvert.DeserializeObject<Object>(reader["Documentos"].As<string>()) : null;
                    result.Persona = !reader.IsDBNull("Persona") ? JsonConvert.DeserializeObject<Object>(reader["Persona"].As<string>()) : null;
                }

                reader.Close();
                return result;            
        }

        /*
         * MAHV 06/06/2022
         * WS para obtener los comentarios
         */
        public async Task<ComentarioContratoOutput> ObtenerComentarioContrato(ComentarioContratoInput input)
        {
               var result = new ComentarioContratoOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratoConsultaComentario", CommandType.StoredProcedure,
                    new SqlParameter("@numeroContrato", input.NoFolio));

                var reader = await mCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.IdDemandaComentario = !reader.IsDBNull("idDemandaComentario") ? reader["idDemandaComentario"].To<int>() : (int?)null;
                    result.NumeroContrato = !reader.IsDBNull("numeroContrato") ? reader["numeroContrato"].As<string>() : null;
                    result.FechaRegistro = !reader.IsDBNull("fechaRegistro") ? reader["fechaRegistro"].To<DateTime>() : (DateTime?)null;
                    result.UsuarioRegistro = !reader.IsDBNull("usuarioRegistro") ? reader["usuarioRegistro"].As<string>() : null;
                    result.Comentario = !reader.IsDBNull("Comentario") ? reader["Comentario"].As<string>() : null;                    
                }

                reader.Close();
                return result;           
        }

        /*
         * MAHV 06/06/2022
         * WS para guardar el comentario
         */
        public void GuardarComentarioContrato(GuardarComentarioContratoInput input)
        {
            
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratoRegistrarComentario", CommandType.StoredProcedure,
                new SqlParameter("@numeroContrato", input.NoFolio),
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

        /*
         * MAHV 06/06/2022
         * WS para guardar los comentarios
         */
        public void GuardarTrackingContrato(GuardarTrackingContratoInput input)
        {
            try
            {
                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratoRegistroTracking", CommandType.StoredProcedure,
                new SqlParameter("@numeroContrato", input.NoFolio),
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
         * MAHV 06/06/2022
         * WS para obtener el tracking
         */
        public async Task<ObtenerTrackingContratoOutput> ObtenerTrackingContrato(ObtenerTrackingContratoInput input)
        {
            try
            {
                var result = new ObtenerTrackingContratoOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratoConsultaTracking", CommandType.StoredProcedure,
                    new SqlParameter("@numeroContrato", input.NoFolio));

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
                        result.Trackings.Add(new ObtenerTrackingContratoOutput.TrackingDto
                        {
                            IdDemandaTracking = !reader.IsDBNull("idDemandaTracking") ? reader["idDemandaTracking"].To<int>() : (int?)null,
                            NumeroContrato = !reader.IsDBNull("numeroContrato") ? reader["numeroContrato"].As<string>() : null,
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
         * 
         * 
         */
        public async Task<ReporteGeneralContratoOutput> ReporteGeneralContrato(ReporteGeneralContratoInput input)
        {
            try
            {
                var result = new ReporteGeneralContratoOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratosReporteGeneral", CommandType.StoredProcedure,
                    new SqlParameter("@numeroContrato", input.NoFolio),
                    new SqlParameter("@idArea", input.IdArea),
                    new SqlParameter("@idEstatus", input.IdEstatus),
                    new SqlParameter("@tipoRreporte", input.IdTipoReporte),
                    new SqlParameter("@idTipoPersona", input.IdPersona),
                    new SqlParameter("@rfc", input.Rfc),
                    new SqlParameter("@fechaDesde", input.FechaRegistroInicio),
                    new SqlParameter("@fechaHasta", input.FechaRegistroFin));

                var reader = await mCommand.ExecuteReaderAsync();

                while (reader.Read())
                {
                    result.Reportes.Add(new ReporteGeneralContratoOutput.ReporteGeneralDto
                    {
                        ExpedienteProvieneDe = !reader.IsDBNull("expedienteProvieneDe") ? reader["expedienteProvieneDe"].To<int>() : (int?)null,
                        Estatus = !reader.IsDBNull("Estatus") ? reader["Estatus"].As<string>() : null,
                        DetalleEstatus = !reader.IsDBNull("DetalleEstatus") ? reader["DetalleEstatus"].As<string>() : null,
                        Solicitante = !reader.IsDBNull("Solicitante") ? reader["Solicitante"].As<string>() : null,
                        CorreoSolicitante = !reader.IsDBNull("correoSolicitante") ? reader["correoSolicitante"].As<string>() : null,
                        NumeroExtencion = !reader.IsDBNull("NumeroExtencion") ? reader["NumeroExtencion"].As<string>() : null,
                        Area = !reader.IsDBNull("Area") ? reader["Area"].As<string>() : null,
                        NombreAgente = !reader.IsDBNull("nombreAgente") ? reader["nombreAgente"].As<string>() : null,
                        ClaveAgente = !reader.IsDBNull("claveAgente") ? reader["claveAgente"].As<string>() : null,
                        TipoPersona = !reader.IsDBNull("TipoPersona") ? reader["TipoPersona"].As<string>() : null,
                        Rfc = !reader.IsDBNull("rfc") ? reader["rfc"].As<string>() : null,
                        TipoContrato = !reader.IsDBNull("TipoContrato") ? reader["TipoContrato"].As<string>() : null,
                        EspecifiqueCual = !reader.IsDBNull("EspecifiqueCual") ? reader["EspecifiqueCual"].As<string>() : null,
                        RevisionProveedorEsteno = !reader.IsDBNull("RevicionProveedorEsteno") ? reader["RevicionProveedorEsteno"].As<string>() : null,
                        Descripcion = !reader.IsDBNull("Descripcion") ? reader["Descripcion"].As<string>() : null
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
         * 
         * 
         */
        public async Task<ReportePrestadoresOutput> ReportePrestadores(ReportePrestadoresInput input)
        {
            try
            {
                var result = new ReportePrestadoresOutput();

                EnsureConnectionOpen();

                var mCommand = CreateCommand("spContratosReportePrestadores", CommandType.StoredProcedure,
                    new SqlParameter("@numeroContrato", input.NoFolio),
                    new SqlParameter("@idArea", input.IdArea),
                    new SqlParameter("@idEstatus", input.IdEstatus),
                    new SqlParameter("@tipoRreporte", input.IdTipoReporte),
                    new SqlParameter("@idTipoPersona", input.IdPersona),
                    new SqlParameter("@rfc", input.Rfc),
                    new SqlParameter("@fechaDesde", input.FechaRegistroInicio),
                    new SqlParameter("@fechaHasta", input.FechaRegistroFin));

                var reader = await mCommand.ExecuteReaderAsync();

                while (reader.Read())
                {
                    result.Prestadores.Add(new ReportePrestadoresOutput.ReportePrestadoresDto
                    {
                        TipoServiciosTerceros = !reader.IsDBNull("tipoServiciosTerceros") ? reader["tipoServiciosTerceros"].As<string>() : null,
                        FechaAcuerdo = !reader.IsDBNull("fechaAcuerdo") ? reader["fechaAcuerdo"].To<DateTime>() : (DateTime?)null,
                        FechaInicio = !reader.IsDBNull("fechaInicio") ? reader["fechaInicio"].To<DateTime>() : (DateTime?)null,
                        FechaBaja = !reader.IsDBNull("fechaBaja") ? reader["fechaBaja"].To<DateTime>() : (DateTime?)null,
                        DescripcionOtrosServiciosTerceros = !reader.IsDBNull("descripcionOtrosServiciosTerceros") ? reader["descripcionOtrosServiciosTerceros"].As<string>() : null,
                        OficioAutoridadOtrosServiciosTerceros = !reader.IsDBNull("oficioAutoridadOtrosServiciosTerceros") ? reader["oficioAutoridadOtrosServiciosTerceros"].As<string>() : null,
                        FechaAutoridadOtrosServiciosTerceros = !reader.IsDBNull("fechaAutoridadOtrosServiciosTerceros") ? reader["fechaAutoridadOtrosServiciosTerceros"].To<DateTime>() : (DateTime?)null,
                        NumeroContrato = !reader.IsDBNull("numeroContrato") ? reader["numeroContrato"].As<string>() : null,
                        TipoPersona = !reader.IsDBNull("tipoPersona") ? reader["tipoPersona"].As<string>() : null,
                        PrimerNombre = !reader.IsDBNull("primerNombre") ? reader["primerNombre"].As<string>() : null,
                        SegundoNombre = !reader.IsDBNull("SegundoNombre") ? reader["SegundoNombre"].As<string>() : null,
                        APaterno = !reader.IsDBNull("aPaterno") ? reader["aPaterno"].As<string>() : null,
                        AMaterno = !reader.IsDBNull("aMaterno") ? reader["aMaterno"].As<string>() : null,
                        Curp = !reader.IsDBNull("curp") ? reader["curp"].As<string>() : null,
                        RazonSocial = !reader.IsDBNull("razonSocial") ? reader["razonSocial"].As<string>() : null,
                        Calle = !reader.IsDBNull("calle") ? reader["calle"].As<string>() : null,
                        Pais = !reader.IsDBNull("pais") ? reader["pais"].As<string>() : null,
                        CalleNumero = !reader.IsDBNull("calleNumero") ? reader["calleNumero"].As<string>() : null,
                        Colonia = !reader.IsDBNull("colonia") ? reader["colonia"].As<string>() : null,
                        EntidadFederativa = !reader.IsDBNull("entidadFederativa") ? reader["entidadFederativa"].As<string>() : null,
                        Municipio = !reader.IsDBNull("municipio") ? reader["municipio"].As<string>() : null,
                        Cp = !reader.IsDBNull("cp") ? reader["cp"].As<string>() : null,
                        Telefono = !reader.IsDBNull("telefono") ? reader["telefono"].As<string>() : null,
                        Correo = !reader.IsDBNull("correo") ? reader["correo"].As<string>() : null
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
    }
}
