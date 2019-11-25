using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Enumerations;
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

            //Leave Currency null if value can't be parsed
            if (Enum.TryParse(typeof(Currency), currency, true, out object parsedCurrency))
                Currency = (Currency)parsedCurrency;
            else
                Currency = null;
        }

        public AmountCurrency(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; set; }

        /// <summary>
        /// Made nullable for validation implementation
        /// </summary>
        public Currency? Currency { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

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
                    this.Currency.ToString());
        }

        public void Validate()
        {
            ValidationErrors = new List<ValidationError>(); 

            if (Amount <= 0)
                ValidationErrors.Add(new ValidationError(nameof(Amount), "Amount is invalid, must be greater than 0"));

            if (Currency == null)
                ValidationErrors.Add(new ValidationError(nameof(Currency), "Currency is invalid"));
            else if (!Enum.IsDefined(typeof(Currency), Currency))
                ValidationErrors.Add(new ValidationError(
                    nameof(Currency), 
                    $"Invalid currency, valid currencies are [{string.Join(", ",Enum.GetNames(typeof(Currency)))}"));
        }
    }
}
