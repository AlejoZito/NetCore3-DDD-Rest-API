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
    public class EventTypeRepository : BaseRepository<EventType>
    {
        public EventTypeRepository(AppDbContext dbContext) : base(dbContext) { }
        public override Task<EventType> FindAsync(Expression<Func<EventType, bool>> predicate)
        {
            return _dbContext.EventTypes
                .Include(x=>x.Category)
                .FirstOrDefaultAsync(predicate);
        }
    }
}