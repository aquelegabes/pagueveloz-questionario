using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PagueVeloz.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable
        where T : IEntity
    {
        long Count();
        long Count(Expression<Func<T, bool>> @where);
        Task<T> GetById(int id);
        IEnumerable<T> GetPaginated(int page = 1, int take = 5);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        IQueryable<T> Query { get; }
    }
}