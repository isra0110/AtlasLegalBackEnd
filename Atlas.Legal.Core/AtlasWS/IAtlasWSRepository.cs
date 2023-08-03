using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Atlas.Legal.AtlasWS.AtlasWSOutput;

namespace Atlas.Legal.AtlasWS
{
    public interface IAtlasWSRepository : ITransientDependency
    {
        Task<NotificacionesOutput> EnviaNotificacion(NotificacionesInput input);
        Task<JuridicoOutput> ConsultaJuridico(JuridicoInput input);
        Task<SiniestroAtlasOutput> ConsultaSiniestros(SiniestroAtlasInput input);
        Task<LitigioAtlasOutput> ConsultaLitigio(LitigioAtlasInput input);
        Task<RecuperacionesSiniestroOutput> ConsultaRecuperaciones(RecuperacionesSiniestroInput input);
    }
}
