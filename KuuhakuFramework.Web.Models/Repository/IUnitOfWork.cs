using KuuhakuFramework.Web.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KuuhakuFramework.Web.Models.Repository
{
    public interface IUnitOfWork<TContext> where TContext : KuuhakuContext
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        TRepository GetRepository<TEntity, TRepository>() where TEntity : class, IEntity where TRepository : class, IRepository<TEntity>;

        Task CommitAsync(CancellationToken ct = default);

        Task<TOutput> ExecuteAsync<TOutput>(Func<TContext, CancellationToken, Task<TOutput>> action, CancellationToken ct = default);
        Task ExecuteAsync(Func<TContext, CancellationToken, Task> action, CancellationToken ct = default);
    }
}
