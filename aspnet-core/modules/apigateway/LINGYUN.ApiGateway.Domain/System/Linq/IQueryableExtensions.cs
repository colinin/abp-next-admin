using System.Collections.Generic;

namespace System.Linq
{
    public static class IQueryableExtensions
    {
        private const int _defaultPageNumber = 0;
        /// <summary>
        /// Linq to Ef 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public static IQueryable<T> EfPageBy<T>(this IQueryable<T> source, int pageNumber = 1, int pageSize = 100)
        {
            pageNumber = pageNumber - 1;
            if (pageNumber < 0)
            {
                pageNumber = _defaultPageNumber;
            }
            return source.Skip(pageNumber * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 查询结果去重复
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TKey>(this IQueryable<T> source, Func<T, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(g => g.First());
        }

        /// <summary>
        /// 去除重复键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, Tkey>(this IEnumerable<T> source, Func<T, Tkey> keySelector)
        {
            return source.GroupBy(keySelector).Select(g => g.First());
        }
    }
}
