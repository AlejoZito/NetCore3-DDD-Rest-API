using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.Aggregates.User
{
    public class User : Entity
    {
        //ToDo: definir si se guarda la deuda desnormalizada
        // o con otra entidad "Debt" con tantas apariciones en db
        // como currencies por user
        public decimal DebtAmount { get; set; }
        public decimal GetDebtAmount() { return default; }
    }
}
