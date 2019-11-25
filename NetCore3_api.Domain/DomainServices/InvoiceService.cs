using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Contracts.Exceptions;
using NetCore3_api.Domain.Enumerations;
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

        public async Task<Invoice> AddCharge(Charge charge, User user)
        {
            Currency? chargeCurrency = charge?.GetCurrency();
            int month = 0;
            int year = 0;

            //Try to get month and year from charge event
            if(charge.Event != null && charge.Event.Date != default(DateTime))
            {
                month = charge.Event.Date.Month;
                year = charge.Event.Date.Year;
            }
            Invoice foundInvoice = await _invoiceRepository.FindAsync(x =>
                x.User.Id == user.Id &&
                x.Month == month &&
                x.Year == year &&
                x.Currency == chargeCurrency);

            //If no invoice exists for this user/month/year combination
            //create a new one
            if(foundInvoice == null)
            {
                Invoice newInvoice = new Invoice(
                    month,
                    year,
                    chargeCurrency,
                    user
                );

                newInvoice.AddCharge(charge);

                if (newInvoice.IsValid())
                {
                    return _invoiceRepository.Insert(newInvoice);
                }
                else
                    throw new InvalidEntityException(newInvoice);
            }
            else
            {
                //If invoice was found, add charge
                foundInvoice.AddCharge(charge);
                if (foundInvoice.IsValid())
                {
                    return _invoiceRepository.Update(foundInvoice);
                }
                else
                    throw new InvalidEntityException(foundInvoice);
            }
        }

        public async Task<Invoice> GetInvoiceForUser(long id, int month, int year)
        {
            return await _invoiceRepository.FindAsync(x =>
                x.User.Id == id &&
                x.Month == month &&
                x.Year == year);
        }
    }
}
