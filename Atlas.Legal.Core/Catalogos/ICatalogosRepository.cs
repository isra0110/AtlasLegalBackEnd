using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    public interface ICatalogosRepository : IRepository<Siniestro, int>
    {
        List<Dictionary<string, object>> ObtenerCatalogo(ObtenerCatalogoInput input);
        ListarCatalogosOutput ListarCatalogos();
        void AgregarRegistroCatalogo(AgregarRegistroCatalogoInput input);
        void ActualizarRegistroCatalogo(ActualizarRegistroCatalogoInput input);
        void EliminarRegistroCatalogo(EliminarRegistroCatalogoInput input);
    }
}
