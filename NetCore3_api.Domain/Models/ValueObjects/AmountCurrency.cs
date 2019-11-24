using NetCore3_api.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.ValueObjects
{
    public class AmountCurrency : IValidatable
    {
        public AmountCurrency(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public List<ValidationError> ValidationErrors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsValid()
        {
            Validate();
            return ValidationErrors.Count == 0;
        }

        public AmountCurrency Sum(AmountCurrency amountCurrency)
        {
            if (amountCurrency.Currency != this.Currency)
                throw new InvalidOperationException("Cannot sum different currencies");
            else
                return new AmountCurrency(
                    amountCurrency.Amount + this.Amount, 
                    this.Currency);
        }

        public void Validate()
        {
            ValidationErrors = new List<ValidationError>(); 

            if (Amount <= 0)
                ValidationErrors.Add(new ValidationError(nameof(Amount), "Amount is invalid, must be greater than 0"));

            if (Amount <= 0)
                ValidationErrors.Add(new ValidationError(nameof(Amount), "Amount is invalid, must be greater than 0"));
        }
    }
}
