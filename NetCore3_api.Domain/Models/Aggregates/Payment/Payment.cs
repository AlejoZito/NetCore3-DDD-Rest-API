using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using U = NetCore3_api.Domain.Models.Aggregates.User;

namespace NetCore3_api.Domain.Models.Aggregates.Payment
{
    public class Payment : Entity, IValidatable
    {
        public Payment() { }
        public Payment(decimal amount, Currency currency, U.User user)
        {
            Amount = amount;
            Currency = currency;
            User = user;
            Date = DateTime.Now;
        }

        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public U.User User { get; set; }
        public DateTime Date {get;set;}
        public List<PaymentCharge> Charges { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public bool IsValid()
        {
            Validate();
            return ValidationErrors.Count == 0;
        }

        public void Validate()
        {

            ValidationErrors = new List<ValidationError>();
            if (Amount <= 0)
                ValidationErrors.Add(new ValidationError(nameof(Amount), "Payment amount should be greater than 0"));
            if (User == null)
                ValidationErrors.Add(new ValidationError(nameof(User), "Could not find requested user"));

            //ToDo: agregar regla de validacion de currency (habría que hacer la prop. nullable)
            //Esta regla quedó en el payment service
        }
    }
}
