using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace Atlas.Legal
{
    public interface ICatalogosAppService : IApplicationService
    {
        List<Dictionary<string, object>> ObtenerCatalogo(ObtenerCatalogoInput input);
        ListarCatalogosOutput ListarCatalogos();
        Task<CatalogoModelOutput> AgregarRegistroCatalogo(AgregarRegistroCatalogoInput input);
        Task<CatalogoModelOutput> ActualizarRegistroCatalogo(ActualizarRegistroCatalogoInput input);
        Task<CatalogoModelOutput> EliminarRegistroCatalogo(EliminarRegistroCatalogoInput input);
    }
}
