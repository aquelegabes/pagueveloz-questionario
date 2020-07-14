using System;
using System.Threading.Tasks;
using PagueVeloz.Data.Context;
using PagueVeloz.Domain.Interfaces;

namespace PagueVeloz.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private PagueVelozContext Ctx { get; }

        public UnitOfWork(PagueVelozContext ctx)
        {
            Ctx = ctx;
        }

        public async Task<bool> CommitAsync()
        {
            using (var transaction = await Ctx.Database.BeginTransactionAsync())
            {
                try
                {
                    await Ctx.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public void Dispose()
        {
            Ctx.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}