using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atlas.Legal.Autorizacion
{
    public interface IAutorizacionRepository : IRepository<OpeUsuario, int>
    {
        void CrearUsuario(CrearUsuarioInput input, out string mensaje, out string usuario, out int idUsuario);
        ValidarAccesoSPOutput ValidarAccesoSP(ValidarAccesoSPInput input);
        ConsultaUsuarioOutput ConsultaUsuario(string usuario);
        void GuardarActualizarRolMenu(GuardarRolMenuInput input, out string mensaje, out int idRol);
        ConsultaRolOutput ConsultaRol(ConsultaRolInput input);
        ConsultaMenuOutput ConsultaMenu(ConsultaMenuInput input);
        ConsultaUsuarioRolOutput ConsultaUsuarioRol(ConsultaUsuarioRolInput input);
    }
}
