using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Contracts.Exceptions
{
    public class InvalidEntityException : Exception
    {
        public InvalidEntityException(Entity model)
        {
            Model = model;
        }
        public Entity Model { get; set; }
    }
}
