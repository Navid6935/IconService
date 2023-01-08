using System.Linq.Expressions;
using System.Transactions;
using Domain.Interfaces;
using Domain.Models;
using Infra.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        #region Constructor

        private ApplicationContext context;
        private DbSet<TEntity> dbSet;

        #endregion

        public Repository(ApplicationContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }
        //public IQueryable<TEntity> GetEntitiesByQuery()
        //{
        //    return dbSet.Where(entity => entity.IsDeleted == false).AsQueryable();
        //}
        public IQueryable<TEntity> GetEntitiesByQuery(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = dbSet.Where(entity => entity.IsDeleted == false).AsQueryable();
            if (includes != null)
                includes.ToList().ForEach(include =>
                {
                    if (include != null)
                        query = query.Include(include);
                });

            return query;
        }

        public async Task<TEntity> GetEntitiesById(Guid Id)
        {
            return await dbSet.FirstOrDefaultAsync(s => s.Id == Id && s.IsDeleted == false);
        }

        public async Task<bool> AddEntity(TEntity entity, Transaction transaction = null)
        {
            try
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = entity.CreatedAt;

                entity.Id = Guid.NewGuid();
                if (transaction is not null)
                {
                    await context.Database.OpenConnectionAsync();
                    context.Database.EnlistTransaction(transaction);
                }
                await dbSet.AddAsync(entity);
                if (entity.Id != Guid.Empty)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UpdateEntity(TEntity entity, bool updateStateId = false, string tableName = "",
            Transaction transaction = null)
        {
            try
            {
                if (transaction is not null)
                {
                    await context.Database.OpenConnectionAsync();
                    context.Database.EnlistTransaction(transaction);
                }

                if (updateStateId)
                {
                    var query = $"DELETE FROM \"Icons\".\"{tableName}\" WHERE \"Id\" = '{entity.Id}'";
                    await context.Database.ExecuteSqlRawAsync(query);
                    dbSet.Add(entity);
                    return;
                }

                entity.UpdatedAt = DateTime.UtcNow;
                dbSet.Update(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task RemoveEntity(TEntity entity, Transaction transaction = null)
        {
            try
            {
                entity.IsDeleted = true;
                await UpdateEntity(entity, false, "", transaction);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public async Task RemoveEntity(Guid entityId, Transaction transaction = null)
        {
            try
            {
                var entity = await GetEntitiesById(entityId);
                await RemoveEntity(entity, transaction);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }

        public async Task HardRemoveEntity(Guid entityId)
        {
            try
            {
                var entity = await GetEntitiesById(entityId);
                dbSet.Remove(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<bool> SaveChanges(Transaction transaction = null)
        {
            try
            {
                if (transaction is not null)
                {
                    await context.Database.OpenConnectionAsync();
                    context.Database.EnlistTransaction(transaction);
                    await context.SaveChangesAsync();

                    await context.Database.CloseConnectionAsync();
                    return true;
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}