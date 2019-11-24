using Microsoft.EntityFrameworkCore;
using NetCore3_api.Domain;
using NetCore3_api.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace NetCore3_api.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected AppDbContext _dbContext;
        public BaseRepository(AppDbContext atlasDbContext)
        {
            _dbContext = atlasDbContext;
        }

        public virtual void Delete(long id)
        {
            T entity = _dbContext.Set<T>().Find(id);
            _dbContext.Remove(entity);
        }

        public virtual Task<T> FindByIdAsync(long id)
        {
            return _dbContext.Set<T>().FindAsync(id).AsTask();
        }
        public virtual Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual T Insert(T entity)
        {
            return _dbContext.Set<T>().Add(entity).Entity;
        }

        public virtual T Update(T entity)
        {
            return _dbContext.Set<T>().Update(entity).Entity;
        }

        public virtual Task<List<T>> ListAsync(SortOptions sortOptions = null, int? pageSize = int.MaxValue, int? pageNumber = 1)
        {
            if (sortOptions == null)
                sortOptions = SortOptions.GetDefaultValue();
            if (!pageSize.HasValue)
                pageSize = int.MaxValue;
            if (!pageNumber.HasValue)
                pageNumber = 1;

            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than zero");

            return _dbContext.Set<T>()
                .OrderBy(sortOptions.Column + " " + (sortOptions.Order == SortOrder.Ascending ? "ASC" : "DESC"))
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
        }

        public virtual Task<List<T>> ListAsync(
            Expression<Func<T, bool>> predicate,
            SortOptions sortOptions = null,
            int? pageSize = int.MaxValue,
            int? pageNumber = 1)
        {
            if (sortOptions == null)
                sortOptions = SortOptions.GetDefaultValue();
            if (!pageSize.HasValue)
                pageSize = int.MaxValue;
            if (!pageNumber.HasValue)
                pageNumber = 1;

            return _dbContext.Set<T>()
                .Where(predicate)
                .OrderBy(sortOptions.Column + " " + (sortOptions.Order == SortOrder.Ascending ? "ASC" : "DESC"))
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
        }
    }
}