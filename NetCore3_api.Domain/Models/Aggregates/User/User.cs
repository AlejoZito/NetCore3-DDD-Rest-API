using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.Aggregates.User
{
    public class User : Entity
    {
        public string Username { get; set; }
        public decimal GetDebtAmount() { return default; }
    }
}
