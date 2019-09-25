using KuuhakuFramework.Web.Models.Attributes;
using KuuhakuFramework.Web.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KuuhakuFramework.Web.Models.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : KuuhakuContext
    {
        protected TContext Context { get; }
        protected IDictionary<Type, object> Repositories { get; }

        public UnitOfWork(TContext context)
        {
            Context = context;
            Repositories = new Dictionary<Type, object>();
            LoadRepositoriesFromAssembly(Assemblies);
        }

        protected virtual Assembly[] Assemblies { get => AppDomain.CurrentDomain.GetAssemblies(); }

        private void LoadRepositoriesFromAssembly(Assembly[] assemblies)
        {
            var repos = assemblies
                .Select(x => x.GetTypes().Where(x => x.GetCustomAttribute<RepositoryAttribute>() != null))
                .Aggregate(new List<Type>(), (x, y) => { x.AddRange(y); return x; });

            foreach (var repo in repos)
            {                
                var @base = repo.BaseType.GetGenericArguments().Single(x => typeof(IEntity).IsAssignableFrom(x));
                Repositories.Add(@base, Activator.CreateInstance(repo, Context));
            }
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return (IRepository<TEntity>)Repositories[typeof(TEntity)];
        }

        public TRepository GetRepository<TEntity, TRepository>() where TEntity : class, IEntity where TRepository : class, IRepository<TEntity>
        {
            return (TRepository)Repositories[typeof(TEntity)];
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            await Context.SaveChangesAsync(ct);
        }

        public async Task ExecuteAsync(Func<TContext, CancellationToken, Task> action, CancellationToken ct = default)
        {
            using (var transaction = await Context.Database.BeginTransactionAsync(ct))
            {
                await action(Context, ct);

                await transaction.CommitAsync(ct);
            }
        }

        public async Task<TOutput> ExecuteAsync<TOutput>(Func<TContext, CancellationToken, Task<TOutput>> action, CancellationToken ct = default)
        {
            TOutput output;

            using (var transaction = await Context.Database.BeginTransactionAsync(ct))
            {
                output = await action(Context, ct);

                await transaction.CommitAsync(ct);
            }

            return output;
        }
    }
}
