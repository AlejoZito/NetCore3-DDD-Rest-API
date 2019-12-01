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
            return await _dbContext.Charges
                .Include(x => x.Payments).ThenInclude(x => x.Payment)
                .Include(x => x.Event).ThenInclude(x => x.User)
                .Include(x => x.Event).ThenInclude(x => x.Type).ThenInclude(x=>x.Category)
                .ToListAsync();
        }
        public override async Task<List<Charge>> ListAsync(
            Expression<Func<Charge, bool>> predicate, 
            SortOptions sortOptions = null, 
            int? pageSize = int.MaxValue, int? pageNumber = 1)
        {
            return await _dbContext.Charges
                .Include(x => x.Payments).ThenInclude(x => x.Payment)
                .Include(x => x.Event).ThenInclude(x => x.User)
                .Include(x => x.Event).ThenInclude(x => x.Type).ThenInclude(x => x.Category)
                .Where(predicate)
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