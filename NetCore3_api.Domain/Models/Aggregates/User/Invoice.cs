using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Contracts.Exceptions;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore3_api.Domain.Models.Aggregates.User
{
    public class Invoice : Entity, IValidatable
    {
        /// <summary>
        /// Parameterless constructor required by Entity Framework
        /// </summary>
        public Invoice()
        {
            Charges = new List<Charge>();
        }
        public Invoice(int month, int year, Currency currency, User user)
        {
            Charges = new List<Charge>();
            Month = month;
            Year = year;
            Currency = currency;
            User = user;
        }

        public int Month { get; set; }
        public int Year { get; set; }
        private List<Charge> Charges { get; set; }
        public Currency Currency { get; set; }
        public User User { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public AmountCurrency GetAmountCurrency()
        {
            if (Charges != null && Charges.Count > 0)
            {
                return new AmountCurrency(Charges.Sum(x => x.Amount.Amount), this.Currency);
            }
            else
                return null;
        }

        public void AddCharge(Charge charge)
        {
            if (charge.Amount == null || charge.Amount.Amount == 0)
                throw new InvalidChargeException("Cannot add charge to invoice, invalid amount");
            if (charge.Amount.Currency == null || charge.Amount.Currency != this.Currency)
                throw new InvalidChargeException("Cannot add charge to invoice, invalid currency");
            if(charge.Event.Date.Year != this.Year || charge.Event.Date.Month != this.Month)
                throw new InvalidChargeException("Cannot add charge to invoice, invalid charge date");
            
            Charges.Add(charge);
        }

        public bool IsValid()
        {
            Validate();
            return ValidationErrors.Count == 0;
        }

        public void Validate()
        {
            ValidationErrors = new List<ValidationError>();
            if (Month == 0)
                ValidationErrors.Add(new ValidationError(nameof(Month), "Invalid month"));
            if (Year == 0)
                ValidationErrors.Add(new ValidationError(nameof(Year), "Invalid year"));

            //ToDo: Validate charge list exists and has items
        }
    }
}
