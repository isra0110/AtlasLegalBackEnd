using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Atlas.Legal.Autorizacion
{
    public class RespuestaModel<T>
    {
        public bool Exito { get; set; }
        public string MensajeSistema { get; set; }
        public T Resultado { get; set; }
    }

    public class CrearUsuarioInput
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int IdRol { get; set; }
        public int IdTipo { get; set; }
        public string Telefono { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public bool EsInterno { get; set; }
        public bool EstaActivo { get; set; }
    }

    public class CrearUsuarioOutput
    {
        public string Mensaje { get; set; }
        public string Usuario { get; set; }
        public int IdUsuario { get; set; }
    }

    public class LoginInput
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }

    public class LoginOutput
    {
        public string Mensaje { get; set; }
        public string Usuario  { get; set; }
        public string Token { get; set; }        
    }

    public class ValidarAccesoSPInput
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }

    public class ValidarAccesoSPOutput
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Rol { get; set; }
        public bool EsInterno { get; set; }
        public string Usuario { get; set; }
        public string Mensaje { get; set; }
        public int CodigoMensaje { get; set; }


    }

    public class ConsultaUsuarioOutput
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string Tipo { get; set; }
        public bool EsInterno { get; set; }
        public string Usuario { get; set; }
        public object ModulosApp { get; set; }
        
    }

    public class ConsultaUsuarioInput
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }

    public class GuardarRolMenuInput
    {
        public int? IdRol { get; set; }
        public string NombreRol { get; set; }
        public object Menu { get; set; }
        public bool BorrarRol { get; set; }
    }

    public class GuardarRolMenuOutput
    {
        public int IdRol { get; set; }        
        public string Mensaje { get; set; }
    }

    public class ConsultaRolInput
    {
        public int? IdRol { get; set; }
    }

    public class ConsultaRolOutput
    {
        public List<RolDto> Rol { get; set; }

        public ConsultaRolOutput()
        {
            Rol = new List<RolDto>();
        }

        public class RolDto
        {
            public int Id { get; set; }
            public string Rol { get; set; }
            public object Menu { get; set; }
        }        
    }

    public class ConsultaMenuInput
    {
        public int? IdRol { get; set; }
    }

    public class ConsultaMenuOutput
    {
        public List<MenuDto> Menu { get; set; }

        public ConsultaMenuOutput()
        {
            Menu = new List<MenuDto>();
        }

        public class MenuDto
        {
            public object Menu { get; set; }
        }       
        
    }

    public class ActualizarMenuInput
    {
        public int? IdRol { get; set; }
        public int IdMenu { get; set; }
        public string NombreMenu { get; set; }
        public bool? Mostrar { get; set; }
        public bool? Acceso { get; set; }
        public string Ruta { get; set; }
        public int? IdSubMenu { get; set; }
        public string NombreSubMenu { get; set; }
        public bool? MostrarSubMenu { get; set; }
        public bool? AccesoSubMenu { get; set; }
        public string RutaSubMenu { get; set; }
        public string Icono { get; set; }
        public string IconoSubMenu { get; set; }
        public bool Insertar { get; set; }
    }

    public class ActualizarMenuOutput
    {
        public int IdMenu { get; set; }
        public string Mensaje { get; set; }
    }


    public class ConsultaUsuarioRolInput
    {
        public string Usuario { get; set; }
        public int? IdRol { get; set; }
    }

    public class ConsultaUsuarioRolOutput
    {
        public List<UsuarioDto> Usuarios { get; set; }

        public ConsultaUsuarioRolOutput()
        {
            Usuarios = new List<UsuarioDto>();
        }

        public class UsuarioDto
        {
            public int IdUsuario { get; set; }
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
            public string Rol { get; set; }
            public int Tipo { get; set; }
            public bool EsInterno { get; set; }
            public string Usuario { get; set; }
            public object Menu { get; set; }
            public bool EstaActivo { get; set; }            
        }
    }
}

