using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseModel
    {
        Task<bool> AddEntity(TEntity entity, Transaction transaction = null);
        IQueryable<TEntity> GetEntitiesByQuery( params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetEntitiesById(Guid Id);
        Task RemoveEntity(TEntity entity,Transaction transaction = null);
        Task RemoveEntity(Guid entityId,Transaction transaction = null);
        Task HardRemoveEntity(Guid entityId);
        Task<bool> SaveChanges(Transaction transaction = null);
        Task UpdateEntity(TEntity entity, bool updatePrimaryKey = false,string tableName = "",Transaction transaction=null);
    }

}
