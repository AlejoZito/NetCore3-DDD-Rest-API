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
using NetCore3_api.Domain.Models.Aggregates.User;

namespace NetCore3_api.Infrastructure.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>
    {
        public InvoiceRepository(AppDbContext dbContext) : base(dbContext) { }
        public override async Task<List<Invoice>> ListAsync(
            Expression<Func<Invoice, bool>> predicate, 
            SortOptions sortOptions = null, 
            int? pageSize = int.MaxValue, int? pageNumber = 1)
        {
            //ToDo: pagination
            return await _dbContext.Invoices
                .Include(x => x.Charges).ThenInclude(x => x.Payments).ThenInclude(x=>x.Payment)
                .Where(predicate)
                .OrderByDescending(x=>x.Year).ThenByDescending(x=>x.Month).ThenBy(x=>x.Currency)
                .ToListAsync();
        }
    }
}