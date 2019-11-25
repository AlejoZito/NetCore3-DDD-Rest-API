using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Contracts.Exceptions
{
    public class InvalidChargeException : Exception
    {
        public InvalidChargeException(string message) : base(message) { }
    }
}
