using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal.Autorizacion
{
    public interface IAutorizacionAppService : IApplicationService
    {
        Task<CrearUsuarioOutput> CrearActualizarUsuario(CrearUsuarioInput input);
        Task<LoginOutput> Login(LoginInput input);
        ConsultaUsuarioOutput ConsultaUsuario();
        Task<GuardarRolMenuOutput> GuardarActualizarRolMenu(GuardarRolMenuInput input);
        ConsultaRolOutput ConsultaRol(ConsultaRolInput input);
        ConsultaMenuOutput ConsultaMenu(ConsultaMenuInput input);
        ConsultaUsuarioRolOutput ConsultaUsuarioRol(ConsultaUsuarioRolInput input);
    }
}
