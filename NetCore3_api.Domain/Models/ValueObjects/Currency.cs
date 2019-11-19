using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.ValueObjects
{
    public class Currency
    {
        /// <summary>
        /// USD, ARS
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// e.g. AR$, US$
        /// </summary>
        public string Symbol { get; set; }
    }
}
