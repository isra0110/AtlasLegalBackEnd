//using Microsoft.AspNetCore.Authorization;
using Abp.Domain.Repositories;
using Atlas.Legal.Autorizacion;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal
{
    //[Authorize()]
    public class CatalogosAppService : LegalAppServiceBase, ICatalogosAppService
    {
        private readonly ICatalogosRepository _catalogosRepository;
        
        public CatalogosAppService(ICatalogosRepository catalogosRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<OpeUsuario, int> opeUsuarioRepository) : base(httpContextAccessor, opeUsuarioRepository)
        {
            _catalogosRepository = catalogosRepository;
        }

        public List<Dictionary<string, object>> ObtenerCatalogo(ObtenerCatalogoInput input)
        {
            var result = _catalogosRepository.ObtenerCatalogo(input);

            return result;
        }

        public ListarCatalogosOutput ListarCatalogos()
        {
            var result = _catalogosRepository.ListarCatalogos();

            return result;
        }

        public async Task<CatalogoModelOutput> AgregarRegistroCatalogo(AgregarRegistroCatalogoInput input)
        {
            _catalogosRepository.AgregarRegistroCatalogo(input);

            return new CatalogoModelOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<CatalogoModelOutput> ActualizarRegistroCatalogo(ActualizarRegistroCatalogoInput input)
        {
            _catalogosRepository.ActualizarRegistroCatalogo(input);

            return new CatalogoModelOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }

        public async Task<CatalogoModelOutput> EliminarRegistroCatalogo(EliminarRegistroCatalogoInput input)
        {
            _catalogosRepository.EliminarRegistroCatalogo(input);

            return new CatalogoModelOutput
            {
                Mensaje = "Operación Exitosa"
            };
        }
    }
}
