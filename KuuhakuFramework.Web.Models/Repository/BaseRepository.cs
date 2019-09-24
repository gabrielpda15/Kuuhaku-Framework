using Contabily.Extensions.Exceptions;
using Contabily.Models.Entities.Base;
using Contabily.Models.Repository.Interfaces;
using Contabily.Models.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contabily.Models.Repository.Base
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected DbSet<TEntity> Entities { get; }
        protected ModelContext Context { get; }

        protected BaseRepository(ModelContext context)
        {
            Context = context;
        }

        public virtual void OnAdd(TEntity entity, IUserContext userContext)
        {
            entity.CreationDate = DateTime.Now;
            entity.CreationIp = userContext.IP;
            entity.CreationUser = ((ClaimsPrincipal)userContext.Principal).Claims.FirstOrDefault().Value;
            OnUpdate(entity, userContext);
        }

        public virtual void OnUpdate(TEntity entity, IUserContext userContext)
        {
            entity.EditionDate = DateTime.Now;
            entity.EditionIp = userContext.IP;
            entity.EditionUser = ((ClaimsPrincipal)userContext.Principal).Claims.FirstOrDefault().Value;
        }

        protected virtual IQueryable<TEntity> GetEntities()
        {
            return Entities;
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> filter, IUserContext userContext, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, CancellationToken ct = default)
        {
            return await Task.Run(() =>
            {
                var query = GetEntities();

                if (filter == null) filter = x => x;
                query = filter.Compile()(query);

                if (orderBy != null) query = orderBy(query);
                else query = query.OrderBy(x => x.Id);

                return query.ToArray();
            }, ct);
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, IUserContext userContext, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, CancellationToken ct = default)
        {
            return await Task.Run(() =>
            {
                var query = GetEntities();

                if (filter == null) filter = x => true;
                query = query.Where(filter);

                if (orderBy != null) query = orderBy(query);
                else query = query.OrderBy(x => x.Id);

                return query.ToArray();
            }, ct);
        }

        public virtual async Task<TEntity> QueryScalarAsync(Expression<Func<TEntity, bool>> filter, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                var query = GetEntities();

                query = query.Where(filter);

                try
                {
                    return await query.SingleAsync(ct);
                }
                catch { throw new QueryException("Não foi possivel localizar apenas um valor que satifaz a condição"); }
            }, ct);
        }

        public virtual async Task<TEntity> QueryByIdAsync(int id, IUserContext userContext, CancellationToken ct = default)
        {
            try
            {
                return await GetEntities().SingleAsync(x => x.Id == id, ct);
            }
            catch { throw new QueryException("Não foi possivel localizar apenas um valor que satifaz a condição"); }
        }

        public virtual async Task<TEntity> CreateNewAsync(IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.FromResult(Activator.CreateInstance<TEntity>());
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                OnAdd(entity, userContext);

                await Entities.AddAsync(entity, ct).ConfigureAwait(false);

                return entity;
            }, ct);
        }

        public virtual async Task<IEnumerable<TEntity>> AddAllAsync(IEnumerable<TEntity> entities, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                foreach (var entity in entities)
                    OnAdd(entity, userContext);

                await Entities.AddRangeAsync(entities, ct).ConfigureAwait(false);

                return entities;
            }, ct);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(() =>
            {
                OnUpdate(entity, userContext);

                Entities.Update(entity);
                return entity;
            }, ct);
        }

        public virtual async Task<IEnumerable<TEntity>> UpdateAllAsync(IEnumerable<TEntity> entities, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(() =>
            {
                foreach (var entity in entities)
                    OnUpdate(entity, userContext);

                Entities.UpdateRange(entities);

                return entities;
            }, ct);
        }

        public virtual async Task<bool> DeleteAsync(int id, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                TEntity toRemove;
                try
                {
                    toRemove = await Entities.SingleAsync(x => x.Id == id);
                }
                catch { throw new QueryException("Não foi possivel localizar apenas um valor que satifaz a condição"); }

                Entities.Remove(toRemove);

                return true;
            }, ct);
        }

        public virtual async Task<string> DeleteAllAsync(string ids, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                var toReturn = new List<string>();

                foreach (var sid in ids.Split(','))
                {
                    try
                    {
                        if (!int.TryParse(sid, out int id)) throw new FormatException("Não é possivel converter os ids fornecidos em numeros");
                        var toRemove = await Entities.SingleAsync(x => x.Id == id, ct);
                        Entities.Remove(toRemove);
                    }
                    catch (FormatException ex) { throw ex; }
                    catch { toReturn.Add(sid); }
                }

                if (toReturn.Count == 0) return null;
                return string.Join(',', toReturn);
            }, ct);
        }        
    }
}
