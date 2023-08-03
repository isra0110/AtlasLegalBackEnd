using Abp.Data;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Atlas.Legal.EntityFrameworkCore;
using Atlas.Legal.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Legal.Notificacion
{
    public class Notificacion : LegalRepositoryBase<Entity, int>, INotificacion
    {
        public Notificacion(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider) : base(dbContextProvider, transactionProvider)
        {
        }

        public async Task GenerarNotificaciones()
        {
            EnsureConnectionOpen();

            using (var mCommand = CreateCommand("spGeneraNotificaciones",
                CommandType.StoredProcedure))
            {
                await mCommand.ExecuteNonQueryAsync();
            }           
        }
    }
}
