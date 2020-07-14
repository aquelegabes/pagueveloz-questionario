using System;
using System.Threading.Tasks;

namespace PagueVeloz.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync();
    }
}