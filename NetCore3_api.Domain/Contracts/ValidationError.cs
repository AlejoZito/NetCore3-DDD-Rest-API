using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Contracts
{
    public class ValidationError
    {
        public ValidationError(string propertyName, string errorDescription)
        {
            PropertyName = propertyName;
            ErrorDescription = errorDescription;
        }
        public string PropertyName { get; set; }
        public string ErrorDescription { get; set; }
    }
}
