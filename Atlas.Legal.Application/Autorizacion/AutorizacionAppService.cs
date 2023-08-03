using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Atlas.Legal.Content;
using Atlas.Legal.Autorizacion;
using Atlas.Legal.Acceso;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Atlas.Legal.Configuration;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
namespace Atlas.Legal
{
    public class AutorizacionAppService: LegalAppServiceBase, IAutorizacionAppService
    {
        private readonly IObjectMapper _objectMapper;        
        private readonly IAccesoRepository _accesoRepository;
        private readonly IConfigurationRoot _config;
        private readonly IAutorizacionRepository _autorizacionRepository;

        public AutorizacionAppService(IObjectMapper objectMapper,
            IAutorizacionRepository autorizacionRepository,
            IAccesoRepository accesoRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository,
            IHostingEnvironment env): base(httpContextAccessor, opeUsuarioRepository)
        {
            _objectMapper = objectMapper;            
            _accesoRepository = accesoRepository;
            _autorizacionRepository = autorizacionRepository;
            _config = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public async Task<CrearUsuarioOutput> CrearActualizarUsuario(CrearUsuarioInput input)
        {
            try
            {
                _autorizacionRepository.CrearUsuario(input, out string mensaje, out string usuario, out int idUsuario);

                return new CrearUsuarioOutput
                {
                    Mensaje = mensaje,
                    Usuario = usuario,
                    IdUsuario = idUsuario
                };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
            
            
        }

        public async Task<LoginOutput> Login(LoginInput input)
        {            
            try
            {
                if (!input.Usuario.Equals("ies_prod_user"))
                {
                    var accesoWS = _accesoRepository.ValidarAccesoWS(new ValidarAccesoWSInput
                    {
                        usuario = input.Usuario,
                        password = input.Password,
                        //funcion = 175
                    });

                    if (accesoWS.HttpStatusCode != System.Net.HttpStatusCode.OK)
                        throw new UserFriendlyException("Se ha presentado un error al tratar de logearse en SIISA");
                }

                var accesoSP = _autorizacionRepository.ValidarAccesoSP(new ValidarAccesoSPInput
                {
                    Usuario = input.Usuario,
                    Password = input.Password
                });
                
                var mClaims = new List<Claim>();
                string usuario = null;

                if (accesoSP == null)
                {
                    var mUsuario = await CrearActualizarUsuario(new CrearUsuarioInput
                    {
                        Usuario = input.Usuario,
                        Password = input.Password,
                        IdRol = 2,
                        EstaActivo = true
                    });
                    usuario = mUsuario.Usuario;
                    mClaims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, mUsuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, mUsuario.Usuario),
                    new Claim(ClaimTypes.Role, "Usuario Interno"),
                    new Claim(ClaimTypes.NameIdentifier, true.ToString()),
                    new Claim("Id", mUsuario.IdUsuario.ToString()),
                    new Claim("Usuario", mUsuario.Usuario),
                    new Claim("Rol", "Usuario Interno")
                    };
                }
                else
                {
                    mClaims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, accesoSP.IdUsuario.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, accesoSP.Usuario),
                    new Claim(ClaimTypes.Role, accesoSP.Rol),
                    new Claim(ClaimTypes.NameIdentifier, accesoSP.EsInterno.ToString()),
                    new Claim("Id", accesoSP.IdUsuario.ToString()),
                    new Claim("Usuario", accesoSP.Usuario),
                    new Claim("Rol", accesoSP.Rol)
                    };
                    usuario = accesoSP.Usuario;
                }                
                
                return new LoginOutput
                {
                    Mensaje = "Operación exitosa",
                    Usuario = usuario,
                    Token = GenerarToken(mClaims)
                };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }           

        }

        public ConsultaUsuarioOutput ConsultaUsuario()
        {
            string user = this.InformacionUsuario.Usuario;
            

            var result = _autorizacionRepository.ConsultaUsuario(this.InformacionUsuario.Usuario);
            return result;
        }

        public async Task<GuardarRolMenuOutput> GuardarActualizarRolMenu(GuardarRolMenuInput input)
        {
            _autorizacionRepository.GuardarActualizarRolMenu(input, out string mens, out int id);

            return new GuardarRolMenuOutput
            {
                Mensaje = mens,
                IdRol = id
            };
        }

        public ConsultaRolOutput ConsultaRol(ConsultaRolInput input)
        {
            var result = _autorizacionRepository.ConsultaRol(input);
            return result;
        }

        public ConsultaMenuOutput ConsultaMenu(ConsultaMenuInput input)
        {
            var result = _autorizacionRepository.ConsultaMenu(input);

            return result;
        }

        public ConsultaUsuarioRolOutput ConsultaUsuarioRol(ConsultaUsuarioRolInput input)
        {
            var result = _autorizacionRepository.ConsultaUsuarioRol(input);

            return result;
        }

        private string GenerarToken(List<Claim> claims)
        {
            var key = _config["Jwt:Key"];

            var mKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var mCreds = new SigningCredentials(mKey, SecurityAlgorithms.HmacSha256);

            var mToken = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires: DateTime.Now.AddDays(7),
                signingCredentials: mCreds,
                claims: claims);
            Debug.WriteLine("TOKEN====>");
            Debug.WriteLine(mToken);
            Debug.WriteLine(mCreds);
            
            return new JwtSecurityTokenHandler().WriteToken(mToken);
        }
        
    }
}
