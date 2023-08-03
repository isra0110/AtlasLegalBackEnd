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

namespace Atlas.Legal.Autorizacion
{
    public class AutorizacionRepository : LegalRepositoryBase<OpeUsuario, int>, IAutorizacionRepository
    {
        public AutorizacionRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider, transactionProvider)
        {

        }

        public void CrearUsuario(CrearUsuarioInput input, out string mensaje, out string usuario, out int idUsuario)
        {
            EnsureConnectionOpen();

            var mCommand = CreateCommand("spAltaUsuario",
                CommandType.StoredProcedure,
                new SqlParameter("@nombre", input.Nombre),
                new SqlParameter("@apaterno", input.ApellidoPaterno),
                new SqlParameter("@amaterno", input.ApellidoMaterno),
                new SqlParameter("@idrol", input.IdRol),
                new SqlParameter("@idtipo", input.IdTipo),
                new SqlParameter("@telefono", input.Telefono),
                new SqlParameter("@email", input.Email),
                new SqlParameter("@usuario", input.Usuario),
                new SqlParameter("@contrasenia", input.Password),
                new SqlParameter("@interno", input.EsInterno),
                new SqlParameter("@activo", input.EstaActivo));

            var reader = mCommand.ExecuteReader();
            string msj = "";
            string user = "";
            int mIdUsuario = 0;
            while (reader.Read())
            {
                msj = reader["Mensaje"].As<string>();
                user = reader["Usuario"].As<string>();
                mIdUsuario = reader["idUsuario"].To<int>();

                if (!(reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1))
                {
                    reader.Close();
                    throw new UserFriendlyException(400, msj);
                }
            }
            mensaje = msj;
            usuario = user;
            idUsuario = mIdUsuario;
            reader.Close();

        }

        public ValidarAccesoSPOutput ValidarAccesoSP(ValidarAccesoSPInput input)
        {
            var result = new ValidarAccesoSPOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spLoginOperacion",
                CommandType.StoredProcedure,
                new SqlParameter("@usuario", input.Usuario),
                new SqlParameter("@contrasenia", input.Password));

            var reader = mCommand.ExecuteReader();
            string msj = "";

