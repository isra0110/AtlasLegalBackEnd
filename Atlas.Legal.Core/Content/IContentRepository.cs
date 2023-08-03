using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.Content.ContentRepository;

namespace Atlas.Legal.Content
{
    public interface IContentRepository : ITransientDependency
    {
        void GuardarDocumento(GuardarDocumentoInput input);
        void ObtenerDocumento(string objectId, out string contenido);
        void EliminarDocumento(string objectId);
    }
}
