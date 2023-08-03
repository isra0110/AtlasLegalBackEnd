using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal.Acceso
{
    public interface IAccesoRepository : ITransientDependency
    {
        ValidarAccesoWSOutput ValidarAccesoWS(ValidarAccesoWSInput acceso);
    }
}