            while (reader.Read())
            {
                msj = reader["Mensaje"].As<string>();

                if (reader["CodigoMensaje"].To<int>() == 0)
                {
                    result.IdUsuario = !reader.IsDBNull(reader.GetOrdinal("IdUsuario")) ? reader["IdUsuario"].To<int>() : 0;
                    result.Nombre = !reader.IsDBNull(reader.GetOrdinal("Nombre")) ? reader["Nombre"].As<string>() : null;
                    result.ApellidoPaterno = !reader.IsDBNull(reader.GetOrdinal("ApellidoPaterno")) ? reader["ApellidoPaterno"].As<string>() : null;
                    result.ApellidoMaterno = !reader.IsDBNull(reader.GetOrdinal("ApellidoMaterno")) ? reader["ApellidoMaterno"].As<string>() : null;
                    result.Email = !reader.IsDBNull(reader.GetOrdinal("Email")) ? reader["Email"].As<string>() : null;
                    result.Rol = !reader.IsDBNull(reader.GetOrdinal("NombreRol")) ? reader["NombreRol"].As<string>() : null;
                    result.EsInterno = !reader.IsDBNull(reader.GetOrdinal("EsInterno")) ? reader["EsInterno"].To<bool>() : false;
                    result.Usuario = !reader.IsDBNull(reader.GetOrdinal("Usuario")) ? reader["Usuario"].As<string>() : null;
                    result.Mensaje = msj;
                }
                else
                {
                    result = null;
                }
            }
            reader.Close();
            return result;
        }

        public ConsultaUsuarioOutput ConsultaUsuario(string usuario)
        {
            var result = new ConsultaUsuarioOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spLoginOperacionConsulta",
                CommandType.StoredProcedure,
                new SqlParameter("@usuario", usuario));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                if (reader["CodigoMensaje"].To<int>() == 0)
                {
                    result.Id = !reader.IsDBNull(reader.GetOrdinal("IdUsuario")) ? reader["IdUsuario"].To<int>() : 0;
                    result.Nombre = !reader.IsDBNull(reader.GetOrdinal("Nombre")) ? reader["Nombre"].As<string>() : null;
                    result.ApellidoPaterno = !reader.IsDBNull(reader.GetOrdinal("ApellidoPaterno")) ? reader["ApellidoPaterno"].As<string>() : null;
                    result.ApellidoMaterno = !reader.IsDBNull(reader.GetOrdinal("ApellidoMaterno")) ? reader["ApellidoMaterno"].As<string>() : null;
                    result.Email = !reader.IsDBNull(reader.GetOrdinal("Email")) ? reader["Email"].As<string>() : null;
                    result.Rol = !reader.IsDBNull(reader.GetOrdinal("NombreRol")) ? reader["NombreRol"].As<string>() : null;
                    result.EsInterno = !reader.IsDBNull(reader.GetOrdinal("EsInterno")) ? reader["EsInterno"].To<bool>() : false;
                    result.Usuario = !reader.IsDBNull(reader.GetOrdinal("Usuario")) ? reader["Usuario"].As<string>() : null;
                    result.ModulosApp = !reader.IsDBNull(reader.GetOrdinal("ModulosApp")) ? JsonConvert.DeserializeObject<Object>(reader["ModulosApp"].As<string>()) : null;
                }
                else
                {
                    throw new UserFriendlyException(400, "El usuario o la contraseña no son correctos, favor de validar");
                }
            }
            reader.Close();
            return result;

        }

        public void GuardarActualizarRolMenu(GuardarRolMenuInput input, out string mensaje, out int idRol)
        {
            EnsureConnectionOpen();

            string menu = input.Menu != null ? JsonConvert.SerializeObject(input.Menu) : null;

            var mCommand = CreateCommand(
                "spAltaRolMenu",
                CommandType.StoredProcedure,
                new SqlParameter("@idrol", input.IdRol),
                new SqlParameter("@nombreRol", input.NombreRol),
                new SqlParameter("@menu", menu),
                new SqlParameter("@borrarRol", input.BorrarRol));

            var reader = mCommand.ExecuteReader();
            string msj = "";
            int rol = 0;
            while (reader.Read())
            {
                msj = reader["Mensaje"].As<string>();
                
                if (!(reader["CodigoMensaje"].To<int>() == 0 || reader["CodigoMensaje"].To<int>() == 1 || reader["CodigoMensaje"].To<int>() == 3))
                {
                    reader.Close();
                    throw new UserFriendlyException(400, msj);
                }
                rol = reader["Rol"].To<int>();
            }

            mensaje = msj;
            idRol = rol;
            reader.Close();
        }

        public ConsultaRolOutput ConsultaRol(ConsultaRolInput input)
        {
            var result = new ConsultaRolOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaRolP",
                CommandType.StoredProcedure,
                new SqlParameter("@idrol", input.IdRol));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                result.Rol.Add(new ConsultaRolOutput.RolDto
                {
                    Id = !reader.IsDBNull(reader.GetOrdinal("idrol")) ? reader["idrol"].To<int>() : 0,
                    Rol = !reader.IsDBNull(reader.GetOrdinal("rol")) ? reader["rol"].As<string>() : null,
                    Menu = !reader.IsDBNull(reader.GetOrdinal("menu")) ? JsonConvert.DeserializeObject<Object>(reader["menu"].As<string>()) : null,
                });                
            }

            reader.Close();
            return result;
        }

        public ConsultaMenuOutput ConsultaMenu(ConsultaMenuInput input)
        {
            var result = new ConsultaMenuOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand(
                "spConsultaMenu",
                CommandType.StoredProcedure,
                new SqlParameter("@idrol", input.IdRol));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                result.Menu.Add(new ConsultaMenuOutput.MenuDto
                {
                    Menu = !reader.IsDBNull(reader.GetOrdinal("menu")) ? JsonConvert.DeserializeObject<Object>(reader["menu"].As<string>()) : null,
                });
            }
            reader.Close();
            return result;

        }




        public ConsultaUsuarioRolOutput ConsultaUsuarioRol(ConsultaUsuarioRolInput input)
        {
            var result = new ConsultaUsuarioRolOutput();

            EnsureConnectionOpen();

            var mCommand = CreateCommand("spConsultaUsuario",
                CommandType.StoredProcedure,
                new SqlParameter("@usuario", input.Usuario),
                new SqlParameter("@idrol", input.IdRol));

            var reader = mCommand.ExecuteReader();

            while (reader.Read())
            {
                result.Usuarios.Add(new ConsultaUsuarioRolOutput.UsuarioDto
                {
                    IdUsuario = !reader.IsDBNull(reader.GetOrdinal("IdUsuario")) ? reader["IdUsuario"].To<int>() : 0,
                    Nombre = !reader.IsDBNull(reader.GetOrdinal("Nombre")) ? reader["Nombre"].As<string>() : null,
                    ApellidoPaterno = !reader.IsDBNull(reader.GetOrdinal("ApellidoPaterno")) ? reader["ApellidoPaterno"].As<string>() : null,
                    ApellidoMaterno = !reader.IsDBNull(reader.GetOrdinal("ApellidoMaterno")) ? reader["ApellidoMaterno"].As<string>() : null,
                    Telefono = !reader.IsDBNull(reader.GetOrdinal("Telefono")) ? reader["Telefono"].As<string>() : null,
                    Email = !reader.IsDBNull(reader.GetOrdinal("Email")) ? reader["Email"].As<string>() : null,
                    Rol = !reader.IsDBNull(reader.GetOrdinal("NombreRol")) ? reader["NombreRol"].As<string>() : null,
                    Tipo = !reader.IsDBNull(reader.GetOrdinal("Tipo")) ? reader["Tipo"].To<int>() : 0,
                    EsInterno = !reader.IsDBNull(reader.GetOrdinal("EsInterno")) ? reader["EsInterno"].To<bool>() : false,
                    Usuario = !reader.IsDBNull(reader.GetOrdinal("Usuario")) ? reader["Usuario"].As<string>() : null,
                    Menu = !reader.IsDBNull(reader.GetOrdinal("Menu")) ? JsonConvert.DeserializeObject <Object>(reader["Menu"].As<string>()) : null,
                    EstaActivo = !reader.IsDBNull(reader.GetOrdinal("Activo")) ? reader["Activo"].To<bool>() : false
                });
            }
            reader.Close();
            return result;
        }
    }
}
