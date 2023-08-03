using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Atlas.Legal.Autorizacion;
using Atlas.Legal.Comun;
using DotLiquid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Atlas.Legal
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class LegalAppServiceBase : ApplicationService
    {
        public readonly InformacionUsuario InformacionUsuario;                
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<OpeUsuario, int> _opeUsuarioRepository;

        protected LegalAppServiceBase()
        {
            LocalizationSourceName = LegalConsts.LocalizationSourceName;
        }

        protected LegalAppServiceBase(IHttpContextAccessor httpContextAccessor, IRepository<OpeUsuario, int> opeUsuarioRepository)
            //:this()
        {
            string loginName;
            _opeUsuarioRepository = opeUsuarioRepository;
            _httpContextAccessor = httpContextAccessor;

               


                if (!(_httpContextAccessor.HttpContext.Request.Path.Value.Contains("Autorizacion/Login")
                        || _httpContextAccessor.HttpContext.Request.Path.Value.Contains("Autorizacion/CrearActualizarUsuario")
                        ))
            {
                var mIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                loginName = mIdentity.Claims.FirstOrDefault(x => x.Type == "Usuario").Value;

                if (loginName != null)
                {
                    InformacionUsuario = new InformacionUsuario
                    {
                        Usuario = loginName,
                    };

                    var mUsuario = _opeUsuarioRepository.FirstOrDefault(u => u.usuario == loginName && u.activo);

                    if (mUsuario == null)
                        throw new AbpAuthorizationException("El usuario no está registrado, o activo, o no se encuentra autorizado, favor de revisar la información");

                    if (mUsuario != null)
                    {                        
                        InformacionUsuario.IdUsuario = mUsuario.Id;
                        InformacionUsuario.IdRol = mUsuario.idrol;
                    }
                }                
            }
            else
            {
               
                InformacionUsuario = new InformacionUsuario
                {
                    Usuario = "ALEPERPA",
                    IdUsuario = 5,
                    //Nombre = "ALEJANDRO PEREZ PACHECO",
                    //Correo = "appachec@live.com.mx",
                    IdRol = 1
                };
            }
        }        

    }
}