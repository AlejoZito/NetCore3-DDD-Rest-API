using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.Aggregates.Event
{
    public class Charge : Entity, IValidatable
    {
        public AmountCurrency Amount { get; set; }
        public List<PaymentCharge> Payments { get; set; }
        public Event Event { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public bool IsValid()
        {
            Validate();
            return ValidationErrors.Count == 0;
        }

        public void Validate()
        {
            ValidationErrors = new List<ValidationError>();

            if (!Event.IsValid())
                ValidationErrors.AddRange(Event.ValidationErrors);
                //ValidationErrors.Add(new ValidationError(nameof(Event), "Event is not valid, check inner properties"));

            if (Amount == null)
                ValidationErrors.Add(new ValidationError(nameof(Amount), "Must enter a valid amount and currency"));

            if (!Amount.IsValid())
                ValidationErrors.AddRange(Amount.ValidationErrors);
        }

        public Currency? GetCurrency()
        {
            if (Amount != null)
                return Amount.Currency;
            else
                return null;
        }
    }
}
