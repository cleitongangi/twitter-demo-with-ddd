using Posterr.Domain.Interfaces.Data;

namespace Posterr.Infra.Data.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IPosterrDbContext _posterrDb;

        public UnitOfWork(IPosterrDbContext posterrDb)
        {
            this._posterrDb = posterrDb;
        }

        public async Task BeginTransactionAsync()
        {
            if (_posterrDb.Database.CurrentTransaction == null)
                await _posterrDb.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_posterrDb.Database.CurrentTransaction != null)
                await _posterrDb.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            if (_posterrDb.Database.CurrentTransaction != null)
                await _posterrDb.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _posterrDb.SaveChangesAsync();
        }
    }
}
