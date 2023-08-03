using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.ExceptionHandling;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Atlas.Legal.Web.Filters
{
    public class CustomExceptionFilter : AbpExceptionFilter
    {
        public CustomExceptionFilter(IErrorInfoBuilder errorInfoBuilder, IAbpAspNetCoreConfiguration configuration)
            :base(errorInfoBuilder, configuration)
        {

        }

        protected override int GetStatusCode(ExceptionContext context, bool wrapOnError)
        {
            if (context.Exception is AbpAuthorizationException)
            {
                return context.HttpContext.User.Identity.IsAuthenticated
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;
            }

            if (context.Exception is AbpValidationException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            if (context.Exception is UserFriendlyException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            if (context.Exception is EntityNotFoundException)
            {
                return (int)HttpStatusCode.NotFound;
            }

            if (wrapOnError)
            {
                return (int)HttpStatusCode.InternalServerError;
            }

            return context.HttpContext.Response.StatusCode;
        }
    }
}
