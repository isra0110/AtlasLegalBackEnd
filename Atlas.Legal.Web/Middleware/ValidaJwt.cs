using Atlas.Legal.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Abp.Authorization;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace Atlas.Legal.Web.Middleware
{
    public class ValidaJwt
    {
        private readonly RequestDelegate _next;
        private readonly IConfigurationRoot _config;

        public ValidaJwt(RequestDelegate next,
            IWebHostEnvironment env)
        {
            _next = next;
            _config = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public async Task Invoke(HttpContext context)
        {            
             string loginName = "";
            
            if ((context.Request.ContentLength > 0 && context.Request.ContentLength != null) || context.Request.Path.Value.Length > 4)
            {
                try
                {

                    
                    if (context.Request.Path.Value.Contains("Autorizacion/Login") 
                        || context.Request.Path.Value.Contains("Autorizacion/CrearActualizarUsuario"))
                        await _next.Invoke(context);
                    else
                    {

                        
                        string token = "";
                        if (!TryRetrieveToken(context, out token))
                            throw new AbpAuthorizationException("Usuario no autorizado");
                        Debug.WriteLine(string.Concat("DateTime.Now: ", DateTime.Now.TimeOfDay));
                        Debug.WriteLine(string.Concat("DateTime.UtcNow: ", DateTime.UtcNow.TimeOfDay));
                        Debug.WriteLine(string.Empty);

                        string tziString = TimeZoneInfo.Local.Id;

                        Debug.WriteLine(string.Concat(tziString, ": ",
                            TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(tziString)).TimeOfDay));

                        Debug.WriteLine(string.Concat("UTC Offset: ", TimeZoneInfo.Local.GetUtcOffset(DateTimeOffset.Now)));
                        var now = DateTime.UtcNow;
                        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(_config["Jwt:Key"]));                        

                        SecurityToken securityToken;
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                        TokenValidationParameters validationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = false,
                            LifetimeValidator = this.LifeTimeValidator,
                            //SignatureValidator = this.SignatureValidator,
                            IssuerSigningKey = securityKey
                        };

                        //Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                        context.User = handler.ValidateToken(token, validationParameters, out securityToken);

                        var mIdentity = (System.Security.Claims.ClaimsIdentity)context.User.Identity;
                        //loginName = mIdentity.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                        loginName = mIdentity.Claims.FirstOrDefault(x => x.Type == "Usuario").Value;


                        await _next.Invoke(context);
                    }                    
                }
                catch (AbpAuthorizationException ex)
                {
                    throw new AbpAuthorizationException(ex.Message);
                }
                catch (SecurityTokenValidationException ex)
                {
                    throw new SecurityTokenValidationException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                await _next.Invoke(context);
            }           

            // TODO: Limpiar.
        }

        private static bool TryRetrieveToken(HttpContext context, out string token)
        {
            token = null;
            StringValues authzHeaders;
            if (!context.Request.Headers.TryGetValue("Authorization", out authzHeaders) || authzHeaders.Count > 1)
            {
                return false;
            }

            var bearerToken = authzHeaders[0];
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;

        }

        public bool LifeTimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        //public SecurityToken SignatureValidator(string token, TokenValidationParameters validationParameters)
        //{
        //    var jwt = new JwtSecurityToken(token);
        //    return jwt;
        //}
    }

    public static class ValidaJwtExtensions
    {
        public static IApplicationBuilder UseValidaJwt(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidaJwt>();
        }
    }
}
