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
using NetCore3_api.Domain.Models.Aggregates.Payment;

namespace NetCore3_api.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>
    {
        public PaymentRepository(AppDbContext dbContext) : base(dbContext) { }
        public override Task<Payment> FindAsync(Expression<Func<Payment, bool>> predicate)
        {
            return _dbContext.Payments
                .Include(x => x.Charges).ThenInclude(x=>x.Charge)
                .FirstOrDefaultAsync(predicate);
        }
    }
}