using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Contracts
{
    public interface IValidatable
    {
        /// <summary>
        /// Executes <see cref="Validate"/> method and checks if any validation error was stored in <see cref="ValidationErrors"/> property
        /// </summary>
        /// <returns></returns>
        bool IsValid();

        /// <summary>
        /// Run validation logic and store validation errors in <see cref="ValidationErrors"/> property
        /// </summary>
        void Validate();

        List<ValidationError> ValidationErrors { get; set; }
    }
}
