using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PagueVeloz.Data.Context;
using PagueVeloz.Domain.Interfaces;

namespace PagueVeloz.Data.Repository
{
    public sealed class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        private PagueVelozContext Ctx { get; }
        private DbSet<T> DbSet => Ctx.Set<T>();

        public Repository(PagueVelozContext ctx)
        {
            Ctx = ctx;
        }

        public IQueryable<T> Query => Ctx.Set<T>();

        public void Add(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            Ctx.Add(entity);
        }

        public long Count() => DbSet.Count();

        public long Count(Expression<Func<T, bool>> where) =>
            DbSet.Count(where);

        public void Delete(int id)
        {
            var entity = DbSet.Find(id);
            if (entity != null)
                DbSet.Remove(entity);
        }

        public void Dispose()
        {
            Ctx.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<T> GetById(int id) =>
            await DbSet.FindAsync(id);

        public IEnumerable<T> GetPaginated(int page = 1, int take = 5) =>
            page == 1 ?
                Query.Take(take) :
                Query.Skip(take * (page-1)).Take(take);

        public void Update(T entity) =>
            DbSet.Update(entity);
    }
}