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
using NetCore3_api.Domain.Models.Aggregates.Event;

namespace NetCore3_api.Infrastructure.Repositories
{
    public class ChargeRepository : BaseRepository<Charge>
    {
        public ChargeRepository(AppDbContext dbContext) : base(dbContext) { }
        public override async Task<List<Charge>> ListAsync(SortOptions sortOptions = null, int? pageSize = int.MaxValue, int? pageNumber = 1)
        {
            if (sortOptions == null)
                sortOptions = SortOptions.GetDefaultValue();
            if (!pageSize.HasValue)
                pageSize = int.MaxValue;
            if (!pageNumber.HasValue)
                pageNumber = 1;

            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than zero");

            return await _dbContext.Charges
                .Include(x => x.Payments).ThenInclude(x => x.Payment)
                .Include(x => x.Event).ThenInclude(x => x.User)
                .Include(x => x.Event).ThenInclude(x => x.Type).ThenInclude(x=>x.Category)
                .OrderBy(sortOptions.Column + " " + (sortOptions.Order == SortOrder.Ascending ? "ASC" : "DESC"))
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
        }
        public override async Task<List<Charge>> ListAsync(
            Expression<Func<Charge, bool>> predicate, 
            SortOptions sortOptions = null, 
            int? pageSize = int.MaxValue, int? pageNumber = 1)
        {
            if (sortOptions == null)
                sortOptions = SortOptions.GetDefaultValue();
            if (!pageSize.HasValue)
                pageSize = int.MaxValue;
            if (!pageNumber.HasValue)
                pageNumber = 1;

            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than zero");

            return await _dbContext.Charges
                .Include(x => x.Payments).ThenInclude(x => x.Payment)
                .Include(x => x.Event).ThenInclude(x => x.User)
                .Include(x => x.Event).ThenInclude(x => x.Type).ThenInclude(x => x.Category)
                .Where(predicate)
                .OrderBy(sortOptions.Column + " " + (sortOptions.Order == SortOrder.Ascending ? "ASC" : "DESC"))
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
        }
        public override Task<Charge> FindByIdAsync(long id)
        {
            return _dbContext.Charges
                .Include(x => x.Payments).ThenInclude(x => x.Payment)
                .Include(x=>x.Event).ThenInclude(x=>x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}