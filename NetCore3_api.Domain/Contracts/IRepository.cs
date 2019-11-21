using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCore3_api.Domain.Contracts
{
    public interface IRepository<T> where T : Entity
    {
        Task<List<T>> ListAsync(SortOptions sortOptions = null, int? pageSize = int.MaxValue, int? pageNumber = 1);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, SortOptions sortOptions = null, int? pageSize = int.MaxValue, int? pageNumber = 1);
        Task<T> FindByIdAsync(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        T Insert(T entity);
        T Update(T entity);
        void Delete(int entity);
    }
}
