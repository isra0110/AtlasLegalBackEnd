using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Legal
{
    public class ObtenerCatalogoInput
    {
        public int Id { get; set; }
    }

    public class ListarCatalogosOutput
    {
        public List<CatalogosDto> ListaCatalogos { get; set; }

        public ListarCatalogosOutput()
        {
            ListaCatalogos = new List<CatalogosDto>();
        }

        public class CatalogosDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

    }

    public class AgregarRegistroCatalogoInput
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class ActualizarRegistroCatalogoInput
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdRegistro { get; set; }
    }

    public class EliminarRegistroCatalogoInput
    {
        public int Id { get; set; }
        public int IdRegistro { get; set; }
    }

    public class CatalogoModelOutput
    {
        public string Mensaje { get; set; }
    }
}
