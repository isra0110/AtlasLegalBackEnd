using Abp.Data;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;


namespace Atlas.Legal.EntityFrameworkCore.Repositories
{
    public abstract class LegalRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<LegalDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly IActiveTransactionProvider _transactionProvider;

        public LegalRepositoryBase(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider) 
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
        }

        //add your common methods for all repositories
        private protected DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var mCommand = Context.Database.GetDbConnection().CreateCommand();

            mCommand.CommandText = commandText;
            mCommand.CommandType = commandType;
            mCommand.Transaction = GetActiveTransaction();

            foreach (var item in parameters)
            {
                mCommand.Parameters.Add(item);
            }

            return mCommand;
        }

        private protected void EnsureConnectionOpen()
        {
            var mConnection = Context.Database.GetDbConnection();

            if (mConnection.State != ConnectionState.Open)
            {
                mConnection.Open();
            }
        }

        private protected DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
            {
                {"ContextType", typeof(LegalDbContext) },
                {"MultiTenancySide", MultiTenancySide }
            });
        }

        private protected bool HasColumn(DbDataReader dataReader, string columnName)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
                if (dataReader.GetName(i).Equals(columnName, System.StringComparison.InvariantCultureIgnoreCase))
                    return true;
            return false;
        }

    }

    public abstract class LegalRepositoryBase<TEntity> : LegalRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected LegalRepositoryBase(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider, transactionProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}