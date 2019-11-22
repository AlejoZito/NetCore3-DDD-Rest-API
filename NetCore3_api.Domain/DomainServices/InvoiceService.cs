using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Models;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore3_api.Domain.DomainServices
{
    public class InvoiceService
    {
        readonly IRepository<Invoice> _invoiceRepository;
        public InvoiceService(IRepository<Invoice> invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<Invoice> AddCharge(DateTime date, Charge charge, User user)
        {
            Invoice foundInvoice = await _invoiceRepository.FindAsync(x =>
                x.User.Id == user.Id &&
                x.Month == date.Month &&
                x.Year == date.Year);

            //If no invoice exists for this user/month/year combination
            //create a new one
            if(foundInvoice == null)
            {
                Invoice newInvoice = new Invoice()
                {
                    Month = date.Month,
                    Year = date.Year,
                    User = user,
                    Charges = new List<Charge>()
                    {
                        charge
                    }
                };
                return _invoiceRepository.Insert(newInvoice);
            }
            else
            {
                foundInvoice.Charges.Add(charge);
                return _invoiceRepository.Update(foundInvoice);
            }
        }
    }
}
