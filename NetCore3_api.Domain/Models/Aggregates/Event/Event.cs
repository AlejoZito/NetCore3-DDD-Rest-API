using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using U = NetCore3_api.Domain.Models.Aggregates.User;

namespace NetCore3_api.Domain.Models.Aggregates.Event
{
    public class Event : Entity, IValidatable
    {
        public EventType Type { get; set; }
        public DateTime Date { get; set; }
        public U.User User { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public bool IsValid()
        {
            Validate();
            return ValidationErrors.Count == 0;
        }

        public void Validate()
        {
            ValidationErrors = new List<ValidationError>();

            if(Type == null)
                ValidationErrors.Add(new ValidationError(nameof(Type), "Invalid event type"));
            else if (Type.Category == null)
                ValidationErrors.Add(new ValidationError(nameof(Type), "Invalid event type, no category found"));
            if (Type == null)
                ValidationErrors.Add(new ValidationError(nameof(Type), "Must enter a valid event type"));
            if(User == null)
                ValidationErrors.Add(new ValidationError(nameof(User), "User not found"));
        }
    }
}
