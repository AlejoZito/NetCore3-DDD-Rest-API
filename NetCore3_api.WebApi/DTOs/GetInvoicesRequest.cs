using NetCore3_api.WebApi.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class GetInvoicesRequest
    {
        private MonthYearDTO From;
        public MonthYearDTO GetFromMonthYear()
        {
            if(From != null && From.IsValidMonthYear())
                return From;
            else
                return null;
        }

        private MonthYearDTO To;
        public MonthYearDTO GetToMonthYear()
        {
            if (To != null && To.IsValidMonthYear())
                return To;
            else
                return null;
        }

        private MonthYearDTO SpecificMonthYear;
        public MonthYearDTO GetSpecificMonthYear()
        {
            if (SpecificMonthYear != null && SpecificMonthYear.IsValidMonthYear())
                return SpecificMonthYear;
            else
                return null;
        }

        /// <summary>
        /// Returns a tuple with From - To month year dtos
        /// If a specific month year is specified, from - to response will match that specific period.
        /// If any month - year period is not valid, e.g. a field value is missing, method returns null
        /// </summary>
        /// <returns></returns>
        public Tuple<MonthYearDTO, MonthYearDTO> GetPeriodToSearch()
        {
            if (GetSpecificMonthYear() != null)
                return new Tuple<MonthYearDTO, MonthYearDTO>(GetSpecificMonthYear(), GetSpecificMonthYear());
            else
                return new Tuple<MonthYearDTO, MonthYearDTO>(GetFromMonthYear(), GetToMonthYear());
        }
    }
    public class MonthYearDTO
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsValidMonthYear() => Month.IsBetween(1, 12) && Year > 0;        
    }
}
