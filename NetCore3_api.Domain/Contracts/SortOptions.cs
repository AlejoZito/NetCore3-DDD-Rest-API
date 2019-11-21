using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Contracts
{
    public class SortOptions
    {
        public SortOptions(string column, SortOrder order)
        {
            Column = column;
            Order = order;
        }

        /// <summary>
        /// Column to sort by
        /// </summary>
        public string Column { get; set; }
        public SortOrder Order { get; set; }

        /// <summary>
        /// Get default sorting order: descending by "Id" column
        /// </summary>
        /// <returns></returns>
        public static SortOptions GetDefaultValue()
        {
            return new SortOptions(nameof(Entity.Id), SortOrder.Descending);
        }
    }
    public enum SortOrder
    {
        Ascending,
        Descending
    }
}
