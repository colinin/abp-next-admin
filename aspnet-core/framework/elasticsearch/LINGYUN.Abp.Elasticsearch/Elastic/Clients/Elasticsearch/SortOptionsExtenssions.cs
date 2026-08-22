using System;
using System.Collections.Generic;
using System.Linq;

namespace Elastic.Clients.Elasticsearch;

public static class SortOptionsExtenssions
{
    public static IEnumerable<SortOptions>? ReverseSort(this IEnumerable<SortOptions>? sortOptions)
    {
        if (sortOptions == null)
        {
            return sortOptions;
        }
        return sortOptions.Select(sort =>
        {
            if (sort.Field != null)
            {
                sort.Field.Order = sort.Field.Order == SortOrder.Asc
                    ? SortOrder.Desc
                    : SortOrder.Asc;
            }
            return sort;
        }).Reverse();
    }
}
