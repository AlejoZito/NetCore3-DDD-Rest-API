using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore3_api.Domain.Contracts
{
    public interface IInvoiceService
    {
        Task<Invoice> AddCharge(Charge charge, User user);
        Task<Invoice> AddPayment(Payment payment, User user);
        Task<List<Invoice>> GetInvoicesForUser(long userId, int? fromYear, int? fromMonth, int? toYear, int? toMonth);
        Task<Invoice> GetInvoiceForUser(long id, int month, int year);
    }
}
