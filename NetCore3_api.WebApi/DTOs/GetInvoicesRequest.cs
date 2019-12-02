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
        public GetInvoicesRequest() { }
        public GetInvoicesRequest(int? fromMonth, int? fromYear, int? toMonth, int? toYear, int? specificMonth, int? specificYear)
        {
            FromMonth = fromMonth;
            FromYear = fromYear;
            ToMonth = toMonth;
            ToYear = toYear;
            SpecificMonth = specificMonth;
            SpecificYear = specificYear;
        }

        public int? FromMonth;
        public int? FromYear;
        public MonthYearDTO GetFromMonthYear()
        {
            MonthYearDTO m = new MonthYearDTO() { Month = FromMonth ?? 0, Year = FromYear ?? 0 };
            if (m.IsValidMonthYear())
                return m;
            else
                return null;
        }

        public int? ToMonth;
        public int? ToYear;
        public MonthYearDTO GetToMonthYear()
        {
            MonthYearDTO m = new MonthYearDTO() { Month = ToMonth ?? 0, Year = ToYear ?? 0 };
            if (m.IsValidMonthYear())
                return m;
            else
                return null;
        }

        public int? SpecificMonth;
        public int? SpecificYear;

        public MonthYearDTO GetSpecificMonthYear()
        {
            MonthYearDTO m = new MonthYearDTO() { Month = SpecificMonth ?? 0, Year = SpecificYear ?? 0 };
            if (m.IsValidMonthYear())
                return m;
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
