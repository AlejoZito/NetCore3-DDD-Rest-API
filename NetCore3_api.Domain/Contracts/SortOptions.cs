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
    public class SortOrderHelper
    {
        public static bool TryParse(string sortOrderString, out SortOrder parsedSortOrder)
        {
            if (sortOrderString.ToUpper() == "ASC" || sortOrderString.ToUpper() == SortOrder.Ascending.ToString().ToUpper())
            {
                parsedSortOrder = SortOrder.Ascending;
                return true;
            }
            else if (sortOrderString.ToUpper() == "DESC" || sortOrderString.ToUpper() == SortOrder.Descending.ToString().ToUpper())
            {
                parsedSortOrder = SortOrder.Descending;
                return true;
            }
            else
                parsedSortOrder = default(SortOrder);
                return false;
        }
    }
    public enum SortOrder
    {
        Ascending,
        Descending
    }
}
